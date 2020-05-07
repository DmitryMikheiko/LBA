using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using VisualEffectsConverter;

namespace LED_Board_Access
{
    public partial class Form1 : Form
    {
        private object SelectedDocument = null; //IDocument
        private Solution solution;
        private NewProjectDialog newProjectDialog;
        ProjectTreeBuilder projectTreeBuilder;
        LED_Board_Manager lb_manager;
        public Form1(string path)
        {
            InitializeComponent();
            solution = new Solution();
            projectTreeBuilder = new ProjectTreeBuilder(solution, this.Project_treeView);
            treeView_Context_Delete.Click += new EventHandler(treeView_Context_Delete_Click);
            treeView_Context_Rename.Click += new EventHandler(treeView_Context_Rename_Click);
            treeView_Context_Up.Click += new EventHandler(treeView_Context_Up_Click);
            treeView_Context_Down.Click += new EventHandler(treeView_Context_Down_Click);
            treeView_Context_Close.Click += new EventHandler(treeView_Context_Delete_Click);

            if(path != "")
            {
                if(Path.GetExtension(path) == Project.Extension){
                    LoadProject(path);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void themeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            solution.AddItem(CreateTheme());
            RealoadProjectTree();
        }
        private Theme CreateTheme()
        {
            ThemeCreator themeCreator = new ThemeCreator(solution.GetProject());
            themeCreator.Show(tabControl_Main);
            themeCreator.ThemeChanged += new EventHandler(ThemeChanged);
            themeCreator.ThemeNameChanged += new EventHandler(ThemeNameChanged);
            SelectedDocument = themeCreator.GetTheme();
            return themeCreator.GetTheme();
        }
        private void LoadTheme(Theme theme)
        {
            ThemeCreator themeCreator = new ThemeCreator(theme);
            themeCreator.Show(tabControl_Main);
            themeCreator.ThemeChanged += new EventHandler(ThemeChanged);
            themeCreator.ThemeNameChanged += new EventHandler(ThemeNameChanged);
            SelectedDocument = theme;
        }
        private void ThemeChanged(object sender, EventArgs e)
        {
            Theme theme = sender as Theme;
            saveToolStripMenuItem.Text = "Save '" + theme.Name + "'";
            saToolStripMenuItem.Text = "Save '" + theme.Name + "' As";
            SelectedDocument = theme;
            RealoadProjectTree();
        }
        private void ThemeNameChanged(object sender, EventArgs e)
        {
            solution.RenameDocument(solution.GetItemDocument((IDocument)sender), ((IDocument)sender).GetName());
        }
        private void RealoadProjectTree()
        {
            projectTreeBuilder.ReloadTree();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedDocument != null && SelectedDocument.GetType().GetInterfaces().Contains(typeof(IDocument)))
            {
                solution.SaveItem((IDocument)SelectedDocument);
                RealoadProjectTree();
            }
        }

        private void saToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedDocument != null && SelectedDocument.GetType().GetInterfaces().Contains(typeof(IDocument)))
            {
                solution.SaveItemAs((IDocument)SelectedDocument);
                RealoadProjectTree();
            }
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            newProjectDialog = new NewProjectDialog();
            if (newProjectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Project project = new Project();
                project.SetBoardType(newProjectDialog.GetBoard());
                project.ProjectName = newProjectDialog.GetName();
                solution.AddItem(project);
                Document document = solution.GetItemDocument(project);
                document.SetPath(newProjectDialog.GetPath());
                solution.SaveItem(project);
                foreach(string dir in project.GetProjectDirectories())
                {
                    string path = Path.GetDirectoryName(newProjectDialog.GetPath()) + "\\" + dir;
                    DirectoryInfo dir_info = new DirectoryInfo(path);
                    if (!dir_info.Exists) dir_info.Create();
                }
                RealoadProjectTree();
            }
        }

