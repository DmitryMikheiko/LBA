using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LED_Board_Access
{
   public interface IDocument
    {

         string GetExtensionName();
         string GetExtension();
         string GetName();
         void Rename(string name);
    }
}
