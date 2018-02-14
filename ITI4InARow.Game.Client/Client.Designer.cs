using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this._MainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItem_Server = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemConnect = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Game = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemSRooms = new System.Windows.Forms.ToolStripMenuItem();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this._stl_Connection = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel_GameSurface = new ITI4InARow.Game.UI.GameUI();
            this.btn_LeaveGame = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.printForm1 = new Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(this.components);
            this._MainMenu.SuspendLayout();
            this._StatusStrip.SuspendLayout();
            this.panel_GameSurface.SuspendLayout();
            this.SuspendLayout();
            // 
            // _MainMenu
            // 
            this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Server,
            this.menuItem_Game});
            this._MainMenu.Location = new System.Drawing.Point(0, 0);
            this._MainMenu.Name = "_MainMenu";
            this._MainMenu.Size = new System.Drawing.Size(1003, 24);
            this._MainMenu.TabIndex = 0;
            this._MainMenu.Text = "menuStrip1";
            // 
            // menuItem_Server
            // 
            this.menuItem_Server.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemConnect,
            this._MenuItemDisconnect});
            this.menuItem_Server.Name = "menuItem_Server";
            this.menuItem_Server.Size = new System.Drawing.Size(51, 20);
            this.menuItem_Server.Text = "Server";
            // 
            // _MenuItemConnect
            // 
            this._MenuItemConnect.Name = "_MenuItemConnect";
            this._MenuItemConnect.Size = new System.Drawing.Size(133, 22);
            this._MenuItemConnect.Text = "Connect...";
            this._MenuItemConnect.Click += new System.EventHandler(this._MenuItemConnect_Click);
            // 
            // _MenuItemDisconnect
            // 
            this._MenuItemDisconnect.Enabled = false;
            this._MenuItemDisconnect.Name = "_MenuItemDisconnect";
            this._MenuItemDisconnect.Size = new System.Drawing.Size(133, 22);
            this._MenuItemDisconnect.Text = "Disconnect";
            this._MenuItemDisconnect.Click += new System.EventHandler(this._MenuItemDisconnect_Click);
            // 
            // menuItem_Game
            // 
            this.menuItem_Game.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemSRooms});
            this.menuItem_Game.Name = "menuItem_Game";
            this.menuItem_Game.Size = new System.Drawing.Size(50, 20);
            this.menuItem_Game.Text = "Game";
            // 
            // _MenuItemSRooms
            // 
            this._MenuItemSRooms.Name = "_MenuItemSRooms";
            this._MenuItemSRooms.Size = new System.Drawing.Size(155, 22);
            this._MenuItemSRooms.Text = "Server Rooms...";
            this._MenuItemSRooms.Click += new System.EventHandler(this._MenuItemSRooms_Click);
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stl_Connection});
            this._StatusStrip.Location = new System.Drawing.Point(0, 590);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(1003, 22);
            this._StatusStrip.TabIndex = 1;
            this._StatusStrip.Text = "statusStrip1";
            // 
            // _stl_Connection
            // 
            this._stl_Connection.Name = "_stl_Connection";
            this._stl_Connection.Size = new System.Drawing.Size(0, 17);
            // 
            // panel_GameSurface
            // 
            this.panel_GameSurface.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel_GameSurface.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_GameSurface.Controls.Add(this.btn_LeaveGame);
            this.panel_GameSurface.Location = new System.Drawing.Point(0, 27);
            this.panel_GameSurface.Name = "panel_GameSurface";
            this.panel_GameSurface.Size = new System.Drawing.Size(949, 628);
            this.panel_GameSurface.TabIndex = 2;
            // 
            // btn_LeaveGame
            // 
            this.btn_LeaveGame.Location = new System.Drawing.Point(839, 86);
            this.btn_LeaveGame.Name = "btn_LeaveGame";
            this.btn_LeaveGame.Size = new System.Drawing.Size(109, 23);
            this.btn_LeaveGame.TabIndex = 2;
            this.btn_LeaveGame.Text = "Leave Game";
            this.btn_LeaveGame.UseVisualStyleBackColor = true;
            this.btn_LeaveGame.Click += new System.EventHandler(this.btn_LeaveGame_Click);
            // 
            // printForm1
            // 
            this.printForm1.DocumentName = "document";
            this.printForm1.Form = this;
            this.printForm1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter;
            this.printForm1.PrinterSettings = ((System.Drawing.Printing.PrinterSettings)(resources.GetObject("printForm1.PrinterSettings")));
            this.printForm1.PrintFileName = null;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 612);
            this.Controls.Add(this.panel_GameSurface);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this._MainMenu);
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.Load += new System.EventHandler(this.Client_Load);
            this._MainMenu.ResumeLayout(false);
            this._MainMenu.PerformLayout();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this.panel_GameSurface.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _MainMenu;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemConnect;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemDisconnect;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemSRooms;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _stl_Connection;
        private System.Windows.Forms.Button btn_LeaveGame;
        private ToolStripMenuItem menuItem_Game;
        private ToolStripMenuItem menuItem_Server;
        private UI.GameUI panel_GameSurface;
        private ColorDialog colorDialog1;
        private Microsoft.VisualBasic.PowerPacks.Printing.PrintForm printForm1;
        
    }
}