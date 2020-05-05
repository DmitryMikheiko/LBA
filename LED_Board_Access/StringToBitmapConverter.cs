using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LED_Board_Access
{
    class StringToBitmapConverter
    {
       public TextRenderingHint _TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
       public SmoothingMode _SmoothingMode = SmoothingMode.None;
       public InterpolationMode _InterpolationMode = InterpolationMode.Default;

        public Size GetSize(string s,Font font)
        {
            return GetBitmap(s, font, Brushes.Red, Color.Black).Size;
        }
        public Bitmap GetBitmap(string s,Font font,Brush FontBrush,Color BackColor)
        {
            Size size = TextRenderer.MeasureText(s, font);
            Bitmap b;
            Graphics g;
            if (size.Height == 0 || size.Width == 0)
            {
                size.Width = (int)font.Size;
                size.Height = (int)font.Size;
                b = new Bitmap(size.Width, size.Height);
                g = Graphics.FromImage(b);
                Pen pen = new Pen(FontBrush,1);
                size.Height--;
                size.Width--;
                Rectangle rect = new Rectangle(new Point(0,0),size);
                g.DrawRectangle(pen, rect);
                return b;
            }
            b = new Bitmap(size.Width, size.Height);
            g = Graphics.FromImage(b);
            g.TextRenderingHint = _TextRenderingHint;
            g.SmoothingMode = _SmoothingMode;
            g.InterpolationMode = _InterpolationMode;
            g.Clear(BackColor);
            g.DrawString(s, font, FontBrush, 0, 0);
            return  GetReduceBitmap(b, BackColor);
        }
        public Bitmap GetReduceBitmap(Bitmap b,Color BackColor)
        {
            int x_min=0,x_max=b.Width-1,y_min=0,y_max=b.Height-1;
            bool f=false;
            int x,y;
            int  BackColorARGB = BackColor.ToArgb();
            for(x=0;x<b.Width && !f;x++)
                for(y=0;y<b.Height;y++)
                {
                    if (b.GetPixel(x, y).ToArgb() != BackColorARGB)
                    {
                        f = true;
                        x_min = x;
                        break;
                    }
                }
            f = false;
            for (x = b.Width -1; x >=x_min && !f; x--)
                for (y = 0; y < b.Height; y++)
                {
                    if (b.GetPixel(x, y).ToArgb() != BackColorARGB)
                    {
                        f = true;
                        x_max = x;
                        break;
                    }
                }
            f = false;
            for (y = 0; y < b.Height && !f; y++)
                for (x = x_min; x <= x_max; x++)
                {
                    if (b.GetPixel(x, y).ToArgb() != BackColorARGB)
                    {
                        f = true;
                        y_min = y;
                        break;
                    }
                }
            f = false;
            for (y = b.Height -1; y >= y_min && !f; y--)
                for (x = x_min; x <= x_max; x++)
                {
                    if (b.GetPixel(x, y).ToArgb() != BackColorARGB)
                    {
                        f = true;
                        y_max = y;
                        break;
                    }
                }
            Size size_new = new Size(x_max - x_min + 1,y_max - y_min + 1);
            Bitmap b2 = new Bitmap(size_new.Width,size_new.Height);
             
            Rectangle rect_des = new Rectangle(new Point(x_min,y_min),size_new);
            Graphics g =Graphics.FromImage(b2);
            g.DrawImage(b, 0, 0, rect_des, GraphicsUnit.Pixel);

                return b2;
        }
     
    }
}
