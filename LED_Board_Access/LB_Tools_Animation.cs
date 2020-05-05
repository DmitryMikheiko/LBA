using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using VisualEffectsConverter;

namespace LED_Board_Access
{
    class LB_Tools_Animation : LB_Tools_Interface
    {

        private Point AnimationPosition;
        private Size AnimationSize;
        private Bitmap bitmap = null;
        private string AnimationPathOld = "";
        private string AnimationPath = "";
        private Size MaxSize = new Size(24, 24);

        //******************Animation********************
        AnimationConverter animationConverter;
        //***********************************************
        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_Animation_Name
        {
            get { return "Tools_Animation"; }
        }
        [Category("Layout")]
        [DisplayName("Location")]
        public Point position
        {
            get { return AnimationPosition; }
            set { AnimationPosition = value; }
        }
        [Category("Main")]
        [DisplayName("Path")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Filename
        {
            get
            {
                return AnimationPath;
            }
            set
            {
                AnimationPath = value;
            }
        }

        [Category("Layout")]
        [DisplayName("Size")]
        [ReadOnly(true)]
        public Size size
        {
            get
            {
                return AnimationSize;
            }
            set
            {
                AnimationSize = value;
            }
        }
        [Category("Layout")]
        [DisplayName("MaxSize")]
        public Size maxSize
        {
            get
            {
                return MaxSize;
            }
            set
            {
                MaxSize = value;
                //ReLoadImage();
            }
        }
        public string LbResourcePath = "";
        public string LbResourceName = "";
        public LB_Tools_Animation()
        {
            animationConverter = new AnimationConverter();
        }
        private void LoadAnimation()
        {
            if (AnimationPath != AnimationPathOld)
            {
                AnimationPathOld = AnimationPath;
                animationConverter.Decode(AnimationPath);
                animationConverter.SetFirstFrame();
                LbResourcePath = AnimationPathOld;
                LbResourceName = LB_Tools.GetFileName(LbResourcePath);
            }           
        }
        public Size GetSize()
        {
            return GetBitmap().Size;
        }

        public Bitmap GetBitmap()
        {
            LoadAnimation();
            Bitmap frame = animationConverter.GetNextFrame();
            if(frame != null)
            {
                if (frame.Size.Width > maxSize.Width || frame.Size.Height > maxSize.Height)
                {
                    bitmap = new Bitmap(MaxSize.Width, MaxSize.Height);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.DrawImage(frame, new Rectangle(0, 0, MaxSize.Width, MaxSize.Height));
                }
                else bitmap = frame;
            }
            else if (bitmap == null)
            {
                Size tempSize = new Size(5, 5);
                bitmap = new Bitmap(tempSize.Width, tempSize.Height);
                Graphics g = Graphics.FromImage(bitmap);
                Pen pen = new Pen(Color.Blue, 1);
                tempSize.Height--;
                tempSize.Width--;
                Rectangle rect = new Rectangle(new Point(0, 0), tempSize);
                g.DrawRectangle(pen, rect);
            }
            size = bitmap.Size;
            return bitmap;
        }
        public Point GetPosition()
        {
            return AnimationPosition;
        }
        public void SetPosition(Point p)
        {
            AnimationPosition = p;
        }
        public void MovePosition(Point dp)
        {
            AnimationPosition.Offset(dp);
        }
        public Rectangle GetArea()
        {
            Rectangle rect = new Rectangle(AnimationPosition, size);
            return rect;
        }
        public string GetToolName()
        {
            return LB_Tools_Animation_Name;
        }
        public string GetResourcePath()
        {
            return AnimationPath;
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
