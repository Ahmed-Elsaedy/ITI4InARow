using System;
using System.Windows.Forms;
using ITI4InARow.Module.Core;
using ITI4InARow.Module.Client;

namespace ITI4InARow.Game.Client
{
    public partial class Client : Form
    {
        GameClient m_Client;
        public Client()
        {
            InitializeComponent();
            m_Client = new GameClient();
            m_Client.ClientStatusChanged += Client_ClientStatusChanged;
        }
        private void Client_ClientStatusChanged(object sender, ClientActionEventArgs e)
        {
            switch (e.Status)
            {
                case ClientStatus.ConnectionError:
                    MessageBox.Show("Connection Error");
                    break;
                case ClientStatus.ClientConnected:
                    _MenuItemConnect.Enabled = false;
                    _MenuItemDisconnect.Enabled = true;
                    Text = "Client - Connected";
                    break;
                case ClientStatus.ClientDisconnected:
                    _MenuItemDisconnect.Enabled = false;
                    _MenuItemConnect.Enabled = true;
                    Text = "Client - Disconnected";
                    break;
            }
        }
        private void _MenuItemConnect_Click(object sender, EventArgs e)
        {
            m_Client.ConnectClient(new byte[] { 172, 16, 5, 28 }, 5031);
        }
        private void _MenuItemDisconnect_Click(object sender, EventArgs e)
        {
            m_Client.DisconnectClient();
        }
        private void _MenuItemRegister_Click(object sender, EventArgs e)
        {
            ClientProfile profile = new ClientProfile();
            if (profile.ShowDialog() == DialogResult.OK)
            {
                m_Client.SendMessage(new RegisterMessage()
                {
                    Name = profile.ProfileName,
                    Age = profile.ProfileAge
                });
            }
        }
    }
}
