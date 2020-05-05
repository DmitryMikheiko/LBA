using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SOE_IDE
{
    public partial class PictureBoxToolTip : UserControl
    {
        public PictureBoxToolTip()
        {
            InitializeComponent();
        }
        public void SetPosition(int x,int y)
        {
            label_Position.Text = String.Format("({0,2},{1,2})", x, y);
        }
        public void SetPosition(Point p)
        {
            SetPosition(p.X, p.Y);
        }
    }
}
