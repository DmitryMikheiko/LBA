using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Media;
using System.Windows.Media.Imaging;
using SOE_IDE;

namespace LED_Board_Access
{
    public class ThemeCreator: IDisposable
    {
        private bool disposed = false;

        ThemeCreatorControl Control;
        TabPage tabPage;
        Theme theme;
        private int DotSize = 12;
        private int DotSpace = 1;
        private Size BoardSize = new Size(10, 10);
        private Point MatrixPosition = new Point(10, 10);
        private Point SelectedPixel = new Point(-1, -1);
        private Point MouseOnPixel = new Point(-1, -1);
        private Point MatrixMousePositionPrev = new Point(-1, -1);
        private LB_Tools_Interface SelectedTool;
        private Bitmap BoardImage;
        private StringToBitmapConverter ToBitmap;
        private PictureBoxToolTip pictureBoxToolTip = new PictureBoxToolTip();
  
        LB_Tools_Interface NewTool;

        private Thread DisplayThread;
        private bool DisplayState = false;
        public event EventHandler ThemeChanged;
        public event EventHandler ThemeNameChanged;
        

        public enum Tools
        {
            Clock,
            Text,
            Image,
            Animation
        }

        public ThemeCreator (Project project)
        {
            theme = new Theme(project);
            theme.Name = "NewTheme";
            ThemeCreatorInit();
        }
        public ThemeCreator(Theme theme)
        {
            if(theme!=null)
            {
                this.theme = theme;
            }
            else
            {
               this.theme = new Theme(null);
               this.theme.Name = "NewTheme";
            }
            ThemeCreatorInit();
        }
        private void ThemeCreatorInit()
        {
            Control = new ThemeCreatorControl();
            tabPage = new TabPage();
            if (theme.GetProject() != null && !theme.GetProject().GetBoardSize().Equals(new Size(0,0)))
            {
                BoardSize = theme.GetProject().GetBoardSize();
            }
            Control.PictureBox.Size = new Size(BoardSize.Width * (DotSize + DotSpace) - DotSpace + MatrixPosition.X * 2,
                                                BoardSize.Height * (DotSize + DotSpace) - DotSpace + MatrixPosition.Y * 2);
          //  tabPage.Location = new System.Drawing.Point(0, 0);
            tabPage.Name = "ThemeCreator";
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            //tabPage.Size = new System.Drawing.Size(1246, 875);
           // tabPage.TabIndex = 3;
            tabPage.Text = theme.Name;
           // this.tabPage.Location = new System.Drawing.Point(4, 22);
           // this.tabPage.Size = new System.Drawing.Size(991, 682);
            tabPage.UseVisualStyleBackColor = true;
            Control.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            tabPage.Size = new Size(Control.Size.Width, Control.Size.Height);
   

            tabPage.Controls.Add(Control);

            // this.Control.Location = new System.Drawing.Point(6, 6);
            // this.Control.Size = new System.Drawing.Size(959, 668);


            BoardImage = new Bitmap(BoardSize.Width, BoardSize.Height);

            Control.PictureBox.Paint += new PaintEventHandler(PictureBox_Paint);
            Control.button_Animation.Click += new EventHandler(Toolbox_Animation_click);
            Control.button_Clock.Click += new EventHandler(Toolbox_Clock_click);
            Control.button_Image.Click += new EventHandler(Toolbox_Image_click);
            Control.button_Pointer.Click += new EventHandler(Toolbox_Pointer_click);
            Control.button_Text.Click += new EventHandler(Toolbox_Text_click);
            Control.button_RunningText.Click += new EventHandler(Toolbox_RunningText_click);
            Control.button_TimeSecProgressBar.Click += new EventHandler(Toolbox_TimesSecProgressBar_click);
            Control.PictureBox.MouseMove += new MouseEventHandler(PictureBox_MouseMove);
            Control.PictureBox.Click += new EventHandler(PictureBox_Click);
            Control.PictureBox.MouseUp += new MouseEventHandler(PictureBox_MouseUp);
            Control.propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(Tool_Property_Changed);
           

            pictureBoxToolTip.Visible = false;

            Control.PictureBox.Controls.Add(pictureBoxToolTip);

            DisplayThread = new Thread(new ThreadStart(DisplayProcc));
            DisplayState = true;
            DisplayThread.Start();
            tabPage.Disposed += new EventHandler(TabDisposed);
            this.theme.SetThemeCreator(this);
        }
        
