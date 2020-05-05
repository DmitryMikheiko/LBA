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
  public class LB_Tools_Text : LB_Tools_Interface
    {
        StringToBitmapConverter stbc;

        private string TextName;
        private string Text;
        private Font TextFont;
        private Color FontColor;
        private Color BackColor;
        private TextRenderingHint _textRenderingHint;
        private SmoothingMode _smoothingMode;
        private InterpolationMode _interpolationMode;
        private Point TextPosition;
        private Size TextSize;

        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_Text_Name
        {
            get { return "Tools_Text"; }
        }
        [Category("Component")]
        [DisplayName("TextName")]
        public string Text_Name
        {
            get { return TextName;  }
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

        [Category("Rendering")]
        [DisplayName("SmoothingMode")]
        private SmoothingMode smoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        [Category("Rendering")]
        [DisplayName("InterpolationMode")]
        private InterpolationMode interpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode = value; }
        }
        [Category("Layout")]
        [DisplayName("Location")]
        public Point position
        {
            get { return TextPosition; }
            set { TextPosition = value; }
        }
        [Category("Layout")]
        [DisplayName("Size")]
        [ReadOnly(true)]
        public Size size
        {
            get { return TextSize; }
            set { TextSize = value; }
        }
        public string LbResourcePath = "";
        public string LbResourceName = "";
        public LB_Tools_Text()
        {
            TextFont = new Font("Arial", 8, FontStyle.Regular);
            Text = "str";
            FontColor= Color.Red;
            BackColor = Color.Black;
            stbc = new StringToBitmapConverter();
            textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
        }
        public Size GetSize()
        {
            return GetBitmap().Size;
        }
        public Bitmap GetBitmap()
        {
            Bitmap bitmap;
            stbc._InterpolationMode = interpolationMode;
            stbc._SmoothingMode = smoothingMode;
            stbc._TextRenderingHint = textRenderingHint;
            bitmap = stbc.GetBitmap(text, font, new SolidBrush(fontColor), backColor);
            TextSize = bitmap.Size;
            return bitmap;
        }
        public Point GetPosition()
        {
            return TextPosition;
        }
        public void SetPosition(Point p)
        {
            TextPosition = p;
        }
        public void MovePosition(Point dp)
        {
            TextPosition.Offset(dp);
        }
        public Rectangle GetArea()
        {
            Rectangle rect = new Rectangle(position, size);
            return rect;
        }
        public string GetToolName()
        {
            return LB_Tools_Text_Name;
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
