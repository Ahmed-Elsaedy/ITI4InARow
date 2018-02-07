using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace ITI4InARow.Game.UI
{
    public partial class GameUI : Form
    {
        public GameUI()
        {
            InitializeComponent();
            //OvalShape ovalShape3 = new OvalShape(shapeContainer1);
            //ovalShape3.Size = new Size(90, 90);
            //ovalShape3.Location = new Point(150, 90);

            //ovalShape3.FillStyle = FillStyle.Solid;
            //ovalShape3.FillColor = Color.Azure;
            //shapeContainer1.ForeColor = Color.Green;
           
        }

     
        private void ovalShape2_Click(object sender, EventArgs e)
        {
            //ovalShape1.FillColor = Color.Aqua;

            //MessageBox.Show(ovalShape1.Tag.ToString());
            //if (shapeContainer1.Shapes.Contains((Shape)sender))
            //{
            //    MessageBox.Show(((Shape)sender).ToString());
            //}


        }

        private void ovalShape41_MouseClick(object sender, MouseEventArgs e)
        {
            //ovalShape41.FillColor = Color.Blue;
        }

        private void GameUI_Load(object sender, EventArgs e)
        {
            for(int i = 0;i< 42; i++)
            {
                ((OvalShape)shapeContainer1.Shapes.get_Item(i)).Tag = i + 1;
                ((OvalShape)shapeContainer1.Shapes.get_Item(i)).MouseClick += GameUI_MouseClick;
            }
        }

        private void GameUI_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(((OvalShape)sender).Tag.ToString());
        }
    }
}
