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
        private Button btnclose;
        List<RoomUpdateMessage> _RoomsUpdates = new List<RoomUpdateMessage>();

        public RoomsForm(GameClient gameClient)
        {
            _GameClient = gameClient;
            InitializeComponent();
            this.VisibleChanged += RoomsForm_VisibleChanged;
        }

        bool loadCachedMessage = false;
        

        private void RoomsForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true && !loadCachedMessage)
            {
                RoomUpdateMessage roomUpdate = new RoomUpdateMessage();
                roomUpdate.UpdateState = RoomUpdateState.AvailableRoomsBroadcast;
                _GameClient.SendMessageToServer(roomUpdate);
                loadCachedMessage = true;
            }
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
            _btnNew.Enabled = true;
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
                    _btnNew.Enabled = true;
                    break;

                case RoomUpdateState.Player2Connected:
                    RoomUpdateMessage message = _RoomsUpdates.Single(x => x.RoomID == msg.RoomID);
                    message.Player2ID = msg.Player2ID;
                    message.UpdateState = RoomUpdateState.RoomComplete;
                    _GameClient.SendMessageToServer(message);
                    break;
                case RoomUpdateState.Broadcast:
                     message = _RoomsUpdates.SingleOrDefault(x => x.RoomID == msg.RoomID);
                    if ((message == null) || (msg.Player1ID <= 0))
                    {
                        if ((message == null) && (msg.Player1ID > 0))
                        {
                            _RoomsUpdates.Add(msg);
                        }
                        else if ((message != null) && (msg.Player1ID == 0))
                        {
                            _RoomsUpdates.Remove(message);
                        }
                        break;
                    }
                    message.Player1ID = msg.Player1ID;
                    message.Player2ID = msg.Player2ID;
                    break;

                    /// remove or check deh abl ma neb3at lel bashmohandis
                case RoomUpdateState.newSpectatorReq:
                    message = _RoomsUpdates.SingleOrDefault(x => x.RoomID == msg.RoomID);
                    message.SpectatorsNum++;
                    break;
            }
            UpdateListViewItem();
            UpdateButtonsStatus();
        }

        internal void EnableNewButton() => _btnNew.Enabled = true;
        

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomsForm));
            this.panel_Rooms = new System.Windows.Forms.Panel();
            this.panel_Waiting = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_Pass = new System.Windows.Forms.Label();
            this.numRoomId = new System.Windows.Forms.NumericUpDown();
            this.btnJoin = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._ListViewRooms = new System.Windows.Forms.ListView();
            this.col_RState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Player1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Player2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Viewers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnView = new System.Windows.Forms.Button();
            this._btnNew = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.panel_Rooms.SuspendLayout();
            this.panel_Waiting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRoomId)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Rooms
            // 
            this.panel_Rooms.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Rooms.Controls.Add(this.panel_Waiting);
            this.panel_Rooms.Controls.Add(this.numRoomId);
            this.panel_Rooms.Controls.Add(this.btnJoin);
            this.panel_Rooms.Controls.Add(this.groupBox1);
            this.panel_Rooms.Controls.Add(this.btnView);
            this.panel_Rooms.Controls.Add(this._btnNew);
            this.panel_Rooms.Location = new System.Drawing.Point(1, 22);
            this.panel_Rooms.Name = "panel_Rooms";
            this.panel_Rooms.Size = new System.Drawing.Size(450, 261);
            this.panel_Rooms.TabIndex = 0;
            // 
            // panel_Waiting
            // 
            this.panel_Waiting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Waiting.Controls.Add(this.label3);
            this.panel_Waiting.Controls.Add(this.btnCancel);
            this.panel_Waiting.Controls.Add(this.label2);
            this.panel_Waiting.Controls.Add(this.lbl_Pass);
            this.panel_Waiting.Location = new System.Drawing.Point(9, 3);
            this.panel_Waiting.Name = "panel_Waiting";
            this.panel_Waiting.Size = new System.Drawing.Size(426, 209);
            this.panel_Waiting.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Waiting For Player 2...";
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(181, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(283, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Please Inform Player 2 With Your New Room`s Password.";
            // 
            // lbl_Pass
            // 
            this.lbl_Pass.AutoSize = true;
            this.lbl_Pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Pass.Location = new System.Drawing.Point(180, 70);
            this.lbl_Pass.Name = "lbl_Pass";
            this.lbl_Pass.Size = new System.Drawing.Size(77, 39);
            this.lbl_Pass.TabIndex = 4;
            this.lbl_Pass.Text = "153";
            // 
            // numRoomId
            // 
            this.numRoomId.Location = new System.Drawing.Point(15, 227);
            this.numRoomId.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numRoomId.Name = "numRoomId";
            this.numRoomId.Size = new System.Drawing.Size(100, 20);
            this.numRoomId.TabIndex = 11;
            // 
            // btnJoin
            // 
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Location = new System.Drawing.Point(121, 226);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(75, 23);
            this.btnJoin.TabIndex = 8;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._ListViewRooms);
            this.groupBox1.Location = new System.Drawing.Point(9, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 207);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Rooms";
            // 
            // _ListViewRooms
            // 
            this._ListViewRooms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_RState,
            this.col_Player1,
            this.col_Player2,
            this.col_Viewers});
            this._ListViewRooms.FullRowSelect = true;
            this._ListViewRooms.Location = new System.Drawing.Point(6, 19);
            this._ListViewRooms.MultiSelect = false;
            this._ListViewRooms.Name = "_ListViewRooms";
            this._ListViewRooms.Size = new System.Drawing.Size(414, 182);
            this._ListViewRooms.TabIndex = 0;
            this._ListViewRooms.UseCompatibleStateImageBehavior = false;
            this._ListViewRooms.View = System.Windows.Forms.View.Details;
            // 
            // col_RState
            // 
            this.col_RState.Text = "State";
            this.col_RState.Width = 84;
            // 
            // col_Player1
            // 
            this.col_Player1.Text = "Player 1";
            this.col_Player1.Width = 120;
            // 
            // col_Player2
            // 
            this.col_Player2.Text = "Player 2";
            this.col_Player2.Width = 120;
            // 
            // col_Viewers
            // 
            this.col_Viewers.Text = "Viewers";
            this.col_Viewers.Width = 80;
            // 
            // btnView
            // 
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Location = new System.Drawing.Point(273, 226);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "Watch Game";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // _btnNew
            // 
            this._btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnNew.Location = new System.Drawing.Point(354, 226);
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(75, 23);
            this._btnNew.TabIndex = 10;
            this._btnNew.Text = "New";
            this._btnNew.UseVisualStyleBackColor = true;
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // btnclose
            // 
            this.btnclose.BackColor = System.Drawing.Color.Red;
            this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnclose.Location = new System.Drawing.Point(437, 1);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(20, 20);
            this.btnclose.TabIndex = 14;
            this.btnclose.Text = "X";
            this.btnclose.UseVisualStyleBackColor = false;
            this.btnclose.Click += new System.EventHandler(this.button1_Click);
            // 
            // RoomsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(458, 285);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.panel_Rooms);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoomsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Rooms";
            this.Load += new System.EventHandler(this.RoomsForm_Load);
            this.panel_Rooms.ResumeLayout(false);
            this.panel_Waiting.ResumeLayout(false);
            this.panel_Waiting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRoomId)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

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
                string[] items = new string[] { message.GetRoomStatus().ToString(), message.Player1ID.ToString(), message.Player2ID.ToString(), message.SpectatorsNum.ToString()};
                ListViewItem item = new ListViewItem(items)
                {
                    Tag = message
                };
                _ListViewRooms.Items.Add(item);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            RoomUpdateMessage reqViewRoom = (RoomUpdateMessage)_ListViewRooms.SelectedItems[0].Tag;
            reqViewRoom.UpdateState = RoomUpdateState.newSpectatorReq;
            _GameClient.SendMessageToServer(reqViewRoom);
            this.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
