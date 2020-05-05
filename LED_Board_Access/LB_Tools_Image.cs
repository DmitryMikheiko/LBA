using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;
using VisualEffectsConverter;

namespace LED_Board_Access
{
    class LB_Tools_Image: LB_Tools_Interface
    {

        private AnimationConverter animationConverter;
        private Point ImagePosition;
        private Size  ImageSize;
        private Bitmap bitmap=null;
        private string ImagePathOld="";
        private string ImagePath="";
        private Size MaxSize = new Size(24, 24);

        [Category("Component")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string LB_Tools_Image_Name
        {
            get { return "Tools_Image"; }
        }
        [Category("Layout")]
        [DisplayName("Location")]
        public Point position
        {
            get { return ImagePosition; }
            set { ImagePosition = value; }
        }
        [Category("Main")]
        [DisplayName("Path")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Filename 
        { 
            get
            {
                return ImagePath;
            }
            set
            {
                ImagePath = value;
            }
        }

        [Category("Layout")]
        [DisplayName("Size")]
        [ReadOnly(true)]
        public Size size
        {
            get
            {
                return ImageSize;
            }
            set
            {
                ImageSize = value;
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
                ReLoadImage();
            }
        }
        public string LbResourcePath = "";
        public string LbResourceName = "";
        public LB_Tools_Image()
        {

        }
        public LB_Tools_Image(string path)
        {
            ImagePath = path;
        }
        private void LoadImage()
        {
            if(ImagePath!=ImagePathOld && ImagePath!="")
            {
                ImagePathOld = ImagePath;              

                FileInfo f_info = new FileInfo(ImagePath);               
                switch(f_info.Extension)
                {
                    case ".bmp":
                        if (f_info.Extension == ".bmp")
                        {
                            Bitmap tempBitmap = new Bitmap(ImagePath);
                            if (tempBitmap.Height > MaxSize.Height || tempBitmap.Width > MaxSize.Width)
                            {
                                bitmap = new Bitmap(MaxSize.Width, MaxSize.Height);
                                Graphics g = Graphics.FromImage(bitmap);
                                g.DrawImage(tempBitmap, new Rectangle(0, 0, MaxSize.Width, MaxSize.Height));
                                tempBitmap.Dispose();
                            }
                            else
                            {
                                bitmap = new Bitmap(tempBitmap);
                                tempBitmap.Dispose();
                            }
                            animationConverter = new AnimationConverter(bitmap);
                            string bma_path =Path.GetDirectoryName(ImagePath) +"\\" + Path.GetFileNameWithoutExtension(ImagePath) + ".bma";
                            if (animationConverter.Encode(bma_path, 0, 0, AnimationConverter.ColorOrder.RGB, AnimationConverter.PixelOrder.VS_TR, AnimationConverter.DataType.Default))
                           {
                               LbResourcePath = bma_path;
                               LbResourceName = LB_Tools.GetFileName(LbResourcePath);
                           }
                        }
                        break;
                    case ".jpg":
                        Stream imageStreamSource = new FileStream(ImagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        JpegBitmapDecoder decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapSource = decoder.Frames[0];
                        bitmap = BitmapFromSource(bitmapSource);
                        break;
                    default: 

                        break;
                }
            }
        }
        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }
        public Size GetSize()
        {
            return GetBitmap().Size;
        }
        public void ReLoadImage()
        {
            ImagePathOld = "";
            LoadImage();
            if(bitmap!=null)
            {
                size = bitmap.Size;
            }
        }
        public Bitmap GetBitmap()
        {
            LoadImage();
            if(bitmap == null)
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
            return ImagePosition;
        }
        public void SetPosition(Point p)
        {
            ImagePosition = p;
        }
        public void MovePosition(Point dp)
        {
            ImagePosition.Offset(dp);
        }
        public Rectangle GetArea()
        {
            Rectangle rect = new Rectangle(ImagePosition, size);
            return rect;
        }
        public string GetToolName()
        {
            return LB_Tools_Image_Name;
        }
        public string GetResourcePath()
        {
            return ImagePath;
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
