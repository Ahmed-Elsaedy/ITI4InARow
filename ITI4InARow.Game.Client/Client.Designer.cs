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
            this._MainMenu = new System.Windows.Forms.MenuStrip();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemConnect = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this._stl_Connection = new System.Windows.Forms.ToolStripStatusLabel();
            this._MainMenu.SuspendLayout();
            this._StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _MainMenu
            // 
            this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientToolStripMenuItem,
            this.gameToolStripMenuItem});
            this._MainMenu.Location = new System.Drawing.Point(0, 0);
            this._MainMenu.Name = "_MainMenu";
            this._MainMenu.Size = new System.Drawing.Size(284, 24);
            this._MainMenu.TabIndex = 0;
            this._MainMenu.Text = "menuStrip1";
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemConnect,
            this._MenuItemDisconnect});
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.clientToolStripMenuItem.Text = "Server";
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
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stl_Connection});
            this._StatusStrip.Location = new System.Drawing.Point(0, 239);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(284, 22);
            this._StatusStrip.TabIndex = 1;
            this._StatusStrip.Text = "statusStrip1";
            // 
            // _stl_Connection
            // 
            this._stl_Connection.Name = "_stl_Connection";
            this._stl_Connection.Size = new System.Drawing.Size(0, 17);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this._MainMenu);
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this._MainMenu.ResumeLayout(false);
            this._MainMenu.PerformLayout();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _MainMenu;
        private System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemConnect;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemDisconnect;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _stl_Connection;
    }
}