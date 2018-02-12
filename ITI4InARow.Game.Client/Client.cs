using System;
using System.Windows.Forms;
using ITI4InARow.Module.Core;
using ITI4InARow.Module.Client;

namespace ITI4InARow.Game.Client
{
    public partial class Client : Form
    {
        private GameClient m_Client;
        private GameUpdateMessage m_GameMove;
        private RoomsForm m_RoomsForm;
        public Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void _MenuItemConnect_Click(object sender, EventArgs e)
        {
            ConnectForm form = new ConnectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.m_Client = new GameClient();
                this.m_RoomsForm = new RoomsForm(this.m_Client);
                this.m_Client.ClientStatusChanged += new EventHandler<ClientActionEventArgs>(this.Client_ClientStatusChanged);
                this.m_Client.GameUpdateMessage += new EventHandler<GameUpdateMessage>(this.Client_GameUpdateMessage);
                this.m_Client.ConnectClient(form.IPAddress, form.Port);
                ProfileUpdateMessage message = new ProfileUpdateMessage
                {
                    Name = form.NickName
                };
                this.m_Client.SendMessageToServer(message);
            }
        }

        private void _MenuItemDisconnect_Click(object sender, EventArgs e)
        {
            this.m_Client.ClientStatusChanged -= new EventHandler<ClientActionEventArgs>(this.Client_ClientStatusChanged);
            this.m_Client.GameUpdateMessage -= new EventHandler<GameUpdateMessage>(this.Client_GameUpdateMessage);
            this.m_Client.DisconnectClient();
            this.m_RoomsForm.UnloadForm();
            this.m_RoomsForm = null;
            this.m_Client = null;
        }

        private void _MenuItemSRooms_Click(object sender, EventArgs e)
        {
            this.m_RoomsForm.ShowDialog();
        }

        private void btn_GameMove_Click(object sender, EventArgs e)
        {
            this.m_Client.SendMessageToServer(this.m_GameMove);
            this.btn_GameMove.Enabled = false;
        }

        private void btn_LeaveGame_Click(object sender, EventArgs e)
        {
            this.m_GameMove.UpdateStatus = GameUpdateStatus.GameLeave;
            this.m_Client.SendMessageToServer(this.m_GameMove);
        }

        private void Client_ClientStatusChanged(object sender, ClientActionEventArgs e)
        {
            switch (e.Status)
            {
                case ClientStatus.ConnectionError:
                    MessageBox.Show("Connection Error");
                    break;

                case ClientStatus.ClientConnected:
                    this._MenuItemConnect.Enabled = false;
                    this._MenuItemDisconnect.Enabled = true;
                    this.Text = "Client - Connected";
                    break;

                case ClientStatus.ClientDisconnected:
                    this._MenuItemDisconnect.Enabled = false;
                    this._MenuItemConnect.Enabled = true;
                    this.SwitchToIdleMode();
                    this.Text = "Client - Disconnected";
                    break;
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_Client != null)
            {
                this._MenuItemDisconnect_Click(null, null);
            }
        }

        private void Client_GameUpdateMessage(object sender, GameUpdateMessage e)
        {
            switch (e.UpdateStatus)
            {
                case GameUpdateStatus.GameStarted:
                    this.SwitchToGamingMode();
                    break;

                case GameUpdateStatus.PlayerMove:
                    this.btn_GameMove.Enabled = true;
                    this.m_GameMove = e;
                    break;

                case GameUpdateStatus.GameLeave:
                    this.m_GameMove = null;
                    this.SwitchToIdleMode();
                    break;
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            this.panel_GameSurface.Hide();
        }

        private void SwitchToGamingMode()
        {
            this.m_RoomsForm.Hide();
            this.panel_GameSurface.Show();
            this.btn_GameMove.Enabled = false;
            this.m_GameMove = null;
        }

        private void SwitchToIdleMode()
        {
            if (this.m_RoomsForm != null)
            {
                this.m_RoomsForm.Show();
            }
            this.panel_GameSurface.Hide();
        }
    }
}
