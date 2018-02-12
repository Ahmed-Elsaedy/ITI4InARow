using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITI4InARow.Game.Client
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._MainMenu = new MenuStrip();
            this.menuItem_Server = new ToolStripMenuItem();
            this._MenuItemConnect = new ToolStripMenuItem();
            this._MenuItemDisconnect = new ToolStripMenuItem();
            this.menuItem_Game = new ToolStripMenuItem();
            this._MenuItemSRooms = new ToolStripMenuItem();
            this._StatusStrip = new StatusStrip();
            this._stl_Connection = new ToolStripStatusLabel();
            this.panel_GameSurface = new Panel();
            this.btn_LeaveGame = new Button();
            this.btn_GameMove = new Button();
            this.lbl_Title = new Label();
            this._MainMenu.SuspendLayout();
            this._StatusStrip.SuspendLayout();
            this.panel_GameSurface.SuspendLayout();
            base.SuspendLayout();
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.menuItem_Server, this.menuItem_Game };
            this._MainMenu.Items.AddRange(toolStripItems);
            this._MainMenu.Location = new Point(0, 0);
            this._MainMenu.Name = "_MainMenu";
            this._MainMenu.Size = new Size(0x210, 0x18);
            this._MainMenu.TabIndex = 0;
            this._MainMenu.Text = "menuStrip1";
            ToolStripItem[] itemArray2 = new ToolStripItem[] { this._MenuItemConnect, this._MenuItemDisconnect };
            this.menuItem_Server.DropDownItems.AddRange(itemArray2);
            this.menuItem_Server.Name = "menuItem_Server";
            this.menuItem_Server.Size = new Size(0x33, 20);
            this.menuItem_Server.Text = "Server";
            this._MenuItemConnect.Name = "_MenuItemConnect";
            this._MenuItemConnect.Size = new Size(0x85, 0x16);
            this._MenuItemConnect.Text = "Connect...";
            this._MenuItemConnect.Click += new EventHandler(this._MenuItemConnect_Click);
            this._MenuItemDisconnect.Enabled = false;
            this._MenuItemDisconnect.Name = "_MenuItemDisconnect";
            this._MenuItemDisconnect.Size = new Size(0x85, 0x16);
            this._MenuItemDisconnect.Text = "Disconnect";
            this._MenuItemDisconnect.Click += new EventHandler(this._MenuItemDisconnect_Click);
            ToolStripItem[] itemArray3 = new ToolStripItem[] { this._MenuItemSRooms };
            this.menuItem_Game.DropDownItems.AddRange(itemArray3);
            this.menuItem_Game.Name = "menuItem_Game";
            this.menuItem_Game.Size = new Size(50, 20);
            this.menuItem_Game.Text = "Game";
            this._MenuItemSRooms.Name = "_MenuItemSRooms";
            this._MenuItemSRooms.Size = new Size(0x9b, 0x16);
            this._MenuItemSRooms.Text = "Server Rooms...";
            this._MenuItemSRooms.Click += new EventHandler(this._MenuItemSRooms_Click);
            ToolStripItem[] itemArray4 = new ToolStripItem[] { this._stl_Connection };
            this._StatusStrip.Items.AddRange(itemArray4);
            this._StatusStrip.Location = new Point(0, 0x176);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new Size(0x210, 0x16);
            this._StatusStrip.TabIndex = 1;
            this._StatusStrip.Text = "statusStrip1";
            this._stl_Connection.Name = "_stl_Connection";
            this._stl_Connection.Size = new Size(0, 0x11);
            this.panel_GameSurface.BorderStyle = BorderStyle.FixedSingle;
            this.panel_GameSurface.Controls.Add(this.btn_LeaveGame);
            this.panel_GameSurface.Controls.Add(this.btn_GameMove);
            this.panel_GameSurface.Controls.Add(this.lbl_Title);
            this.panel_GameSurface.Location = new Point(0x9f, 0x86);
            this.panel_GameSurface.Name = "panel_GameSurface";
            this.panel_GameSurface.Size = new Size(0xd8, 0x90);
            this.panel_GameSurface.TabIndex = 2;
            this.btn_LeaveGame.Location = new Point(0x3a, 0x56);
            this.btn_LeaveGame.Name = "btn_LeaveGame";
            this.btn_LeaveGame.Size = new Size(0x6d, 0x17);
            this.btn_LeaveGame.TabIndex = 2;
            this.btn_LeaveGame.Text = "Leave Game";
            this.btn_LeaveGame.UseVisualStyleBackColor = true;
            this.btn_LeaveGame.Click += new EventHandler(this.btn_LeaveGame_Click);
            this.btn_GameMove.Location = new Point(0x2b, 0x3a);
            this.btn_GameMove.Name = "btn_GameMove";
            this.btn_GameMove.Size = new Size(0x8a, 0x17);
            this.btn_GameMove.TabIndex = 1;
            this.btn_GameMove.Text = "Simulate Game Move";
            this.btn_GameMove.UseVisualStyleBackColor = true;
            this.btn_GameMove.Click += new EventHandler(this.btn_GameMove_Click);
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbl_Title.Location = new Point(0x2f, 0x1d);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new Size(130, 0x18);
            this.lbl_Title.TabIndex = 0;
            this.lbl_Title.Text = "Gaming Mode";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x210, 0x18c);
            base.Controls.Add(this.panel_GameSurface);
            base.Controls.Add(this._StatusStrip);
            base.Controls.Add(this._MainMenu);
            base.Name = "Client";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Client";
            base.FormClosing += new FormClosingEventHandler(this.Client_FormClosing);
            base.Load += new System.EventHandler(this.Client_Load);
            this._MainMenu.ResumeLayout(false);
            this._MainMenu.PerformLayout();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this.panel_GameSurface.ResumeLayout(false);
            this.panel_GameSurface.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip _MainMenu;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemConnect;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemDisconnect;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemSRooms;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _stl_Connection;
        private System.Windows.Forms.Button btn_GameMove;
        private System.Windows.Forms.Button btn_LeaveGame;
        private Label lbl_Title;
        private ToolStripMenuItem menuItem_Game;
        private ToolStripMenuItem menuItem_Server;
        private Panel panel_GameSurface;
    }
}