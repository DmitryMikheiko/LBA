using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;


namespace LED_Board_Access
{
    class LB_Tools_TimeSecProgressBar : LB_Tools_Interface
    {
        private Bitmap bitmap;
        private Point PBPosition;
        private Size  _size;
        private Color PBColor;
        private Color PBBackColor;
        private ColorEffect PBColorEffect;

        public int PbBackColorARGB;
        public int PbColorARGB;

        byte WheelPos = 0;
        public enum ColorEffect
        {
            None,
            RainbowCycle
        }
        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_TimeSecProgressBar_Name
        {
            get { return "Tools_TimeSecProgressBar"; }
        }
        [Category("Layout")]
        [DisplayName("Location")]
        public Point position
        {
            get { return PBPosition; }
            set { PBPosition = value; }
        }
       
        [Category("Layout")]
        [DisplayName("Width")]
        public Size size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        [Category("Appearance")]
        [DisplayName("Color")]
        public Color color
        {
            get { return PBColor; }
            set { PBColor = value; PbColorARGB = value.ToArgb(); }
        }
        [Category("Appearance")]
        [DisplayName("ColorEffect")]
        public ColorEffect colorEffect
        {
            get { return PBColorEffect; }
            set { PBColorEffect = value; }
        }
        [Category("Appearance")]
        [DisplayName("BackColor")]
        public Color backColor
        {
            get { return PBBackColor; }
            set { PBBackColor = value; PbBackColorARGB = value.ToArgb(); }
        }

        public string LbResourcePath = "";
        public string LbResourceName = "";

        public LB_Tools_TimeSecProgressBar()
        {
            color = Color.Red;
            backColor = Color.Black;
            _size.Width = 24;
            _size.Height = 1;
            PbColorARGB = color.ToArgb();
            PbBackColorARGB = backColor.ToArgb();
        }
        private Color RainbowCycleGetColor(byte WheelPos)
        {
            byte[] c = new byte[3];
          if(WheelPos < 85) 
          {
             c[0]=(byte)(WheelPos * 3);
             c[1] = (byte)(255 - WheelPos * 3);
             c[2]=0;
          } else if(WheelPos < 170) 
          {
             WheelPos -= 85;
             c[0] = (byte)(255 - WheelPos * 3);
             c[1]=0;
             c[2] = (byte)(WheelPos * 3);
          } else 
          {
            WheelPos -= 170;
            c[0]=0;
            c[1] = (byte)(WheelPos * 3);
            c[2] = (byte)(255 - WheelPos * 3);
           }
          return Color.FromArgb(c[0], c[1], c[2]);
        }
        public Bitmap GetBitmap()
        {      
            if (bitmap == null)
            {
                bitmap = new Bitmap(size.Width, 1);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(backColor);
            }
            else
            {
                Color lineColor;
                switch(colorEffect)
                {
                    case ColorEffect.None:
                        lineColor = color;
                        break;
                    case ColorEffect.RainbowCycle:
                        lineColor = RainbowCycleGetColor(WheelPos++);
                        break;
                    default:
                        lineColor = color;
                        break;
                }

                bitmap = new Bitmap(size.Width, 1);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(backColor);
                int TimeSec = DateTime.Now.Second;
                int part = (int)((double)TimeSec / 59.0 * size.Width);
                for (int x = 0; x < part; x++) bitmap.SetPixel(x, 0, lineColor);
            }
            return bitmap;
        }
        public Point GetPosition()
        {
            return position;
        }
        public void SetPosition(Point p)
        {
            position = p;
        }
        public void MovePosition(Point dp)
        {
            PBPosition.X += dp.X;
            PBPosition.Y += dp.Y;
        }
        public Rectangle GetArea()
        {
            Rectangle rect = new Rectangle(position, new Size(size.Width, 1));
            return rect;
        }
        public Size GetSize()
        {
            return new Size(size.Width, 1);
        }
        public string GetToolName()
        {
            return LB_Tools_TimeSecProgressBar_Name;
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
