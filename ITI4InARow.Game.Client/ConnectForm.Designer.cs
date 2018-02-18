namespace ITI4InARow.Game.Client
{
    partial class ConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectForm));
            this.lblusername = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._numIP4 = new System.Windows.Forms.NumericUpDown();
            this._numIP2 = new System.Windows.Forms.NumericUpDown();
            this._numIP3 = new System.Windows.Forms.NumericUpDown();
            this._numIP1 = new System.Windows.Forms.NumericUpDown();
            this._numPort = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnColore = new System.Windows.Forms.Button();
            this.txt_NickName = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.ClinetColoreDialog = new System.Windows.Forms.ColorDialog();
            this.lbluserColorChoice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._numIP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblusername
            // 
            this.lblusername.AutoSize = true;
            this.lblusername.Location = new System.Drawing.Point(5, 28);
            this.lblusername.Name = "lblusername";
            this.lblusername.Size = new System.Drawing.Size(63, 13);
            this.lblusername.TabIndex = 1;
            this.lblusername.Text = "User Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Server Port";
            // 
            // _numIP4
            // 
            this._numIP4.Location = new System.Drawing.Point(206, 37);
            this._numIP4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this._numIP4.Name = "_numIP4";
            this._numIP4.Size = new System.Drawing.Size(40, 20);
            this._numIP4.TabIndex = 4;
            this._numIP4.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // _numIP2
            // 
            this._numIP2.Location = new System.Drawing.Point(116, 37);
            this._numIP2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this._numIP2.Name = "_numIP2";
            this._numIP2.Size = new System.Drawing.Size(40, 20);
            this._numIP2.TabIndex = 2;
            this._numIP2.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // _numIP3
            // 
            this._numIP3.Location = new System.Drawing.Point(161, 37);
            this._numIP3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this._numIP3.Name = "_numIP3";
            this._numIP3.Size = new System.Drawing.Size(40, 20);
            this._numIP3.TabIndex = 3;
            this._numIP3.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // _numIP1
            // 
            this._numIP1.Location = new System.Drawing.Point(71, 37);
            this._numIP1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this._numIP1.Name = "_numIP1";
            this._numIP1.Size = new System.Drawing.Size(40, 20);
            this._numIP1.TabIndex = 1;
            this._numIP1.Value = new decimal(new int[] {
            172,
            0,
            0,
            0});
            // 
            // _numPort
            // 
            this._numPort.Location = new System.Drawing.Point(71, 64);
            this._numPort.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this._numPort.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this._numPort.Name = "_numPort";
            this._numPort.Size = new System.Drawing.Size(175, 20);
            this._numPort.TabIndex = 5;
            this._numPort.Value = new decimal(new int[] {
            5252,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 86);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbluserColorChoice);
            this.groupBox2.Controls.Add(this.btnColore);
            this.groupBox2.Controls.Add(this.txt_NickName);
            this.groupBox2.Controls.Add(this.lblusername);
            this.groupBox2.Location = new System.Drawing.Point(7, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 63);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player Data";
            // 
            // btnColore
            // 
            this.btnColore.BackColor = System.Drawing.Color.Black;
            this.btnColore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnColore.FlatAppearance.BorderSize = 0;
            this.btnColore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColore.Location = new System.Drawing.Point(218, 23);
            this.btnColore.Name = "btnColore";
            this.btnColore.Size = new System.Drawing.Size(25, 25);
            this.btnColore.TabIndex = 7;
            this.btnColore.UseVisualStyleBackColor = false;
            this.btnColore.Click += new System.EventHandler(this.btnColore_Click);
            // 
            // txt_NickName
            // 
            this.txt_NickName.Location = new System.Drawing.Point(64, 25);
            this.txt_NickName.Name = "txt_NickName";
            this.txt_NickName.Size = new System.Drawing.Size(149, 20);
            this.txt_NickName.TabIndex = 6;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(171, 173);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnexct_Click);
            // 
            // lbluserColorChoice
            // 
            this.lbluserColorChoice.AutoSize = true;
            this.lbluserColorChoice.Location = new System.Drawing.Point(182, 7);
            this.lbluserColorChoice.Name = "lbluserColorChoice";
            this.lbluserColorChoice.Size = new System.Drawing.Size(71, 13);
            this.lbluserColorChoice.TabIndex = 8;
            this.lbluserColorChoice.Text = "Choose Color";
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 205);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this._numPort);
            this.Controls.Add(this._numIP1);
            this.Controls.Add(this._numIP3);
            this.Controls.Add(this._numIP2);
            this.Controls.Add(this._numIP4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect To Game Server";
            this.Load += new System.EventHandler(this.ConnectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._numIP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblusername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown _numIP4;
        private System.Windows.Forms.NumericUpDown _numIP2;
        private System.Windows.Forms.NumericUpDown _numIP3;
        private System.Windows.Forms.NumericUpDown _numIP1;
        private System.Windows.Forms.NumericUpDown _numPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_NickName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnColore;
        private System.Windows.Forms.ColorDialog ClinetColoreDialog;
        private System.Windows.Forms.Label lbluserColorChoice;
    }
}