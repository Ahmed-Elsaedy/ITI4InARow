namespace ITI4InARow.Game.Client
{
    using ITI4InARow.Module.Client;
    using ITI4InARow.Module.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class RoomsForm : Form
    {
        private Button _btnNew;
        private GameClient _GameClient;
        private ListView _ListViewRooms;
        private RoomUpdateMessage _MyRoomUpdate;
        private List<RoomUpdateMessage> _RoomsUpdates;
        private Button btnCancel;
        private Button btnJoin;
        private Button btnView;
        private ColumnHeader col_Player1;
        private ColumnHeader col_Player2;
        private ColumnHeader col_RState;
        private ColumnHeader col_Viewers;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label2;
        private Label label3;
        private Label lbl_Pass;
        private NumericUpDown numRoomId;
        private Panel panel_Rooms;
        private Panel panel_Waiting;

        public RoomsForm(GameClient gameClient)
        {
            _GameClient = gameClient;
            InitializeComponent();
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            RoomUpdateMessage message1 = new RoomUpdateMessage
            {
                UpdateState = RoomUpdateState.NewRoomRequest
            };
            _MyRoomUpdate = message1;
            _GameClient.SendMessageToServer(_MyRoomUpdate);
            _btnNew.Enabled = false;
        }

        private void _ListViewRooms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateButtonsStatus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _MyRoomUpdate.UpdateState = RoomUpdateState.NewRoomRollback;
            _GameClient.SendMessageToServer(_MyRoomUpdate);
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            RoomUpdateMessage tag = (RoomUpdateMessage)_ListViewRooms.SelectedItems[0].Tag;
            if (((int)numRoomId.Value) == tag.RoomID)
            {
                tag.UpdateState = RoomUpdateState.Player2Connected;
                _GameClient.SendMessageToServer(tag);
            }
            else
            {
                MessageBox.Show("Wrong Room Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing); // exception hena lama bn2fl 3alatool 
        }

        private void GameClient_RoomUpdateMessage(object sender, RoomUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateState)
            {
                case RoomUpdateState.NewRoomRequest:
                    _MyRoomUpdate = msg;
                    _RoomsUpdates.Add(msg);
                    SwitchToWaitingMode(msg.RoomID.ToString());
                    break;

                case RoomUpdateState.NewRoomRollback:
                    msg.UpdateState = RoomUpdateState.NewRoomRollback;
                    _RoomsUpdates.Remove(_RoomsUpdates.Single<RoomUpdateMessage>(x => x.RoomID == msg.RoomID));
                    _MyRoomUpdate = null;
                    SwitchToRoomsMode();
                    break;

                case RoomUpdateState.Player2Connected:
                    message = _RoomsUpdates.Single<RoomUpdateMessage>(x => x.RoomID == msg.RoomID);
                    message.Player2ID = msg.Player2ID;
                    message.UpdateState = RoomUpdateState.RoomComplete;
                    _GameClient.SendMessageToServer(message);
                    break;

                case RoomUpdateState.Broadcast:
                    message = _RoomsUpdates.SingleOrDefault<RoomUpdateMessage>(x => x.RoomID == msg.RoomID);
                    if ((message == null) || (msg.Player1ID <= 0))
                    {
                        if ((message == null) && (msg.Player1ID > 0))
                        {
                            _RoomsUpdates.Add(msg);
                        }
                        else if ((message != null) && (msg.Player1ID == 0))
                        {
                            _RoomsUpdates.Remove(msg);
                        }
                        break;
                    }
                    message.Player1ID = msg.Player1ID;
                    message.Player2ID = msg.Player2ID;
                    break;
            }
            UpdateListViewItem();
            UpdateButtonsStatus();
        }

        private void InitializeComponent()
        {
            panel_Rooms = new System.Windows.Forms.Panel();
            panel_Waiting = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            lbl_Pass = new System.Windows.Forms.Label();
            numRoomId = new System.Windows.Forms.NumericUpDown();
            btnJoin = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            _ListViewRooms = new System.Windows.Forms.ListView();
            col_RState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            col_Player1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            col_Player2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            col_Viewers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            btnView = new System.Windows.Forms.Button();
            _btnNew = new System.Windows.Forms.Button();
            panel_Rooms.SuspendLayout();
            panel_Waiting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(numRoomId)).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel_Rooms
            // 
            panel_Rooms.Controls.Add(panel_Waiting);
            panel_Rooms.Controls.Add(numRoomId);
            panel_Rooms.Controls.Add(btnJoin);
            panel_Rooms.Controls.Add(groupBox1);
            panel_Rooms.Controls.Add(btnView);
            panel_Rooms.Controls.Add(_btnNew);
            panel_Rooms.Location = new System.Drawing.Point(1, 1);
            panel_Rooms.Name = "panel_Rooms";
            panel_Rooms.Size = new System.Drawing.Size(450, 261);
            panel_Rooms.TabIndex = 0;
            // 
            // panel_Waiting
            // 
            panel_Waiting.Controls.Add(label3);
            panel_Waiting.Controls.Add(btnCancel);
            panel_Waiting.Controls.Add(label2);
            panel_Waiting.Controls.Add(lbl_Pass);
            panel_Waiting.Location = new System.Drawing.Point(0, 0);
            panel_Waiting.Name = "panel_Waiting";
            panel_Waiting.Size = new System.Drawing.Size(450, 258);
            panel_Waiting.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(163, 142);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(111, 13);
            label3.TabIndex = 7;
            label3.Text = "Waiting For Player 2...";
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(181, 165);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(79, 119);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(278, 13);
            label2.TabIndex = 5;
            label2.Text = "Please Inform Player 2 With Your New Room`s Password.";
            // 
            // lbl_Pass
            // 
            lbl_Pass.AutoSize = true;
            lbl_Pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_Pass.Location = new System.Drawing.Point(180, 70);
            lbl_Pass.Name = "lbl_Pass";
            lbl_Pass.Size = new System.Drawing.Size(77, 39);
            lbl_Pass.TabIndex = 4;
            lbl_Pass.Text = "153";
            // 
            // numRoomId
            // 
            numRoomId.Location = new System.Drawing.Point(15, 227);
            numRoomId.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            numRoomId.Name = "numRoomId";
            numRoomId.Size = new System.Drawing.Size(100, 20);
            numRoomId.TabIndex = 11;
            // 
            // btnJoin
            // 
            btnJoin.Location = new System.Drawing.Point(121, 226);
            btnJoin.Name = "btnJoin";
            btnJoin.Size = new System.Drawing.Size(75, 23);
            btnJoin.TabIndex = 8;
            btnJoin.Text = "Join";
            btnJoin.UseVisualStyleBackColor = true;
            btnJoin.Click += new System.EventHandler(btnJoin_Click);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(_ListViewRooms);
            groupBox1.Location = new System.Drawing.Point(9, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(426, 207);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Server Rooms";
            // 
            // _ListViewRooms
            // 
            _ListViewRooms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            col_RState,
            col_Player1,
            col_Player2,
            col_Viewers});
            _ListViewRooms.FullRowSelect = true;
            _ListViewRooms.Location = new System.Drawing.Point(6, 19);
            _ListViewRooms.MultiSelect = false;
            _ListViewRooms.Name = "_ListViewRooms";
            _ListViewRooms.Size = new System.Drawing.Size(414, 182);
            _ListViewRooms.TabIndex = 0;
            _ListViewRooms.UseCompatibleStateImageBehavior = false;
            _ListViewRooms.View = System.Windows.Forms.View.Details;
            // 
            // col_RState
            // 
            col_RState.Text = "State";
            col_RState.Width = 84;
            // 
            // col_Player1
            // 
            col_Player1.Text = "Player 1";
            col_Player1.Width = 120;
            // 
            // col_Player2
            // 
            col_Player2.Text = "Player 2";
            col_Player2.Width = 120;
            // 
            // col_Viewers
            // 
            col_Viewers.Text = "Viewers";
            col_Viewers.Width = 80;
            // 
            // btnView
            // 
            btnView.Location = new System.Drawing.Point(273, 226);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(75, 23);
            btnView.TabIndex = 9;
            btnView.Text = "Watch Game";
            btnView.UseVisualStyleBackColor = true;
            // 
            // _btnNew
            // 
            _btnNew.Location = new System.Drawing.Point(354, 226);
            _btnNew.Name = "_btnNew";
            _btnNew.Size = new System.Drawing.Size(75, 23);
            _btnNew.TabIndex = 10;
            _btnNew.Text = "New";
            _btnNew.UseVisualStyleBackColor = true;
            _btnNew.Click += new System.EventHandler(_btnNew_Click);
            // 
            // RoomsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(451, 261);
            Controls.Add(panel_Rooms);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RoomsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Server Rooms";
            Load += new System.EventHandler(RoomsForm_Load);
            panel_Rooms.ResumeLayout(false);
            panel_Waiting.ResumeLayout(false);
            panel_Waiting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(numRoomId)).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);

        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            /////////////////////////////////////////////////////////////////////////////
            /////////////////////-_-/////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////
            //this._MyRoomUpdate. = RoomUpdateState.Broadcast;
            //this._GameClient.SendMessageToServer(this._MyRoomUpdate);
            
        }

        private void RoomsForm_Load(object sender, EventArgs e)
        {
            _RoomsUpdates = new List<RoomUpdateMessage>();
            _GameClient.RoomUpdateMessage += new EventHandler<RoomUpdateMessage>(GameClient_RoomUpdateMessage);
            _ListViewRooms.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(_ListViewRooms_ItemSelectionChanged);
            SwitchToRoomsMode();
        }

        public void SwitchToRoomsMode()
        {
            panel_Waiting.Hide();
        }

        public void SwitchToWaitingMode(string pass)
        {
            lbl_Pass.Text = pass;
            panel_Waiting.Show();
        }

        public void UnloadForm()
        {
            _GameClient.RoomUpdateMessage -= new EventHandler<RoomUpdateMessage>(GameClient_RoomUpdateMessage);
            _ListViewRooms.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(_ListViewRooms_ItemSelectionChanged);
            base.Dispose();
        }

        private void UpdateButtonsStatus()
        {
            if (_ListViewRooms.SelectedItems.Count > 0)
            {
                RoomUpdateMessage tag = (RoomUpdateMessage)_ListViewRooms.SelectedItems[0].Tag;
                btnJoin.Enabled = numRoomId.Enabled = (tag.Player1ID != 0) && (tag.Player2ID == 0);
                btnView.Enabled = (tag.Player1ID != 0) && (tag.Player2ID > 0);
                _btnNew.Enabled = false;
            }
            else
            {
                btnJoin.Enabled = numRoomId.Enabled = false;
                btnView.Enabled = false;
                _btnNew.Enabled = _MyRoomUpdate == null;
            }
        }

        private void UpdateListViewItem()
        {
            _ListViewRooms.Items.Clear();
            foreach (RoomUpdateMessage message in _RoomsUpdates)
            {
                string[] items = new string[] { message.GetRoomStatus().ToString(), message.Player1ID.ToString(), message.Player2ID.ToString(), "0" };
                ListViewItem item = new ListViewItem(items)
                {
                    Tag = message
                };
                _ListViewRooms.Items.Add(item);
            }
        }
    }
}
