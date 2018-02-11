﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITI4InARow.Game.Client
{
    public partial class ConnectForm : Form
    {
        public byte[] IPAddress { get; private set; }
        public int Port { get; private set; }
        public string NickName { get; private set; }

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_NickName.Text))
            {
                int p1 = (int)_numIP1.Value;
                int p2 = (int)_numIP2.Value;
                int p3 = (int)_numIP3.Value;
                int p4 = (int)_numIP4.Value;

                IPAddress = new byte[4];
                IPAddress[0] = (byte)p1;
                IPAddress[1] = (byte)p2;
                IPAddress[2] = (byte)p3;
                IPAddress[3] = (byte)p4;

                Port = (int)_numPort.Value;
                NickName = txt_NickName.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}