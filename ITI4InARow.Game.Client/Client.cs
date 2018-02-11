﻿using System;
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
            CheckForIllegalCrossThreadCalls = false;
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
                    _stl_Connection.Text = "Connected";
                    break;
                case ClientStatus.ClientDisconnected:
                    _MenuItemDisconnect.Enabled = false;
                    _MenuItemConnect.Enabled = true;
                    Text = "Client - Disconnected";
                    _stl_Connection.Text = "Disconnected";
                    break;
            }
        }
        private void _MenuItemConnect_Click(object sender, EventArgs e)
        {
            ConnectForm cForm = new ConnectForm();
            DialogResult result = cForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_Client = new GameClient();
                m_Client.ClientStatusChanged += Client_ClientStatusChanged;
                m_Client.MessageRecieved += Client_MessageRecieved;
                m_Client.ConnectClient(cForm.IPAddress, cForm.Port);
            }
        }
        private void _MenuItemDisconnect_Click(object sender, EventArgs e)
        {
            m_Client.DisconnectClient();
            m_Client.ClientStatusChanged -= Client_ClientStatusChanged;
            m_Client.MessageRecieved -= Client_MessageRecieved;
        }
        private void Client_MessageRecieved(object sender, MessageRevievedEventArgs e)
        {
            _stl_Connection.Text = $"Connected - ({e.ClientHandle})";

            switch (e.Message.MsgType.Name)
            {
                case "ListofRoomsMessage":

                    ListofRoomsMessage mylist = (ListofRoomsMessage)e.Message;
                    for (int i = 0; i < mylist.availableRooms.Count; i++)
                    {
                        ListViewItem room = new ListViewItem();
                        room.SubItems.Add(mylist.availableRooms[i].RoomID.ToString());
                        room.SubItems.Add(mylist.availableRooms[i].name);
                        room.SubItems.Add(mylist.availableRooms[i].roomState.ToString());
                        if (mylist.availableRooms[i].isSpectator)
                        {
                            room.SubItems.Add("view available");
                        }
                        else
                        {
                            room.SubItems.Add("view Not available");
                        }

                        listView1.Items.Add(room);
                    }
                    break;
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Client.DisconnectClient();
            m_Client.ClientStatusChanged -= Client_ClientStatusChanged;
            m_Client.MessageRecieved -= Client_MessageRecieved;
        }
    }
}
