using System;
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
    public partial class ClientProfile : Form
    {
        public string ProfileName => _tbName.Text;
        public int ProfileAge => (int)_numAge.Value;
        public ClientProfile()
        {
            InitializeComponent();
        }
        private void _btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_tbName.Text))
                DialogResult = DialogResult.OK;
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
