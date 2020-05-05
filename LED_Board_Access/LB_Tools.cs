using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace LED_Board_Access
{
  public static class LB_Tools
    {
      public static bool isItToolArea(LB_Tools_Interface tool, Point p)
      {
          return tool.GetArea().Contains(p);
      }
      public static string GetFileName(string path)
      {
          if (path != "")
          {
              return Path.GetFileName(path);
          }
          return "";
      }
    }
}
