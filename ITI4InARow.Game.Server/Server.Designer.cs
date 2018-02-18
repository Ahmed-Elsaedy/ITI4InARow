namespace ITI4InARow.Game.Server
{
    partial class Server
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this._lblListBox = new System.Windows.Forms.Label();
            this._lblOutput = new System.Windows.Forms.Label();
            this._MainMenu = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemStart = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemStop = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this._lbxConnectClients = new System.Windows.Forms.ListBox();
            this._lbxOutput = new System.Windows.Forms.ListBox();
            this.lblIP = new System.Windows.Forms.Label();
            this._MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblListBox
            // 
            this._lblListBox.AutoSize = true;
            this._lblListBox.Location = new System.Drawing.Point(12, 35);
            this._lblListBox.Name = "_lblListBox";
            this._lblListBox.Size = new System.Drawing.Size(93, 13);
            this._lblListBox.TabIndex = 4;
            this._lblListBox.Text = "Connected Clients";
            // 
            // _lblOutput
            // 
            this._lblOutput.AutoSize = true;
            this._lblOutput.Location = new System.Drawing.Point(12, 169);
            this._lblOutput.Name = "_lblOutput";
            this._lblOutput.Size = new System.Drawing.Size(73, 13);
            this._lblOutput.TabIndex = 6;
            this._lblOutput.Text = "Server Output";
            // 
            // _MainMenu
            // 
            this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.gameToolStripMenuItem,
            this.outputToolStripMenuItem});
            this._MainMenu.Location = new System.Drawing.Point(0, 0);
            this._MainMenu.Name = "_MainMenu";
            this._MainMenu.Size = new System.Drawing.Size(413, 24);
            this._MainMenu.TabIndex = 7;
            this._MainMenu.Text = "menuStrip1";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemStart,
            this._MenuItemStop});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // _MenuItemStart
            // 
            this._MenuItemStart.Name = "_MenuItemStart";
            this._MenuItemStart.Size = new System.Drawing.Size(98, 22);
            this._MenuItemStart.Text = "Start";
            this._MenuItemStart.Click += new System.EventHandler(this._MenuItemStart_Click);
            // 
            // _MenuItemStop
            // 
            this._MenuItemStop.Enabled = false;
            this._MenuItemStop.Name = "_MenuItemStop";
            this._MenuItemStop.Size = new System.Drawing.Size(98, 22);
            this._MenuItemStop.Text = "Stop";
            this._MenuItemStop.Click += new System.EventHandler(this._MenuItemStop_Click);
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemSettings});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // _MenuItemSettings
            // 
            this._MenuItemSettings.Name = "_MenuItemSettings";
            this._MenuItemSettings.Size = new System.Drawing.Size(125, 22);
            this._MenuItemSettings.Text = "Settings...";
            this._MenuItemSettings.Click += new System.EventHandler(this._MenuItemSettings_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuItemClear});
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.outputToolStripMenuItem.Text = "Output";
            // 
            // _MenuItemClear
            // 
            this._MenuItemClear.Name = "_MenuItemClear";
            this._MenuItemClear.Size = new System.Drawing.Size(101, 22);
            this._MenuItemClear.Text = "Clear";
            this._MenuItemClear.Click += new System.EventHandler(this._MenuItemClear_Click);
            // 
            // _lbxConnectClients
            // 
            this._lbxConnectClients.Enabled = false;
            this._lbxConnectClients.FormattingEnabled = true;
            this._lbxConnectClients.Location = new System.Drawing.Point(12, 52);
            this._lbxConnectClients.Name = "_lbxConnectClients";
            this._lbxConnectClients.Size = new System.Drawing.Size(389, 108);
            this._lbxConnectClients.TabIndex = 8;
            // 
            // _lbxOutput
            // 
            this._lbxOutput.FormattingEnabled = true;
            this._lbxOutput.Location = new System.Drawing.Point(12, 186);
            this._lbxOutput.Name = "_lbxOutput";
            this._lbxOutput.Size = new System.Drawing.Size(389, 186);
            this._lbxOutput.TabIndex = 9;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(320, 35);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(31, 13);
            this.lblIP.TabIndex = 10;
            this.lblIP.Text = "MyIP";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 388);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this._lbxOutput);
            this.Controls.Add(this._lbxConnectClients);
            this.Controls.Add(this._lblOutput);
            this.Controls.Add(this._lblListBox);
            this.Controls.Add(this._MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._MainMenu;
            this.Name = "Server";
            this.Text = "Server";
            this._MainMenu.ResumeLayout(false);
            this._MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _lblListBox;
        private System.Windows.Forms.Label _lblOutput;
        private System.Windows.Forms.MenuStrip _MainMenu;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemStart;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemStop;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemSettings;
        private System.Windows.Forms.ListBox _lbxConnectClients;
        private System.Windows.Forms.ListBox _lbxOutput;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _MenuItemClear;
        private System.Windows.Forms.Label lblIP;
    }
}