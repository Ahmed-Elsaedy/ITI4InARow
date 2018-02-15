﻿using System;
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
                //m_GameMove.MsgType = MessageType.GameUpdateMessage;
                m_GameMove.UpdateStatus = GameUpdateStatus.PlayerMove;
                m_Client.SendMessageToServer(m_GameMove);
                panel_GameSurface.Enabled = false;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Wrong Move _playeAction Method", "Game Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    PlayerName = form.NickName
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
<<<<<<< HEAD
        }

=======

        }
        //private void btn_GameMove_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendMessageToServer(m_GameMove);
        //    //btn_GameMove.Enabled = false;
        //}
>>>>>>> 63503665c0817b696a0a609f7197cb2c3d9c1078
        private void btn_LeaveGame_Click(object sender, EventArgs e)
        {
            m_GameMove.UpdateStatus = GameUpdateStatus.GameLeave;
            m_Client.SendMessageToServer(m_GameMove);
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
                    //case sending clint masseg
                    //prossesincomemassege
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
                //handling msgs from server during the game
                case GameUpdateStatus.GameStarted:
                    SwitchToGamingMode();
                    if (e.IsGameRunning)
                    {
                        panel_GameSurface.Enabled = true;
                    }
                    else
                    {
                        panel_GameSurface.Enabled = false;
                    }
                    m_GameMove = e;
                    break;

                case GameUpdateStatus.PlayerMove:
<<<<<<< HEAD
                    m_GameMove = e;
                    MessageBox.Show(m_GameMove.TokenPosition.ToString());
                    //amr ana hena 3ayez anady 3ala function te3mel el action 3ala el user control bta3na 
                    panel_GameSurface.applay_Other_Clint_Action(m_GameMove.TokenPosition);
                    //apply the action that come from server 
=======
                    panel_GameSurface.Enabled = true;                     
                    if (e.TokenPosition>=0)
                    {
                        MessageBox.Show(m_GameMove.TokenPosition.ToString());
                    }
                    //apply the action that come from server 
                    m_GameMove = e;
                    
                    //apaly the action that come from server 
>>>>>>> 63503665c0817b696a0a609f7197cb2c3d9c1078
                    break;

                case GameUpdateStatus.GameLeave:
                    m_GameMove = null;
                    SwitchToIdleMode();
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
            //btn_GameMove.Enabled = false;
            //m_GameMove = null;
        }
        private void SwitchToIdleMode()
        {
            panel_GameSurface.Hide();
        }
    }
}
