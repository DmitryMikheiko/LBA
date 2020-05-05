using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace LED_Board_Access
{
    class LB_Tools_Clock : LB_Tools_Interface
    {
        StringToBitmapConverter stbc;

        private string Text;
        private Font TextFont;
        private Color FontColor;
        private Color BackColor;
        private Point ClockPosition;
        private Size  ClockSize;
        private ColorEffect CColorEffect;

        public int FontColorARGB;
        public int BackColorARGB;

        public enum ColorEffect
        {
            None,
            RainbowCycle
        }

        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_Clock_Name
        {
            get { return "Tools_Clock"; }
        }
        [Category("Appearance")]
        [DisplayName("FontColor")]
        public Color fontColor
        {
            get { return FontColor; }
            set { FontColor = value; FontColorARGB = value.ToArgb(); }
        }
        [Category("Appearance")]
        [DisplayName("ColorEffect")]
        public ColorEffect colorEffect
        {
            get { return CColorEffect; }
            set { CColorEffect = value; }
        }
        [Category("Appearance")]
        [DisplayName("BackColor")]
        public Color backColor
        {
            get { return BackColor; }
            set { BackColor = value; BackColorARGB = value.ToArgb(); }
        }

      
        [Category("Layout")]
        [DisplayName("Location")]
        public Point position
        {
            get { return ClockPosition; }
            set { ClockPosition = value; }
        }
        [Category("Layout")]
        [DisplayName("Size")]
        [ReadOnly(true)]
        public Size size
        {
            get { return ClockSize; }
            set { ClockSize = value; }
        }
        public string LbResourcePath = "";
        public string LbResourceName = "";
        public LB_Tools_Clock()
        {
            TextFont = new Font("Tahoma", 7, FontStyle.Regular);
            Text = "clock";
            FontColor= Color.Red;
            BackColor = Color.Black;
            ClockSize = new Size(24, 8);
            stbc = new StringToBitmapConverter();
            stbc._InterpolationMode = InterpolationMode.Default;
            stbc._SmoothingMode = SmoothingMode.None;
            stbc._TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            FontColorARGB = FontColor.ToArgb();
            BackColorARGB = BackColor.ToArgb();
        }
        public Size GetSize()
        {
            return GetBitmap().Size;
        }
        public Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(ClockSize.Width, ClockSize.Height);
            Bitmap str_bitmap;
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(BackColor);

            Text = DateTime.Now.ToString("hh:mm tt");// Text = DateTime.Now.ToString("mm:ss tt");
            str_bitmap = stbc.GetBitmap(Text, TextFont, new SolidBrush(fontColor), backColor);
            g.DrawImage(str_bitmap, 1, 0);
            
            return bitmap;
        }
        public Point GetPosition()
        {
            return ClockPosition;
        }
        public void SetPosition(Point p)
        {
            ClockPosition = p;
        }
        public void MovePosition(Point dp)
        {
            ClockPosition.Offset(dp);
        }
        public Rectangle GetArea()
        {
            Rectangle rect = new Rectangle(position, size);
            return rect;
        }
        public string GetToolName()
        {
            return LB_Tools_Clock_Name;
        }
        public string GetResourcePath()
        {
            return "";
        }
        public string GetLBResourcePath()
        {
            return LbResourcePath;
        }
        public string GetLBResourceName()
        {
            return LbResourceName;
        }
    }
}
