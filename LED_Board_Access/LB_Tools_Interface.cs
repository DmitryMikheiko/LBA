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
  public interface LB_Tools_Interface
    {

        Size GetSize();
        Bitmap GetBitmap();
        Point GetPosition();
        void SetPosition(Point p);
        void MovePosition(Point dp);
        Rectangle GetArea();
        string GetToolName();
        string GetResourcePath();
        string GetLBResourcePath();
        string GetLBResourceName();
     /* public LB_Tools_Interface()
      {

      }
      public virtual Size GetSize()
        {
          return new Size(0, 0);
        }
      public virtual Bitmap GetBitmap()
        {
            return null;
        }
      public virtual Point GetPosition()
        {
            return new Point(0, 0);
        }
      public virtual void SetPosition(Point p)
        {

        }
      public virtual void MovePosition(Point dp)
        {

        }
      public virtual Rectangle GetArea()
        {
            return new Rectangle(0, 0, 0, 0);
        }*/
    }
}
