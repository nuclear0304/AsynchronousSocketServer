using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsynchronousSocketServer
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SocketServerHelper c = new SocketServerHelper(textBox_Ip.Text, Convert.ToInt16(textBox_Port.Text));
            c.Connstat = WriteToConnstat;
            c.Exclog = WriteToExclog;
            c.Recvlog = WriteToRecvlog;
            c.StartServer();

            //SocketServerNX c = new SocketServerNX(textBox_Ip.Text, Convert.ToInt16(textBox_Port.Text));
            //c.runningStatus = WriteToConnstat;
            //c.SocketReceiveEvent += ReciveEvent;
            //c.StartServer();
        }

        private string ReciveEvent(string msg)
        {
            return "ack";
        }

        private void WriteToConnstat(string s)
        {
            Invoke(new Action<string>((x) => { textBox_ConnState.AppendText(x); }), $"{DateTime.Now.ToString("HH:mm:ss")}->{s}{Environment.NewLine}");
        }

        private void WriteToExclog(string s)
        {
            Invoke(new Action<string>((x) => { textBox_Exclog.AppendText(x); }), $"{DateTime.Now.ToString("HH:mm:ss")}->{s}{Environment.NewLine}");
        }

        private void WriteToRecvlog(string s)
        {
            Invoke(new Action<string>((x) => { textBox_Recvlog.AppendText(x); }), s + Environment.NewLine);
        }
    }
}
