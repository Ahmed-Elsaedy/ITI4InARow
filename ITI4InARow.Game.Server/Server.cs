using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using ITI4InARow.Module.Server;
using ITI4InARow.Module.Core;

namespace ITI4InARow.Game.Server
{
    public partial class Server : Form
    {
        GameServer m_Server;
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            string hostName = Dns.GetHostName();
            m_Server = new GameServer(new byte[] { 172, 16, 5, 18 }, 63210);
            m_Server.ServerStatusChanged += Server_ServerStatusChanged;
        }
        private void Server_ServerStatusChanged(object sender, ServerActionEventArgs e)
        {
            switch (e.Status)
            {
                case ServerStatus.ServerStarted:
                    _MenuItemStart.Enabled = false;
                    _MenuItemStop.Enabled = true;
                    Text = "Server - Started";
                    break;
                case ServerStatus.ServerStopCancelled:
                    MessageBox.Show("Stop Cancelled By Server");
                    break;
                case ServerStatus.ServerStopped:
                    _MenuItemStop.Enabled = false;
                    _MenuItemStart.Enabled = true;
                    Text = "Server - Stopped";
                    break;
                case ServerStatus.ClientConnected:
                    _lbxConnectClients.Items.Add(e.Client);
                    break;
                case ServerStatus.ClientDisconnected:
                    var target = _lbxConnectClients.Items.Cast<ServerClient>().Single(x => x.ClientID == e.Client.ClientID);
                    _lbxConnectClients.Items.Remove(target);
                    break;
            }
            _lbxOutput.Items.Add(e);
        }
        private void _MenuItemStart_Click(object sender, EventArgs e)
        {
            m_Server.StartServer();
            lblIP.Text = Helper.GetLocalIP().ToString();
        }
        private void _MenuItemStop_Click(object sender, EventArgs e)
        {
            m_Server.StopServer();
        }
        private void _MenuItemSettings_Click(object sender, EventArgs e)
        {

        }
        private void _MenuItemClear_Click(object sender, EventArgs e)
        {
            _lbxOutput.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPAddress(new byte[] { 127, 0, 0, 1 }), 5031);
            //NetworkStream nStream = client.GetStream();
            //byte[] outStream = Encoding.ASCII.GetBytes("mY mESSAGE");
            //nStream.Write(outStream, 0, outStream.Length);
            //nStream.Flush();

            //byte[] inStream = new byte[10025];
            //nStream.Read(inStream, 0, outStream.Length);
            //string data = Encoding.ASCII.GetString(inStream);
            //nStream.Dispose();
            //client.Close();

        }
    }
}