        private void TabDisposed(object sender, EventArgs e)
        {
            Dispose();
        }
        public void Dispose()
        {
        Dispose(true);
        GC.SuppressFinalize(this);
        }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                if (DisplayThread != null)
                {
                    DisplayState = false;
                    DisplayThread.Abort();
                }
            }
            
            disposed = true;
        }
    }

    ~ThemeCreator()
    {
        Dispose (false);
    }
        public void Show(TabControl tabControl)
        {
            tabControl.KeyDown += new KeyEventHandler(KeyPressed);
            tabControl.Controls.Add(tabPage);
            tabPage.Tag = theme;
            tabControl.SelectedTab = tabPage;
        }
        public Theme GetTheme()
        {
            return theme;
        }
        private void DisplayProcc()
        {
            while (DisplayState)
            {
                BoardImage = GetBitmap();
                Control.PictureBox.Invalidate();
                Thread.Sleep(1000 / 25);
            }
        }
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle();
            Rectangle rect2 = new Rectangle();
            Rectangle rect3 = new Rectangle();
            Brush brush;
            Pen pen1 = new Pen(AppAppearance.MatrixCellSelectColor, 2);
            Pen pen2 = new Pen(Color.Black, 1);
            Pen pen3 = new Pen(AppAppearance.MatrixToolSelectColor, 2);
            Point p1 = new Point();
            Point p2 = new Point();
            Color PixelColor;
            int BlackARGB = Color.Black.ToArgb();           
            rect.Height = DotSize;
            rect.Width = DotSize;
            rect2.Height = DotSize + DotSpace;
            rect2.Width = DotSize + DotSpace;
            rect3.Height = DotSize - 1;
            rect3.Width = DotSize - 1;
            for (int x = 0; x < BoardImage.Width; x++)
                for (int y = 0; y < BoardImage.Height; y++)
                {
                    if (x == SelectedPixel.X && y == SelectedPixel.Y)
                    {
                        rect2.X = x * (DotSize + DotSpace) + MatrixPosition.X - (DotSpace / 2);
                        rect2.Y = y * (DotSize + DotSpace) + MatrixPosition.Y- (DotSpace / 2);
                        g.DrawRectangle(pen1, rect2);
                    }
                    rect.X = x * (DotSize + DotSpace) + MatrixPosition.X;
                    rect.Y = y * (DotSize + DotSpace) + MatrixPosition.Y;
                    rect3.X = x * (DotSize + DotSpace) + MatrixPosition.X;
                    rect3.Y = y * (DotSize + DotSpace) + MatrixPosition.Y;
                    PixelColor = BoardImage.GetPixel(x, y);
                   // if (PixelColor.ToArgb() != BlackARGB)
                    {
                        brush = new SolidBrush(PixelColor);
                        g.FillRectangle(brush, rect);
                        g.DrawRectangle(pen2, rect3);
                    }
                  /*  else
                    {
                        brush = new SolidBrush(AppAppearance.DefaultMatrixColor);
                        g.FillRectangle(brush, rect);
                        g.DrawRectangle(pen2, rect3);
                        p1.X = rect3.X;
                        p1.Y = rect3.Y;
                        p2.X = p1.X + rect3.Width;
                        p2.Y = p1.Y + rect3.Height;
                        g.DrawLine(pen2, p1, p2);
                        p1.X = rect3.X + rect3.Width;
                        p1.Y = rect3.Y;
                        p2.X = rect3.X;
                        p2.Y = p1.Y + rect3.Height;
                        g.DrawLine(pen2, p1, p2);
                    }*/

                }
            if(SelectedTool!=null)
            {
                Rectangle toolArea = SelectedTool.GetArea();
                toolArea.Height = toolArea.Height * (DotSize + DotSpace);
                toolArea.Width  = toolArea.Width  * (DotSize + DotSpace);
                toolArea.X = toolArea.X * (DotSize + DotSpace) + MatrixPosition.X - (DotSpace / 2);
                toolArea.Y = toolArea.Y * (DotSize + DotSpace) + MatrixPosition.Y - (DotSpace / 2);
                g.DrawRectangle(pen3, toolArea);
            }
            //Thread.Sleep(10);
        }
        private void Toolbox_Tool_SetActive(object sender)
        {
            foreach(Button b in Control.Toolbox_tabpage.Controls)
            {
                b.BackColor = AppAppearance.Buttons_BackColor;
            }
            Button button = (Button)sender;
            button.BackColor = AppAppearance.Buttons_MouseOverBackColor;
            button.Focus();
        }
        private void Toolbox_Pointer_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            Button button = (Button)sender;
            NewTool = null;
        }
        private void Toolbox_Text_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_Text();
        }
        private void Toolbox_Clock_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_Clock();
        }
        private void Toolbox_Image_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_Image();
        }
        private void Toolbox_Animation_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_Animation();
        }
        private void Toolbox_RunningText_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_RunningText();
        }
        private void Toolbox_TimesSecProgressBar_click(object sender, EventArgs e)
        {
            Toolbox_Tool_SetActive(sender);
            NewTool = new LB_Tools_TimeSecProgressBar();
        }
        private void NewToolAdded()
        {
            Toolbox_Tool_SetActive(Control.button_Pointer);
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            SelectedPixel = MouseOnPixel;
            if (NewTool != null)
            {
                NewTool.SetPosition(SelectedPixel);
                theme.AddTool(NewTool); // Add new tool to collection
                Control.propertyGrid1.SelectedObject = NewTool;
                NewToolAdded(); // call method for reset NewTool and set focus on button_pointer
                NewTool = null;
                Theme_Changed();
            }
            SelectedTool = null; 
            Control.PictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            foreach(LB_Tools_Interface tool in theme.GetTools())
            {
                if (LB_Tools.isItToolArea(tool, SelectedPixel))
                {
                    Control.propertyGrid1.SelectedObject = tool;
                    SelectedTool = tool;
                    Control.PictureBox.Cursor = System.Windows.Forms.Cursors.SizeAll;
                }
            }
            if(SelectedTool==null)
            {
                Control.propertyGrid1.SelectedObject = theme;
            }
            tabPage.Focus();
            Control.PictureBox.Invalidate();
        }
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            MatrixMousePositionPrev.X = -1;
            MatrixMousePositionPrev.Y = -1;
        }
        private Point GetMatriMousePosition(MouseEventArgs e)
        {
            int x, y;
            Point p = new Point(e.X - MatrixPosition.X, e.Y - MatrixPosition.Y);
            Point p2 = new Point(-1, -1);
            if (p.X > 0 && p.Y > 0)
            {
                for (x = 0; x < BoardSize.Width; x++)
                    for (y = 0; y < BoardSize.Height; y++)
                    {
                        if (p.X >= (x * (DotSpace + DotSize)) && p.X < (x * (DotSpace + DotSize) + DotSize))
                        {
                            if (p.Y >= (y * (DotSpace + DotSize)) && p.Y < (y * (DotSpace + DotSize) + DotSize))
                            {
                                p2.X = x;
                                p2.Y = y;
                            }
                        }
                    }

            }
            if (p2.X >= 0 && p2.Y >= 0)
            {

            }
            else
            {

            }
            return p2;
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPixel = GetMatriMousePosition(e);
            if (MouseOnPixel.X == -1 || MouseOnPixel.Y == -1)
            {
                pictureBoxToolTip.Visible = false;
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SelectedPixel = MouseOnPixel;
                if(SelectedTool!=null && MatrixMousePositionPrev.X != -1 && MatrixMousePositionPrev.Y != -1)
                {
                    if(MouseOnPixel != MatrixMousePositionPrev)
                    {
                        SelectedTool.MovePosition(new Point(MouseOnPixel.X - MatrixMousePositionPrev.X, MouseOnPixel.Y  - MatrixMousePositionPrev.Y));
                        Theme_Changed();
                    }
                }
                MatrixMousePositionPrev = MouseOnPixel;               
                Control.PictureBox.Invalidate();
            }

                pictureBoxToolTip.Visible = true;
                if (MouseOnPixel.X < 20 && MouseOnPixel.Y < 20) pictureBoxToolTip.Location = new Point(e.X + 20, e.Y + 20);
                else pictureBoxToolTip.Location = new Point(e.X - 80, e.Y - 20);
                pictureBoxToolTip.SetPosition(MouseOnPixel);
            

        }
        private Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(BoardSize.Width, BoardSize.Height);
            ClearBitmap(bitmap, theme.BackColor);
   
            foreach(object tool in theme.GetTools())
            {
                if(tool!=null && tool.GetType()==typeof(LB_Tools_Text))
                {
                    LB_Tools_Text lb_tools_text = (LB_Tools_Text)tool;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_text.GetBitmap(), lb_tools_text.position, theme.TransparentKey);
                }
                else if(tool!=null && tool.GetType()==typeof(LB_Tools_Clock))
                {
                    LB_Tools_Clock lb_tools_clock = tool as LB_Tools_Clock;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_clock.GetBitmap(), lb_tools_clock.position, theme.TransparentKey);
                }
                else if (tool != null && tool.GetType() == typeof(LB_Tools_Image))
                {
                    LB_Tools_Image lb_tools_clock = tool as LB_Tools_Image;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_clock.GetBitmap(), lb_tools_clock.position, theme.TransparentKey);
                }
                else if (tool != null && tool.GetType() == typeof(LB_Tools_Animation))
                {
                    LB_Tools_Animation lb_tools_clock = tool as LB_Tools_Animation;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_clock.GetBitmap(), lb_tools_clock.position, theme.TransparentKey);
                }
                else if (tool != null && tool.GetType() == typeof(LB_Tools_RunningText))
                {
                    LB_Tools_RunningText lb_tools_clock = tool as LB_Tools_RunningText;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_clock.GetBitmap(), lb_tools_clock.position, theme.TransparentKey);
                }
                else if (tool != null && tool.GetType() == typeof(LB_Tools_TimeSecProgressBar))
                {
                    LB_Tools_TimeSecProgressBar lb_tools_clock = tool as LB_Tools_TimeSecProgressBar;
                    CopyBitmapWithTransparentKey(bitmap, lb_tools_clock.GetBitmap(), lb_tools_clock.position, theme.TransparentKey);
                }
            }
            return bitmap;
        }
        private void Theme_Changed()
        {
            EventHandler handler = ThemeChanged;
            handler(theme, EventArgs.Empty);
            Control.PictureBox.Invalidate();
            if (tabPage.Text != theme.Name)
            {
                tabPage.Text = theme.Name;
                handler = ThemeNameChanged;
                handler(theme, EventArgs.Empty);
            }

        }
        private void Tool_Property_Changed(Object sender, PropertyValueChangedEventArgs e)
        {
            Theme_Changed();
        }
        private void CopyBitmap(Bitmap srcBitmap,Bitmap dscBitmap,Point srcPos)
        {
            Graphics g = Graphics.FromImage(srcBitmap);
            g.DrawImage(dscBitmap, srcPos);
        }
        private void CopyBitmapWithTransparentKey(Bitmap srcBitmap, Bitmap dscBitmap, Point srcPos,Color TransparentKey)
        {
            int x, y;
            Color pixel;
            int TransparentKeyARGB = TransparentKey.ToArgb();
            int src_x = srcPos.X;
            Rectangle src_area = new Rectangle(0,0,srcBitmap.Width,srcBitmap.Height);

            for (y = 0; y < dscBitmap.Height; y++, srcPos.Y++)
                for (x = 0,srcPos.X =src_x; x < dscBitmap.Width; x++, srcPos.X++)
                {
                    pixel = dscBitmap.GetPixel(x, y);
                    if (pixel.ToArgb() != TransparentKeyARGB && src_area.Contains(srcPos)) srcBitmap.SetPixel(srcPos.X, srcPos.Y, pixel);
                }

        }
        private void ClearBitmap(Bitmap bitmap,Color color)
        {
           Graphics g = Graphics.FromImage(bitmap);
           g.Clear(color);
        }
        public void SelectTool(LB_Tools_Interface tool)
        {
            SelectedTool = tool;
            Control.propertyGrid1.SelectedObject = tool;
            tabPage.Focus();
        }
        public void KeyPressed(object sender, KeyEventArgs e)
        {
            TabControl tc = sender as TabControl;
            if (tc.SelectedTab == this.tabPage)
            {
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        if(SelectedTool!=null)
                        {
                            theme.RemoveTool(SelectedTool);
                            SelectedTool = null;
                            Control.PictureBox.Invalidate();
                            Theme_Changed();
                        }
                        break;
                    default: break;
                }
            }
        }
   
    }
}
