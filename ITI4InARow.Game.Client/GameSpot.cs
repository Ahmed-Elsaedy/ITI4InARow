using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITI4InARow.Game.Client2
{
    public partial class GameSpot : UserControl
    {
        public GameSpot()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillEllipse(new SolidBrush(SpotSelectedColor),
                new RectangleF(new PointF(4, 4), new SizeF(Width - 8, Height - 8)));
            e.Graphics.DrawEllipse(new Pen(ForeColor, 4),
                new RectangleF(new PointF(4, 4), new SizeF(Width - 8, Height - 8)));
        }

        public Color SpotSelectedColor { get; set; }
    }
}
