using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AsynchronousSocketServer
{
    class SocketServerNX
    {
        /// <summary>
        /// APM (Asynchronous Programming Model Pattern) 非同步SocketServerNX
        /// 新版加入非同步接收Timeout機制
        /// </summary>
        public SocketServerNX(string ip, int port)
        {
            Ip = ip;
            Port = port;
            SocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private string Ip { get; set; }
        private int Port { get; set; }
        private Socket SocketListener { get; set; }

        /// <summary>
        /// 運行資訊
        /// </summary>
        public Action<string> runningStatus;

        /// <summary>
        /// Socket接收資料事件
        /// </summary>
        public event Func<string, string> SocketReceiveEvent;

        private ManualResetEvent allDone = new ManualResetEvent(false);

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
                runningStatus?.Invoke($"Start listen at {Ip}:{Port}");
            }
            catch (FormatException fe)
            {
                throw fe;
            }
            catch (ArgumentOutOfRangeException aore)
            {
                throw aore;
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

        /// <summary>
        /// 開始非同步等待連入
        /// </summary>
        private void StartSocketAsync()
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 開始非同步等待接收
        /// </summary>
        private async void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                runningStatus?.Invoke("Incoming connection.");
                allDone.Set();

                Socket socket = (Socket)ar.AsyncState;
                socket = socket.EndAccept(ar);

                // 有新的連線後,將狀態透過SocketHandler保存
                SocketHandler socketHandler = new SocketHandler(socket, runningStatus);
                socketHandler.SocketReceiveEvent += SocketReceiveEvent;
                socketHandler.StartReceive();
            }
            catch (SocketException sec)
            {
                runningStatus.Invoke(sec.ToString());
            }
            catch (Exception ex)
            {
                runningStatus.Invoke(ex.ToString());
            }
        }
    }

    /// <summary>
    /// 連線狀態保存
    /// </summary>
    class SocketHandler
    {
        public SocketHandler(Socket socket, Action<string> runningstatus)
        {
            _timeout = false;
            _createdTime = DateTime.Now;
            _socket = socket;
            runningStatus = runningstatus;

            timeoutTimer = new System.Timers.Timer();
            timeoutTimer.Elapsed += TimeoutTimer_Elapsed;
            timeoutTimer.Interval = 1000;
            timeoutTimer.Start();
        }

        /// <summary>
        /// Socket接收資料事件
        /// </summary>
        public event Func<string, string> SocketReceiveEvent;

        /// <summary>
        /// 運行資訊
        /// </summary>
        private Action<string> runningStatus;

        private AutoResetEvent sendDone = new AutoResetEvent(false);
        private AutoResetEvent receiveDone = new AutoResetEvent(false);

        public Socket _socket;
        private bool _timeout;
        public DateTime _createdTime;

        private System.Timers.Timer timeoutTimer;

        /// <summary>
        /// 偵測是否接收timeout
        /// </summary>
        private void TimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if ((DateTime.Now - _createdTime).TotalSeconds > 120)
            {
                _timeout = true;
                _socket.Close();
                _socket.Dispose();
                timeoutTimer.Stop();
                timeoutTimer.Dispose();
            }
        }

        /// <summary>
        /// 等待接收
        /// </summary>
        public void StartReceive()
        {
            ReceiveObj receiveObj = new ReceiveObj();
            receiveObj.RecvSocket = _socket;
            _socket.BeginReceive(receiveObj.Buffer, 0, receiveObj.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), receiveObj);
            receiveDone.WaitOne(1000);
        }

        /// <summary>
        /// 非同步接收資料
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            ReceiveObj receiveObj = null;
            Socket handler = null;
            try
            {
                if (_timeout)
                {
                    runningStatus.Invoke("Connection timeout.");
                    return;
                }

                receiveObj = (ReceiveObj)ar.AsyncState;
                handler = receiveObj.RecvSocket;

                var recvlen = handler.EndReceive(ar);
                receiveDone.Set();

                // 接收訊息
                var recvaddr = handler.RemoteEndPoint;
                var msg = Encoding.ASCII.GetString(receiveObj.Buffer, 0, recvlen);

                runningStatus.Invoke($"{recvaddr} -> {msg}");

                // 回應訊息變數
                string return_msg = string.Empty;
                // 結束連線旗標
                bool have_to_close = false;

                if (SocketReceiveEvent != null)
                {
                    var result = SocketReceiveEvent.Invoke(msg);
                    return_msg = result; // 回應給client的訊息
                }

                var a = msg.IndexOf("<EOF>");

                if (msg.IndexOf("<EOF>") > -1)
                    have_to_close = true;

                // 回覆訊息給client
                if (return_msg != string.Empty)
                {
                    SendObj sendObj = new SendObj();
                    sendObj.SendSocket = receiveObj.RecvSocket;
                    sendObj.Msg = return_msg;
                    SocketSend(sendObj);
                    runningStatus.Invoke($"{recvaddr} -< {return_msg}");
                }

                // 是否結束socket連線, 或是等待下次接收資料
                if (have_to_close)
                {
                    ConnectionClose(handler);
                }
                else
                {
                    receiveObj.RecvSocket.BeginReceive(receiveObj.Buffer, 0, receiveObj.BufferSize, SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), receiveObj);
                    receiveDone.WaitOne(1000);
                }
            }
            catch (Exception ex)
            {
                ConnectionClose(handler);
                runningStatus.Invoke(ex.ToString());
            }
            finally
            {
                runningStatus.Invoke(Environment.NewLine);
            }
        }

        /// <summary>
        /// 發送資料
        /// </summary>
        private void SocketSend(SendObj sendObj)
        {
            try
            {
                Socket socket = sendObj.SendSocket;
                byte[] sendbyte = Encoding.ASCII.GetBytes(sendObj.Msg);
                socket.BeginSend(sendbyte, 0, sendbyte.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendObj);
                sendDone.WaitOne(1000);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 開始非同步發送資料
        /// </summary>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                SendObj sendObj = (SendObj)ar.AsyncState;
                Socket socket = sendObj.SendSocket;
                socket.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception ex)
            {
                runningStatus.Invoke(ex.ToString());
            }
        }

        /// <summary>
        /// 關閉Socket連線
        /// </summary>
        /// <param name="socket"></param>
        private void ConnectionClose(Socket socket)
        {
            try
            {
                socket?.Close();
                runningStatus.Invoke("ConnectionClose.");
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
                Buffer = new byte[32768];
                BufferSize = 32768;
                Sb = new StringBuilder();
            }

            public byte[] Buffer { get; set; }
            public int BufferSize { get; set; }
            public StringBuilder Sb { get; set; }
            public Socket RecvSocket { get; set; }
        }

        class SendObj
        {
            public string Msg { get; set; }
            public Socket SendSocket { get; set; }
        }
    }
}
