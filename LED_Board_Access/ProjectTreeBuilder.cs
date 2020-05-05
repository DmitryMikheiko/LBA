using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.IO;

namespace LED_Board_Access
{
public class ProjectTreeBuilder
    {
      private Solution solution;
      TreeView tree;
      ImageList images;
      public ProjectTreeBuilder(Solution solution,TreeView tree)
      {
          this.solution = solution;
          this.tree = tree;
          images = new ImageList();
          images.Images.Add(Properties.Resources.red_circle_1);
          images.Images.Add(Properties.Resources.green_circle_1);
          images.Images.Add(Properties.Resources.folder);
          images.Images.Add(Properties.Resources.task_2);
          tree.ImageList = images;
          tree.ImageIndex = 0;
          tree.SelectedImageIndex = 1;
      }
      public void ReloadTree()
      {
          if (tree == null || solution == null) return;
          tree.Nodes.Clear();
          LoadTree();
      }
      public void LoadTree()
      {
         /* foreach(Project project in solution.GetProjects())
          {
              TreeNode project_node = new TreeNode(project.ProjectName);
              TreeNode themes_node = new TreeNode("Themes");
              tree.Nodes.Add(project_node);
              project_node.Nodes.Add(themes_node);
              foreach(Theme theme in solution.GetProjectThemes(project))
              {
                  themes_node.Nodes.Add(new TreeNode(theme.Name));
              }
          }*/
          TreeNode theme_node,img_node,an_node,tasks_node;
          img_node = new TreeNode("Images");
          img_node.Name = "Images";
          img_node.ImageIndex = 2;
          img_node.SelectedImageIndex = 2;
          img_node.NodeFont = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);

          an_node = new TreeNode("Animations");
          an_node.Name = "Animations";
          an_node.ImageIndex = 2;
          an_node.SelectedImageIndex = 2;
          an_node.NodeFont = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
          foreach(Document document in solution.GetDocuments())
          {
              IDocument item = document.GetItem();
              Type item_type = item.GetType();
              TreeNode node = new TreeNode(item.GetName());
              node.Name = item.GetName();
              node.Tag = item;

              theme_node = new TreeNode("Themes");
              theme_node.Name = "Themes";
              theme_node.ImageIndex = 2;
              theme_node.SelectedImageIndex = 2;
              theme_node.NodeFont = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            
              if(item_type == typeof(Project))
              {
                  if (!tree.Nodes.Contains(node))
                  {
                      //node.NodeFont = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);                     
                      tree.Nodes.Add(node);
                      node.Nodes.AddRange(new TreeNode[] {theme_node, img_node, an_node });
                  }
                  TreeNode[] project_node = tree.Nodes.Find(item.GetName(), false);
                      tasks_node = new TreeNode("Tasks");
                      tasks_node.Name = "Tasks";
                      tasks_node.ImageIndex = 3;
                      tasks_node.SelectedImageIndex = 3;
                      tasks_node.Tag = ((Project)item).GetTaskScheduler();
                     if(project_node.Length != 0) project_node[0].Nodes.Add(tasks_node);
                  
              }
              else if(item_type == typeof(Theme))
              {
                  GetResourceNodes((Theme)item, img_node, an_node);
                  Project project = solution.GetItemProject(item);
                  if(project==null)
                  {
                      tree.Nodes.Add(GetThemeNode((Theme) item));
                  }
                  else
                  {
                      TreeNode[] project_node = tree.Nodes.Find(project.GetName(), false);
                      if (project_node.Length == 0) 
                      {
                          project_node = new TreeNode[1];
                          project_node[0] = new TreeNode(project.GetName());
                          project_node[0].Name = project.GetName();
                          tree.Nodes.Add(project_node[0]);
                          project_node[0].Nodes.Add(theme_node);
                          project_node[0].Nodes.AddRange(new TreeNode[] { theme_node, img_node, an_node });
                      }
                      theme_node = project_node[0].Nodes.Find("Themes",false)[0];
                      theme_node.Nodes.Add(GetThemeNode((Theme)item));
                  }
              }
          }

          tree.ExpandAll();
      }

    private TreeNode GetThemeNode(Theme theme)
      {
          TreeNode node = new TreeNode(theme.GetName());
          node.Name = theme.GetName();
          node.Tag = theme;
          TreeNode tool_node;
          foreach(LB_Tools_Interface tool in theme.Tools.Reverse<LB_Tools_Interface>())
          {
              tool_node = new TreeNode(tool.GetToolName());
              tool_node.Name = tool.GetToolName();
              tool_node.Tag = tool;
              node.Nodes.Add(tool_node);

          }
          return node;
      }
    private void GetResourceNodes(Theme theme,TreeNode img_node,TreeNode an_node)
    {
        TreeNode tool_node;
        foreach (LB_Tools_Interface tool in theme.Tools.Reverse<LB_Tools_Interface>())
        {
            if(tool.GetResourcePath()!="")

            switch(Path.GetExtension(tool.GetResourcePath()))
            {
                case ".bmp":
                    tool_node = new TreeNode(Path.GetFileName(tool.GetResourcePath()));
                    tool_node.Name = tool_node.Text;
                    tool_node.Tag = tool;
                    img_node.Nodes.Add(tool_node);
                break;
                case ".jpg":
                    tool_node = new TreeNode(Path.GetFileName(tool.GetResourcePath()));
                    tool_node.Name = tool_node.Text;
                    tool_node.Tag = tool;
                    img_node.Nodes.Add(tool_node);
                break;
                case ".jpeg":
                    tool_node = new TreeNode(Path.GetFileName(tool.GetResourcePath()));
                    tool_node.Name = tool_node.Text;
                    tool_node.Tag = tool;
                    img_node.Nodes.Add(tool_node);
                break;
                case ".bma":
                    tool_node = new TreeNode(Path.GetFileName(tool.GetResourcePath()));
                    tool_node.Name = tool_node.Text;
                    tool_node.Tag = tool;
                    an_node.Nodes.Add(tool_node);
                break;

                default: break;
            }

        }

    }
     
    }
}
