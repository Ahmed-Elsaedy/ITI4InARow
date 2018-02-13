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
            this._GameClient = gameClient;
            this.InitializeComponent();
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            RoomUpdateMessage message1 = new RoomUpdateMessage
            {
                UpdateState = RoomUpdateState.NewRoomRequest
            };
            this._MyRoomUpdate = message1;
            this._GameClient.SendMessageToServer(this._MyRoomUpdate);
            this._btnNew.Enabled = false;
        }

        private void _ListViewRooms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.UpdateButtonsStatus();
            }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._MyRoomUpdate.UpdateState = RoomUpdateState.NewRoomRollback;
            this._GameClient.SendMessageToServer(this._MyRoomUpdate);
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            RoomUpdateMessage tag = (RoomUpdateMessage)this._ListViewRooms.SelectedItems[0].Tag;
            if (((int)this.numRoomId.Value) == tag.RoomID)
            {
                tag.UpdateState = RoomUpdateState.Player2Connected;
                this._GameClient.SendMessageToServer(tag);
                UI.GameUI gameBord = new UI.GameUI();
                gameBord.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong Room Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GameClient_RoomUpdateMessage(object sender, RoomUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateState)
            {
                case RoomUpdateState.NewRoomRequest:
                    this._MyRoomUpdate = msg;
                    this._RoomsUpdates.Add(msg);
                    this.SwitchToWaitingMode(msg.RoomID.ToString());
                    break;

                case RoomUpdateState.NewRoomRollback:
                    msg.UpdateState = RoomUpdateState.NewRoomRollback;
                    this._RoomsUpdates.Remove(this._RoomsUpdates.Single<RoomUpdateMessage>(x => x.RoomID == msg.RoomID));
                    this._MyRoomUpdate = null;
                    this.SwitchToRoomsMode();
                    break;

                case RoomUpdateState.Player2Connected:
                    message = this._RoomsUpdates.Single<RoomUpdateMessage>(x => x.RoomID == msg.RoomID);
                    message.Player2ID = msg.Player2ID;
                    message.UpdateState = RoomUpdateState.RoomComplete;
                    this._GameClient.SendMessageToServer(message);
                    break;

                case RoomUpdateState.Broadcast:
                    message = this._RoomsUpdates.SingleOrDefault<RoomUpdateMessage>(x => x.RoomID == msg.RoomID);
                    if ((message == null) || (msg.Player1ID <= 0))
                    {
                        if ((message == null) && (msg.Player1ID > 0))
                        {
                            this._RoomsUpdates.Add(msg);
                        }
                        else if ((message != null) && (msg.Player1ID == 0))
                        {
                            this._RoomsUpdates.Remove(msg);
                        }
                        break;
                    }
                    message.Player1ID = msg.Player1ID;
                    message.Player2ID = msg.Player2ID;
                    break;
            }
            this.UpdateListViewItem();
            this.UpdateButtonsStatus();
        }

        private void InitializeComponent()
        {
            this.panel_Rooms = new Panel();
            this.numRoomId = new NumericUpDown();
            this._btnNew = new Button();
            this.btnView = new Button();
            this.btnJoin = new Button();
            this.groupBox1 = new GroupBox();
            this.panel_Waiting = new Panel();
            this.label3 = new Label();
            this.btnCancel = new Button();
            this.label2 = new Label();
            this.lbl_Pass = new Label();
            this._ListViewRooms = new ListView();
            this.col_RState = new ColumnHeader();
            this.col_Player1 = new ColumnHeader();
            this.col_Player2 = new ColumnHeader();
            this.col_Viewers = new ColumnHeader();
            this.panel_Rooms.SuspendLayout();
            this.numRoomId.BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel_Waiting.SuspendLayout();
            base.SuspendLayout();
            this.panel_Rooms.Controls.Add(this.panel_Waiting);
            this.panel_Rooms.Controls.Add(this.numRoomId);
            this.panel_Rooms.Controls.Add(this.btnJoin);
            this.panel_Rooms.Controls.Add(this.groupBox1);
            this.panel_Rooms.Controls.Add(this.btnView);
            this.panel_Rooms.Controls.Add(this._btnNew);
            this.panel_Rooms.Location = new Point(1, 1);
            this.panel_Rooms.Name = "panel_Rooms";
            this.panel_Rooms.Size = new Size(450, 0x105);
            this.panel_Rooms.TabIndex = 0;
            this.numRoomId.Location = new Point(15, 0xe3);
            int[] bits = new int[4];
            bits[0] = 0x270f;
            this.numRoomId.Maximum = new decimal(bits);
            this.numRoomId.Name = "numRoomId";
            this.numRoomId.Size = new Size(100, 20);
            this.numRoomId.TabIndex = 11;
            this._btnNew.Location = new Point(0x162, 0xe2);
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new Size(0x4b, 0x17);
            this._btnNew.TabIndex = 10;
            this._btnNew.Text = "New";
            this._btnNew.UseVisualStyleBackColor = true;
            this._btnNew.Click += new EventHandler(this._btnNew_Click);
            this.btnView.Location = new Point(0x111, 0xe2);
            this.btnView.Name = "btnView";
            this.btnView.Size = new Size(0x4b, 0x17);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnJoin.Location = new Point(0x79, 0xe2);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new Size(0x4b, 0x17);
            this.btnJoin.TabIndex = 8;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new EventHandler(this.btnJoin_Click);
            this.groupBox1.Controls.Add(this._ListViewRooms);
            this.groupBox1.Location = new Point(9, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1aa, 0xcf);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Rooms";
            this.panel_Waiting.Controls.Add(this.label3);
            this.panel_Waiting.Controls.Add(this.btnCancel);
            this.panel_Waiting.Controls.Add(this.label2);
            this.panel_Waiting.Controls.Add(this.lbl_Pass);
            this.panel_Waiting.Location = new Point(0, 0);
            this.panel_Waiting.Name = "panel_Waiting";
            this.panel_Waiting.Size = new Size(450, 0x102);
            this.panel_Waiting.TabIndex = 13;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xa3, 0x8e);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x6f, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Waiting For Player 2...";
            this.btnCancel.Location = new Point(0xb5, 0xa5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x4f, 0x77);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x116, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Please Inform Player 2 With Your New Room`s Password.";
            this.lbl_Pass.AutoSize = true;
            this.lbl_Pass.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Pass.Location = new Point(180, 70);
            this.lbl_Pass.Name = "lbl_Pass";
            this.lbl_Pass.Size = new Size(0x4d, 0x27);
            this.lbl_Pass.TabIndex = 4;
            this.lbl_Pass.Text = "153";
            ColumnHeader[] values = new ColumnHeader[] { this.col_RState, this.col_Player1, this.col_Player2, this.col_Viewers };
            this._ListViewRooms.Columns.AddRange(values);
            this._ListViewRooms.FullRowSelect = true;
            this._ListViewRooms.Location = new Point(6, 0x13);
            this._ListViewRooms.MultiSelect = false;
            this._ListViewRooms.Name = "_ListViewRooms";
            this._ListViewRooms.Size = new Size(0x19e, 0xb6);
            this._ListViewRooms.TabIndex = 0;
            this._ListViewRooms.UseCompatibleStateImageBehavior = false;
            this._ListViewRooms.View = View.Details;
            this.col_RState.Text = "State";
            this.col_RState.Width = 0x54;
            this.col_Player1.Text = "Player 1";
            this.col_Player1.Width = 120;
            this.col_Player2.Text = "Player 2";
            this.col_Player2.Width = 120;
            this.col_Viewers.Text = "Viewers";
            this.col_Viewers.Width = 80;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1c3, 0x105);
            base.Controls.Add(this.panel_Rooms);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "RoomsForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Server Rooms";
            base.Load += new EventHandler(this.RoomsForm_Load);
            this.panel_Rooms.ResumeLayout(false);
            this.numRoomId.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel_Waiting.ResumeLayout(false);
            this.panel_Waiting.PerformLayout();
            base.ResumeLayout(false);
        }

        private void RoomsForm_Load(object sender, EventArgs e)
        {
            this._RoomsUpdates = new List<RoomUpdateMessage>();
            this._GameClient.RoomUpdateMessage += new EventHandler<RoomUpdateMessage>(this.GameClient_RoomUpdateMessage);
            this._ListViewRooms.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this._ListViewRooms_ItemSelectionChanged);
            this.SwitchToRoomsMode();
        }

        public void SwitchToRoomsMode()
        {
            this.panel_Waiting.Hide();
        }

        public void SwitchToWaitingMode(string pass)
        {
            this.lbl_Pass.Text = pass;
            this.panel_Waiting.Show();
        }

        public void UnloadForm()
        {
            this._GameClient.RoomUpdateMessage -= new EventHandler<RoomUpdateMessage>(this.GameClient_RoomUpdateMessage);
            this._ListViewRooms.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(this._ListViewRooms_ItemSelectionChanged);
            base.Dispose();
        }

        private void UpdateButtonsStatus()
        {
            if (this._ListViewRooms.SelectedItems.Count > 0)
            {
                RoomUpdateMessage tag = (RoomUpdateMessage)this._ListViewRooms.SelectedItems[0].Tag;
                this.btnJoin.Enabled = this.numRoomId.Enabled = (tag.Player1ID != 0) && (tag.Player2ID == 0);
                this.btnView.Enabled = (tag.Player1ID != 0) && (tag.Player2ID > 0);
                this._btnNew.Enabled = false;
            }
            else
            {
                this.btnJoin.Enabled = this.numRoomId.Enabled = false;
                this.btnView.Enabled = false;
                this._btnNew.Enabled = this._MyRoomUpdate == null;
            }
        }

        private void UpdateListViewItem()
        {
            this._ListViewRooms.Items.Clear();
            foreach (RoomUpdateMessage message in this._RoomsUpdates)
            {
                string[] items = new string[] { message.GetRoomStatus().ToString(), message.Player1ID.ToString(), message.Player2ID.ToString(), "0" };
                ListViewItem item = new ListViewItem(items)
                {
                    Tag = message
                };
                this._ListViewRooms.Items.Add(item);
            }
        }
    }
}
