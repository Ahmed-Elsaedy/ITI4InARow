namespace ITI4InARow.Game.Client
{
    partial class Server_Rooms
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
            this._btnPlay = new System.Windows.Forms.Button();
            this._btnView = new System.Windows.Forms.Button();
            this._btnCreate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _btnPlay
            // 
            this._btnPlay.Location = new System.Drawing.Point(30, 208);
            this._btnPlay.Name = "_btnPlay";
            this._btnPlay.Size = new System.Drawing.Size(75, 23);
            this._btnPlay.TabIndex = 0;
            this._btnPlay.Text = "Play";
            this._btnPlay.UseVisualStyleBackColor = true;
            // 
            // _btnView
            // 
            this._btnView.Location = new System.Drawing.Point(111, 208);
            this._btnView.Name = "_btnView";
            this._btnView.Size = new System.Drawing.Size(75, 23);
            this._btnView.TabIndex = 1;
            this._btnView.Text = "View";
            this._btnView.UseVisualStyleBackColor = true;
            // 
            // _btnCreate
            // 
            this._btnCreate.Location = new System.Drawing.Point(192, 208);
            this._btnCreate.Name = "_btnCreate";
            this._btnCreate.Size = new System.Drawing.Size(75, 23);
            this._btnCreate.TabIndex = 2;
            this._btnCreate.Text = "Create";
            this._btnCreate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Connected Clients";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 42);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(257, 160);
            this.listBox1.TabIndex = 4;
            // 
            // Server_Rooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 240);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._btnCreate);
            this.Controls.Add(this._btnView);
            this.Controls.Add(this._btnPlay);
            this.Name = "Server_Rooms";
            this.Text = "Server_Rooms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnPlay;
        private System.Windows.Forms.Button _btnView;
        private System.Windows.Forms.Button _btnCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
    }
}