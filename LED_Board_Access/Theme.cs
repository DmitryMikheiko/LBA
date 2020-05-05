using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace LED_Board_Access
{
    public class Theme : IDocument
    {
        ThemeCreator themeCreator;
        string name;
        Color backColor;
        Color transparentKey = Color.Black;

        public const string Extension = ".lbt";
        public const string ExtensionName = "LED Board Theme";

        public List<LB_Tools_Interface> Tools;

        public int ThemeBackColorARGB;
        public int ThemeTransparentKeyARGB;
        
        [Category("Main")]
        [DisplayName("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [Category("Appearance")]
        [DisplayName("BackColor")]
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; ThemeBackColorARGB = value.ToArgb(); }
        }
        [Category("Appearance")]
        [DisplayName("TransparentKey")]
        public Color TransparentKey
        {
            get { return transparentKey; }
            set { transparentKey = value; ThemeTransparentKeyARGB = value.ToArgb(); }
        }
        public Theme()
        {
            Tools = new List<LB_Tools_Interface>();
        }
        public void AddTool(LB_Tools_Interface tool)
        {
            Tools.Add(tool);
        }
        public void RemoveTool(LB_Tools_Interface tool)
        {
            Tools.Remove(tool);
        }
        public List<LB_Tools_Interface> GetTools()
        {
            return new List<LB_Tools_Interface>(Tools);
        }
        public void ToolUp(LB_Tools_Interface tool)
        {            
            int index = Tools.IndexOf(tool);
            if (index < (Tools.Count - 1))
            {
                Tools.Remove(tool);
                Tools.Insert(index + 1, tool);
            }
        }
        public void ToolDown(LB_Tools_Interface tool)
        {
            int index = Tools.IndexOf(tool);
            if (index > 0)
            {
                Tools.Remove(tool);
                Tools.Insert(index - 1, tool);
            }
        }
        public string GetExtensionName()
        {
            return ExtensionName;
        }
        public string GetExtension()
        {
            return Extension;
        }
        public string GetName()
        {
            return name;
        }
        public void Rename(string name)
        {
            this.name = name;
        }
        public void SetThemeCreator(ThemeCreator creator)
        {
            themeCreator = creator;
        }
        public ThemeCreator GetThemeCreator()
        {
            return themeCreator;
        }
    }
}
