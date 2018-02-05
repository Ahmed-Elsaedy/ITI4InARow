namespace ITI4InARow.Game.Client
{
    partial class ClientProfile
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
            this._lblName = new System.Windows.Forms.Label();
            this._lblAge = new System.Windows.Forms.Label();
            this._tbName = new System.Windows.Forms.TextBox();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._numAge = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this._numAge)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblName
            // 
            this._lblName.AutoSize = true;
            this._lblName.Location = new System.Drawing.Point(21, 16);
            this._lblName.Name = "_lblName";
            this._lblName.Size = new System.Drawing.Size(38, 13);
            this._lblName.TabIndex = 0;
            this._lblName.Text = "Name:";
            // 
            // _lblAge
            // 
            this._lblAge.AutoSize = true;
            this._lblAge.Location = new System.Drawing.Point(21, 41);
            this._lblAge.Name = "_lblAge";
            this._lblAge.Size = new System.Drawing.Size(29, 13);
            this._lblAge.TabIndex = 1;
            this._lblAge.Text = "Age:";
            // 
            // _tbName
            // 
            this._tbName.Location = new System.Drawing.Point(76, 13);
            this._tbName.Name = "_tbName";
            this._tbName.Size = new System.Drawing.Size(173, 20);
            this._tbName.TabIndex = 2;
            // 
            // _btnOk
            // 
            this._btnOk.Location = new System.Drawing.Point(116, 73);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 4;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(197, 73);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 5;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _numAge
            // 
            this._numAge.Location = new System.Drawing.Point(76, 39);
            this._numAge.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this._numAge.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numAge.Name = "_numAge";
            this._numAge.Size = new System.Drawing.Size(173, 20);
            this._numAge.TabIndex = 6;
            this._numAge.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // ClientProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 105);
            this.Controls.Add(this._numAge);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._tbName);
            this.Controls.Add(this._lblAge);
            this.Controls.Add(this._lblName);
            this.Name = "ClientProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Client Profile";
            ((System.ComponentModel.ISupportInitialize)(this._numAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblName;
        private System.Windows.Forms.Label _lblAge;
        private System.Windows.Forms.TextBox _tbName;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.NumericUpDown _numAge;
    }
}