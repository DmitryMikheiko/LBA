using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

using VisualEffectsConverter;

namespace LED_Board_Access
{
 public   class LB_Tools_RunningText : LB_Tools_Interface
    {
        
 StringToBitmapConverter stbc;

        private string TextName;
        private string Text;
        private Font TextFont;
        private Color FontColor;
        private Color BackColor;
        private TextRenderingHint _textRenderingHint;
        private Size TextSize;
        private Rectangle TextArea;
        private Bitmap bitmap;
        private Rectangle ActiveFrame;
        private double ActiveFrameX;
        private int FPS = 25;
        private int Speed = 10;
        private int SpaceWidth=10;
        

        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_RunningText_Name
        {
            get { return "Tools_RunningText"; }
        }
        [Category("Component")]
        [DisplayName("TextName")]
        public string Text_Name
        {
            get { return TextName; }
            set { TextName = value; }
        }
        [Category("Appearance")]
        [DisplayName("Text")] 
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        [Category("Appearance")]
        [DisplayName("Font")] 
        public Font font
        {
            get { return TextFont; }
            set { TextFont = value; }
        }

        [Category("Appearance")]
        [DisplayName("FontColor")]
        public Color fontColor
        {
            get { return FontColor; }
            set { FontColor = value; }
        }

        [Category("Appearance")]
        [DisplayName("BackColor")]
        public Color backColor
        {
            get { return BackColor; }
            set { BackColor = value; }
        }

        [Category("Rendering")]
        [DisplayName("TextRenderingHint")]
        public TextRenderingHint textRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

       
       // [Category("Layout")]
       // [DisplayName("Location")]      
        public Point position
        {
            get { return TextArea.Location; }
            set { TextArea.Location = value; }
        }
        [Category("Layout")]
        [DisplayName("Size")]
        [ReadOnly(true)]
        public Size size
        {
            get { return TextSize; }
            set { TextSize = value; }
        }
        [Category("Layout")]
        [DisplayName("TextArea")]
        public Rectangle textArea
        {
            get { return TextArea; }
            set { TextArea = value; }
        }
        [Category("Special")]
        [DisplayName("Speed")]
        public int speed
        {
            get { return Speed; }
            set { Speed = value; }
        }
        [Category("Special")]
        [DisplayName("SpaceWidth")]
        public int spaceWidth
        {
            get { return SpaceWidth; }
            set { SpaceWidth = value; }
        }
        public string LbResourcePath = "";
        public string LbResourceName = "";
        public LB_Tools_RunningText()
        {
            TextFont = new Font("Tahoma", 8, FontStyle.Regular);
            Text = "str";
            FontColor= Color.Red;
            BackColor = Color.Black;
            stbc = new StringToBitmapConverter();
            textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            TextArea.Size = new Size(24, 8);
            ActiveFrame.Location = new Point(0, 0);
            ActiveFrameX = 0.0;
            ActiveFrame.X=0;
            ActiveFrame.Y=0;
            ActiveFrame.Size = new Size(24, 8);
            bitmap = new Bitmap(1, 8);
            DateTime dt = DateTime.Now;
            Text_Name = String.Format("{0:d2}{1:d2}{2:d4}_{3:d2}{4:d2}{5:d2}", dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute, dt.Second);
        }
        public Size GetSize()
        {
            return GetBitmap().Size;
        }
        public Bitmap GetBitmap()
        {
            Graphics g;
            Bitmap textBitmap;
            stbc._InterpolationMode = InterpolationMode.Bicubic;
            stbc._SmoothingMode = SmoothingMode.None;
            stbc._TextRenderingHint = textRenderingHint;
            ActiveFrame.Width = TextArea.Width;
            ActiveFrame.Height = TextArea.Height;
            
            while(ActiveFrame.X + ActiveFrame.Width > bitmap.Width)
            {
                textBitmap = stbc.GetBitmap(text, font, new SolidBrush(fontColor), backColor);
                TextSize = textBitmap.Size;
                int NewWidth = bitmap.Width + SpaceWidth + textBitmap.Width - ActiveFrame.X ;
                Bitmap NewBitmap = new Bitmap(NewWidth, textBitmap.Height);
                g = Graphics.FromImage(NewBitmap);
                g.Clear(BackColor);
                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width - ActiveFrame.X, textBitmap.Height), new Rectangle(ActiveFrame.X, 0, bitmap.Width - ActiveFrame.X, textBitmap.Height), GraphicsUnit.Pixel);
                g.DrawImage(textBitmap, bitmap.Width - ActiveFrame.X + SpaceWidth, 0);
                bitmap = NewBitmap;
                ActiveFrameX -= ActiveFrame.X;
                ActiveFrame.X = 0;
            }

            Bitmap frame = new Bitmap(TextArea.Width, TextArea.Height);
            g = Graphics.FromImage(frame);
            g.Clear(BackColor);
            g.DrawImage(bitmap, new Rectangle(0, 0, frame.Width, frame.Height), ActiveFrame,GraphicsUnit.Pixel);
            ActiveFrameX += (float)Speed / (float)FPS;
            ActiveFrame.X = (int)ActiveFrameX;

            return frame;
        }
        public bool SaveAsBMA(string path)
        {
            if (path == "") return false;
            bitmap = new Bitmap(1, 8);
            GetBitmap();
            int frames = ActiveFrame.Width;
            ActiveFrameX = 0.0;
            ActiveFrame.X = 0;
            int x = 0;

            AnimationConverter animationConverter = new AnimationConverter(TextArea.Width, TextArea.Height);
            while (frames > 0)
            {
                GetBitmap();
                if (x != ActiveFrame.X)
                {
                    x = ActiveFrame.X;
                    frames--;
                }
            }
            frames = SpaceWidth + TextSize.Width + 1;
            while (frames > 0)
            {
                animationConverter.AddFrame(GetBitmap());
                if(x!=ActiveFrame.X)
                {
                    x = ActiveFrame.X;
                    frames--;
                }
            }
            if (animationConverter.Encode(path, 0, animationConverter.animation.FramesCount - 1, AnimationConverter.ColorOrder.RGB, AnimationConverter.PixelOrder.VS_TR, AnimationConverter.DataType.Default))
            {
                SetLBResourcePath(path);
                return true;
            }
            return false;
        }
        public Point GetPosition()
        {
            return TextArea.Location;
        }
        public void SetPosition(Point p)
        {
            TextArea.Location = p;
        }
        public void MovePosition(Point dp)
        {
            TextArea.X += dp.X;
            TextArea.Y += dp.Y;
        }
        public Rectangle GetArea()
        {
            return TextArea;
        }
        public string GetToolName()
        {
            return LB_Tools_RunningText_Name;
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
        public void SetLBResourcePath(string path)
        {
            LbResourcePath = path;
            LbResourceName = LB_Tools.GetFileName(path);
        }
    }
}
