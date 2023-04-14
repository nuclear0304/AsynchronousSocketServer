using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AsynchronousSocketServer
{
    /// <summary>
    /// APM (Asynchronous Programming Model Pattern) 非同步SocketServer
    /// </summary>
    class SocketServerHelper
    {
        public SocketServerHelper(string ip, int port)
        {
            Ip = ip;
            Port = port;
            SocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private string Ip { get; set; }
        private int Port { get; set; }
        private Socket SocketListener { get; set; }

        public Action<string> Connstat;
        public Action<string> Exclog;
        public Action<string> Recvlog;
        public Action<string> RecvAction { get; set; }
        public delegate void RecvEvent(EventArgs e);
        public event RecvEvent OnRecv;

        private ManualResetEvent allDone = new ManualResetEvent(false);
        private AutoResetEvent receiveDone = new AutoResetEvent(false);
        private AutoResetEvent sendDone = new AutoResetEvent(false);

        /// <summary>
        /// 開始監聽
        /// </summary>
        public void StartServer()
        {
            try
            {
                SocketListener.Bind(new IPEndPoint(IPAddress.Parse(Ip), Port));
                SocketListener.Listen(100);
                Task.Run(() => StartSocketAsync());
                Connstat?.Invoke("Start listen.");
            }
            catch (ArgumentOutOfRangeException aore)
            {
                Exclog?.Invoke(aore.ToString());
            }
            catch (SocketException sec)
            {
                Exclog?.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                Exclog?.Invoke(ex.ToString());
            }
        }

        /// <summary>
        /// 開始非同步等待連入
        /// </summary>
        public void StartSocketAsync()
        {
            try
            {
                while (true)
                {
                    allDone.Reset();
                    SocketListener.BeginAccept(new AsyncCallback(AcceptCallback), SocketListener);
                    allDone.WaitOne();
                }
            }
            catch (SocketException sec)
            {
                throw sec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                allDone.Set();
                Connstat?.Invoke("Incoming connection.");
                Connstat?.Invoke("New Accept.");
                Socket socket = (Socket)ar.AsyncState;
                socket = socket.EndAccept(ar);

                ReceiveObj receiveObj = new ReceiveObj();
                receiveObj.RecvSocket = socket;
                socket.BeginReceive(receiveObj.Buffer, 0, receiveObj.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), receiveObj);
                receiveDone.WaitOne(3000);
            }
            catch (SocketException sec)
            {
                Exclog?.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                Exclog?.Invoke(ex.ToString());
            }
        }

        /// <summary>
        /// 非同步接收資料
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ReceiveObj receiveObj = (ReceiveObj)ar.AsyncState;
                Socket handler = receiveObj.RecvSocket;

                var recvlen = handler.EndReceive(ar);
                receiveDone.Set();
                var recvaddr = handler.RemoteEndPoint;
                if (recvlen > 0)
                {
                    var temstr = Encoding.ASCII.GetString(receiveObj.Buffer, 0, recvlen);
                    //收到資料存入StringBuilder
                    receiveObj.Sb.Append(temstr);
                    RecvAction?.Invoke(temstr);
                    //OnRecv?.Invoke(new RecvEventObj(temstr));
                    if (temstr.IndexOf("<EOF>") > -1)
                    {//收到結束字串, 結束通訊
                        SendObj sendObj = new SendObj
                        {
                            SendSocket = receiveObj.RecvSocket,
                            Msg = "bye.",
                            Eof = true
                        };
                        SocketSend(sendObj);
                        string strContent = receiveObj.Sb.ToString();
                        Recvlog?.Invoke($"{recvaddr}->{strContent}");
                        //var str = Encoding.UTF8.GetString(receiveObj.Buffer, 0, recvlen);
                    }
                    else
                    {
                        SendObj sendObj = new SendObj
                        {
                            SendSocket = receiveObj.RecvSocket,
                            Msg = "ack.",
                            Eof = false
                        };
                        SocketSend(sendObj);
                        receiveObj.RecvSocket.BeginReceive(receiveObj.Buffer, 0, receiveObj.BufferSize, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), receiveObj);
                        receiveDone.WaitOne(3000);
                    }
                }
            }
            catch (SocketException sec)
            {
                Exclog?.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                Exclog?.Invoke(ex.ToString());
            }
        }

        private void SocketSend(SendObj sendObj)
        {
            try
            {
                Socket socket = sendObj.SendSocket;
                byte[] sendbyte = Encoding.ASCII.GetBytes(sendObj.Msg);
                socket.BeginSend(sendbyte, 0, sendbyte.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendObj);
                sendDone.WaitOne(1000);
            }
            catch (SocketException sec)
            {
                Exclog?.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                Exclog?.Invoke(ex.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                SendObj sendObj = (SendObj)ar.AsyncState;
                Socket socket = sendObj.SendSocket;
                socket.EndSend(ar);
                sendDone.Set();
                if (sendObj.Eof)
                {
                    ConnectionClose(socket);
                }
            }
            catch (SocketException sec)
            {
                Exclog?.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                Exclog?.Invoke(ex.ToString());
            }
        }

        private void ConnectionClose(Socket socket)
        {
            try
            {
                socket?.Close();
            }
            catch (SocketException sec)
            {
                throw sec;
            }
            catch (Exception)
            {
                throw;
            }
        }

        class ReceiveObj
        {
            public ReceiveObj()
            {
                Buffer = new byte[16384];
                BufferSize = 16384;
                Sb = new StringBuilder();
            }

            public byte[] Buffer { get; set; }
            public int BufferSize { get; set; }
            public StringBuilder Sb { get; set; }
            public Socket RecvSocket { get; set; }
        }

        class SendObj
        {
            public bool Eof { get; set; }
            public string Msg { get; set; }
            public Socket SendSocket { get; set; }
        }

        public class RecvEventObj : EventArgs
        {
            public RecvEventObj(string message)
            {
                Message = message;
            }

            public string Message { get; set; }
        }
    }
}
