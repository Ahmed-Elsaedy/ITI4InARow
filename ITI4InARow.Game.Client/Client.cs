using System;
using System.Windows.Forms;
using ITI4InARow.Module.Core;
using ITI4InARow.Module.Client;
using System.Drawing;
using Microsoft.VisualBasic.PowerPacks;

namespace ITI4InARow.Game.Client
{
    public partial class Client : Form
    {
        private GameClient m_Client;
        private static GameUpdateMessage m_GameMove;
        private RoomsForm m_RoomsForm;
        private Color ChosenColor;
        public Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            panel_GameSurface.PlayerAction += Panel_GameSurface_PlayerAction;
            panel_GameSurface.BorderStyle = BorderStyle.None;
        }

        private void Panel_GameSurface_PlayerAction(object sender, OvalShape myShape)
        {
            try
            {
                m_GameMove.TokenPosition = (int)myShape.Tag;
                m_GameMove.MsgType = MessageType.GameUpdateMessage;
                m_GameMove.Player2Color = panel_GameSurface.player1Color.ToArgb().ToString(); 
                m_GameMove.UpdateStatus = GameUpdateStatus.PlayerMove;
                m_Client.SendMessageToServer(m_GameMove);
                panel_GameSurface.isGameRunning = false;
            }
            catch (NullReferenceException)
            {
                
                MessageBox.Show("Player Move Invalid", "Game Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void _MenuItemConnect_Click(object sender, EventArgs e)
        {
            ConnectForm form = new ConnectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                m_Client = new GameClient();
                m_RoomsForm = new RoomsForm(m_Client);
                ChosenColor = form.clientcolor;
                m_Client.ClientStatusChanged += new EventHandler<ClientActionEventArgs>(Client_ClientStatusChanged);
                m_Client.GameUpdateMessage += new EventHandler<GameUpdateMessage>(Client_GameUpdateMessage);
                m_Client.ConnectClient(form.IPAddress, form.Port);
                ProfileUpdateMessage message = new ProfileUpdateMessage
                {
                    PlayerName = form.NickName,
                    PlayerColor = ChosenColor.ToArgb().ToString()
                };
                m_Client.SendMessageToServer(message);
            }
        }
        private void _MenuItemDisconnect_Click(object sender, EventArgs e)
        {
            m_Client.DisconnectClient();
            m_Client.ClientStatusChanged -= new EventHandler<ClientActionEventArgs>(Client_ClientStatusChanged);
            m_Client.GameUpdateMessage -= new EventHandler<GameUpdateMessage>(Client_GameUpdateMessage);
            m_RoomsForm.UnloadForm();
            m_RoomsForm = null;
            m_Client = null;
        }
        private void _MenuItemSRooms_Click(object sender, EventArgs e)
        {
            try
            {
                m_RoomsForm.ShowDialog();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You are not connected Yet", "New Room Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_LeaveGame_Click(object sender, EventArgs e)
        {
            m_GameMove.UpdateStatus = GameUpdateStatus.GameLeave;
            m_Client.SendMessageToServer(m_GameMove);
            m_RoomsForm.EnableNewButton();
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
                    Text = "Client - Connected";
                    break;

                case ClientStatus.ClientDisconnected:
                    this._MenuItemDisconnect.Enabled = false;
                    this._MenuItemConnect.Enabled = true;
                    SwitchToIdleMode();
                    Text = "Client - Disconnected";
                    break;
                    //sending messages 
                    //process incoming messages 
            }
        }
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Client != null)
            {
                _MenuItemDisconnect_Click(null, null);
            }
        }
        private void Client_GameUpdateMessage(object sender, GameUpdateMessage e)
        {
            switch (e.UpdateStatus)
            {
                //handling messages from server during the game
                case GameUpdateStatus.GameStarted:
                    SwitchToGamingMode();
                    panel_GameSurface.player2Color = Color.FromArgb(int.Parse(e.Player2Color));
                    if (e.IsGameRunning)
                    {
                        panel_GameSurface.isGameRunning = true;
                        panel_GameSurface.HighlightedPlayer();
                    }
                    else if (!e.IsGameRunning)
                    {
                        panel_GameSurface.isGameRunning = false;
                    }
                    m_GameMove = e;
                    break;

                case GameUpdateStatus.PlayerMove:
                    m_GameMove = e;
                    panel_GameSurface.Apply_Other_Client_Action(m_GameMove.TokenPosition, Color.FromArgb(int.Parse(e.Player2Color)));
                    break;

                case GameUpdateStatus.MakeYourMove:
                    panel_GameSurface.isGameRunning = true;
                    break;

                case GameUpdateStatus.GameDraw:
                    MessageBox.Show("draw");
                    break;

                case GameUpdateStatus.lose:
                    MessageBox.Show("you lose");
                    panel_GameSurface.isGameRunning = false;
                    break;

                case GameUpdateStatus.win:
                    MessageBox.Show("you win");
                    panel_GameSurface.isGameRunning = false;
                    break;

                case GameUpdateStatus.GameLeave:
                    m_GameMove = null;
                    SwitchToIdleMode();
                    m_RoomsForm.EnableNewButton();
                    break;
                case GameUpdateStatus.SpectatorJoin:
                    panel_GameSurface.fillcolorsforspectetorjoin(e.viewSpectatorBoard);
                    SwitchToGamingMode();
                    panel_GameSurface.Cursor = Cursors.No;
                    break;
                case GameUpdateStatus.viewMoveToSpectator:
                    panel_GameSurface.fillcolorsforspectetorjoin(e.viewSpectatorBoard);
                    break;
                case GameUpdateStatus.therisWinner:
                    MessageBox.Show($"{e.PlayerID} win");
                    break;
            }
        }
        private void Client_Load(object sender, EventArgs e)
        {
            panel_GameSurface.Hide();
        }
        private void SwitchToGamingMode()
        {
            m_RoomsForm.Hide();
            panel_GameSurface.Show();
            panel_GameSurface.player1Color = ChosenColor;
        }
        private void SwitchToIdleMode()
        {
            panel_GameSurface.Hide();
            panel_GameSurface.BoardReset();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        class MenuColorTable : ProfessionalColorTable
        {
            public MenuColorTable()
            {
               
                base.UseSystemColors = false;
            }
            public override System.Drawing.Color MenuBorder
            {
                get { return Color.Black; }
            }
            public override System.Drawing.Color MenuItemBorder
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color MenuItemSelected
            {
                get { return Color.Red; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.Red; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.Red; }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.Red; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.Black; }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return Color.Black; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return Color.Black; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return Color.Black; }
            }
        }
    }
}