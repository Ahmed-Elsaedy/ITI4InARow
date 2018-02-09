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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._numIP4 = new System.Windows.Forms.NumericUpDown();
            this._numIP2 = new System.Windows.Forms.NumericUpDown();
            this._numIP3 = new System.Windows.Forms.NumericUpDown();
            this._numIP1 = new System.Windows.Forms.NumericUpDown();
            this._numPort = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_NickName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._numIP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numIP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "NickName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Server Port";
            // 
            // _numIP4
            // 
            this._numIP4.Location = new System.Drawing.Point(206, 37);
            this._numIP4.Name = "_numIP4";
            this._numIP4.Size = new System.Drawing.Size(40, 20);
            this._numIP4.TabIndex = 4;
            this._numIP4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _numIP2
            // 
            this._numIP2.Location = new System.Drawing.Point(116, 37);
            this._numIP2.Name = "_numIP2";
            this._numIP2.Size = new System.Drawing.Size(40, 20);
            this._numIP2.TabIndex = 2;
            // 
            // _numIP3
            // 
            this._numIP3.Location = new System.Drawing.Point(161, 37);
            this._numIP3.Name = "_numIP3";
            this._numIP3.Size = new System.Drawing.Size(40, 20);
            this._numIP3.TabIndex = 3;
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
            127,
            0,
            0,
            0});
            // 
            // _numPort
            // 
            this._numPort.Location = new System.Drawing.Point(71, 64);
            this._numPort.Maximum = new decimal(new int[] {
            50000,
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
            5031,
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
            this.groupBox2.Controls.Add(this.txt_NickName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 63);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player Data";
            // 
            // txt_NickName
            // 
            this.txt_NickName.Location = new System.Drawing.Point(64, 25);
            this.txt_NickName.Name = "txt_NickName";
            this.txt_NickName.Size = new System.Drawing.Size(175, 20);
            this.txt_NickName.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(171, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 203);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._numPort);
            this.Controls.Add(this._numIP1);
            this.Controls.Add(this._numIP3);
            this.Controls.Add(this._numIP2);
            this.Controls.Add(this._numIP4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect To Game Server";
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

        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.Button button1;
    }
}