        private void addThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project project = solution.GetProject();
            if (project == null)
            {
                MessageBox.Show("You must create or open project previously.", "Error", MessageBoxButtons.OK);
                return;
            }
            string path = ShowOpenFileDialog(Theme.ExtensionName, Theme.Extension);
            if (path != "")
            {
                solution.AddItem(path, project);
                RealoadProjectTree();
            }
        }
        private string ShowOpenFileDialog(string FileExtensionName, string FileExtension)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = FileExtensionName + " (*" + FileExtension + ")|*" + FileExtension;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else return "";
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadProject(ShowOpenFileDialog(Project.ExtensionName, Project.Extension));
        }
        private void LoadProject(string path)
        {
            if (path == "") return;
            if (solution != null)
            {
                Project pr = solution.GetProject();
                if (pr != null)
                {
                    if (MessageBox.Show("Do you want to close current project?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        CloseTabIfOpened(pr);
                        solution.CloseDocumentWithQuestion(solution.GetItemDocument(pr));
                    }
                    else return;
                }
            }
            foreach (Project pr in solution.GetProjects()) // seach the same opened project
            {
                if (solution.GetItemDocument(pr).GetPath() == path)
                {
                    MessageBox.Show("The project '" + pr.GetName() + "' is already opened", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            Project project = (Project)solution.AddItem(path);
            if (project != null) LoadProjectFiles(project);
            RealoadProjectTree();
        }
        private void LoadProjectFiles(Project project)
        {
            string project_directory = solution.GetItemDocument(project).GetDirectory();
            string file_path;
            foreach (string file_name in project.GetDocumentNames())
            {
                foreach (string directory in project.GetProjectDirectories())
                {
                    file_path = project_directory + directory + file_name;
                    IDocument item = solution.AddItem(file_path, project);
                    if (item == null) // !=
                    {
                        break;
                    }
                    if(item.GetExtension() == Theme.Extension)
                    {
                        ((Theme)item).SetProject(project);
                    }
                }
            }
        }

        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = ShowOpenFileDialog(Theme.ExtensionName, Theme.Extension);
            if (path == "") return;
            Theme theme = (Theme)solution.AddItem(path);
            if (theme == null) return;
            LoadTheme(theme);
            RealoadProjectTree();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (SelectedDocument != null) CloseTabIfOpened(SelectedDocument);
            if (solution.GetDocumentsCount() > 0)
            {
                if (!solution.CloseAll()) e.Cancel = true;
            }
        }

        private void addNewThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project project = solution.GetProject();
            if (project == null)
            {
                MessageBox.Show("You must create or open project previously.", "Error", MessageBoxButtons.OK);
                return;
            }
            Theme theme = CreateTheme();
            Document document = new Document(theme, project);
            solution.SaveDocument(document);
            solution.AddDocument(document);
            RealoadProjectTree();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solution.SaveAll();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTabIfOpened(SelectedDocument);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (SelectedDocument != null) CloseTabIfOpened(SelectedDocument);
        }

        private void Project_treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node as TreeNode;
            object item = node.Tag;
            if (item == null) return;
            if (item.GetType() == typeof(Project))
            {

            }
            else if (item.GetType() == typeof(Theme))
            {
                SelectedDocument = (IDocument)item;
            }
            else if(item.GetType().GetInterfaces().Contains(typeof(LB_Tools_Interface)))
            {
                Theme theme = node.Parent.Tag as Theme;
                if(theme!=null && node.Tag != null)
                {
                    if (theme.GetThemeCreator()!=null)
                    theme.GetThemeCreator().SelectTool((LB_Tools_Interface) node.Tag);
                }
            }
            else if(item.GetType() == typeof(TaskScheduler))
            {
                SelectedDocument = item;
            }
            OpenSelectedDocumentInTab();
            node.Expand();
        }
        private void OpenItemInTab(object item)
        {
            if (item == null) return;
            if (item.GetType() == typeof(Theme))
            {
                LoadTheme((Theme)item);
            }
            else if(item.GetType() == typeof(TaskScheduler))
            {
                //TreeNode project_node = Project_treeView.SelectedNode.Parent;
               // if (project_node != null && project_node.Tag != null)
                {
                    TaskScheduler taskScheduler = item as TaskScheduler;
                    taskScheduler.SetSolution(solution);
                    if (taskScheduler != null) taskScheduler.Show(tabControl_Main);
                }
            }
        }
        private void OpenSelectedDocumentInTab()
        {
            if (SelectedDocument == null) return;
            bool InTab = false;
            foreach (TabPage tabpage in tabControl_Main.TabPages)
            {
                if (tabpage.Tag == SelectedDocument)
                {
                    InTab = true;
                    tabControl_Main.SelectedTab = tabpage;
                    break;
                }
            }
            if (!InTab)
            {
                OpenItemInTab(SelectedDocument);
            }
        }
        private void CloseTabIfOpened(object item)
        {
            if (item == null) return;
            if(item.GetType() == typeof(Project))
            {
                foreach(IDocument project_item in solution.GetProjectItems((Project)item))
                {
                    CloseTabIfOpened(project_item);
                }
                foreach(TabPage tabpage in tabControl_Main.TabPages)
                {
                    if(tabpage.Tag.GetType() == typeof(TaskScheduler) )
                    {
                        TaskScheduler ts = tabpage.Tag as TaskScheduler;
                        if (ts.GetSolution() == solution)
                        {
                            tabControl_Main.TabPages.Remove(tabpage);
                        }
                    }
                }
            }
                else foreach(TabPage tabpage in tabControl_Main.TabPages)
            {
                if (tabpage.Tag == item)
                {
                    tabpage.Dispose();
                    tabControl_Main.TabPages.Remove(tabpage);
                    break;                   
                }
            }
            
        }

        private void Project_treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Node.Tag == null) return;
                e.Node.ContextMenuStrip = treeView_contextMenuStrip;
                treeView_contextMenuStrip.Tag = e.Node.Tag;
                treeView_Context_Down.Visible = false;
                treeView_Context_Up.Visible = false;
                treeView_Context_Close.Visible = false;
                treeView_Context_Delete.Visible = true;
                if (e.Node.Tag.GetType() == typeof(Theme) || e.Node.Tag.GetType() == typeof(Project))
                {
                    if (e.Node.Tag.GetType() == typeof(Project))
                    {
                        treeView_Context_Close.Visible = true;
                        treeView_Context_Delete.Visible = false;
                    }
                    Action T_Delete = () =>
                    {
                        IDocument item = (IDocument)e.Node.Tag;
                        if (item != null)
                        {
                            CloseTabIfOpened(item);
                            solution.CloseDocumentWithQuestion(solution.GetItemDocument(item));
                        }
                    };
                    treeView_Context_Delete.Tag = T_Delete;
                    treeView_Context_Close.Tag = T_Delete;
                    treeView_contextMenuStrip.Show();
                }
                else if (e.Node.Tag.GetType().GetInterfaces().Contains(typeof(LB_Tools_Interface)))
                {
                    
                    
                    Action T_UP = () => {
                        Theme theme = (Theme)e.Node.Parent.Tag;
                        if (theme != null) theme.ToolUp((LB_Tools_Interface)e.Node.Tag);
                    };
                    treeView_Context_Up.Tag = T_UP;

                    Action T_DOWN = () =>
                    {
                        Theme theme = (Theme)e.Node.Parent.Tag;
                        if (theme != null) theme.ToolDown((LB_Tools_Interface)e.Node.Tag);
                    };
                    treeView_Context_Down.Tag = T_DOWN;
                    Action T_Delete = () =>
                        {
                            Theme theme = (Theme)e.Node.Parent.Tag;
                            if (theme != null) theme.RemoveTool((LB_Tools_Interface)e.Node.Tag);
                        };
                    treeView_Context_Delete.Tag = T_Delete;

                    treeView_Context_Down.Visible = true;
                    treeView_Context_Up.Visible = true;
                    treeView_contextMenuStrip.Show();
                }
            }
        }
        private void treeView_Context_Delete_Click(object sender,EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            if (menu_item.Tag != null)
            {
                Action t_del = (Action)menu_item.Tag;
                t_del();
                RealoadProjectTree();
            }
        }
        private void treeView_Context_Rename_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            if (menu_item.Owner.Tag != null)
            {
                //treeView_Context_NewName.Visible = true;
                IDocument item = menu_item.Owner.Tag as IDocument;
               // solution.RenameDocument(solution.GetItemDocument(item),);
                RealoadProjectTree();
            }
        }
        private void treeView_Context_Up_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            if (menu_item.Tag != null)
            {
                Action t_up =(Action) menu_item.Tag;
                t_up();
                RealoadProjectTree();
            }
        }
        private void treeView_Context_Down_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            if (menu_item.Tag != null)
            {
                Action t_down = (Action)menu_item.Tag;
                t_down();
                RealoadProjectTree();
            }
        }

        private void tabControl_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_Main.SelectedTab != null)
            {
                SelectedDocument = tabControl_Main.SelectedTab.Tag;
            }
            else SelectedDocument = null;
        }

        private void uploadToLEDBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solution.SaveAll();
            lb_manager = new LED_Board_Manager();
            lb_manager.Size = new Size(425,180);

            Project project = solution.GetProject();
            if (project == null) return;
            string prject_file_path = solution.GetItemPath(project);
            string project_folder_path = Path.GetDirectoryName(prject_file_path) + "\\";        
            lb_manager.AddFile(prject_file_path);
            foreach (IDocument item in solution.GetProjectItems(project))
            {
                Document document = solution.GetItemDocument(item);
                string path = document.GetPath();
                lb_manager.AddFile(path);
                if(item.GetType() == typeof(Theme))
                {
                    Theme theme = item as Theme;
                    foreach(LB_Tools_Interface tool in theme.GetTools())
                    {
                        string tool_res;
                        
                        if(tool.GetType() == typeof(LB_Tools_Text))
                        {
                            LB_Tools_Text tool_text = tool as LB_Tools_Text;
                            tool_res = project_folder_path + Project.AnimationsDirectory + "LB_Tools_Text_" + tool_text.Text_Name + ".bma";
                            tool_text.SetLBResourcePath(tool_res);
                            AnimationConverter animationConverter = new AnimationConverter(tool_text.GetBitmap());
                            if (animationConverter.Encode(tool_res, 0, 0, AnimationConverter.ColorOrder.RGB, AnimationConverter.PixelOrder.VS_TR, AnimationConverter.DataType.Default))
                            {
                                lb_manager.AddFile(tool_res);
                            }
                        }
                        else if(tool.GetType() == typeof(LB_Tools_RunningText))
                        {
                            LB_Tools_RunningText tool_rtext = tool as LB_Tools_RunningText;
                            tool_res = project_folder_path + Project.AnimationsDirectory + "LB_Tools_Text_" + tool_rtext.Text_Name + ".bma";
                            CloseTabIfOpened(theme);
                            if(tool_rtext.SaveAsBMA(tool_res))
                            {
                                lb_manager.AddFile(tool_res);
                            }
                        }
                        else if(tool.GetType() == typeof(LB_Tools_Animation))
                        {
                            string animations_folder = project_folder_path + Project.AnimationsDirectory;
                            if (!Directory.Exists(animations_folder))
                            {
                                Directory.CreateDirectory(animations_folder);
                            }
                            string animation_file_path = tool.GetLBResourcePath();
                            if (animations_folder != Path.GetDirectoryName(animation_file_path)){
                                File.Copy(animation_file_path, animations_folder + Path.GetFileName(animation_file_path), true);
                                animation_file_path = animations_folder + Path.GetFileName(animation_file_path);
                                lb_manager.AddFile(animation_file_path);
                            }
                        }
                        else
                        {
                            tool_res = tool.GetLBResourcePath();
                            if (tool_res != "") lb_manager.AddFile(tool_res);
                        }
                        
                    }
                }
            }

            
            lb_manager.Show();
        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

      
    }
        
}
