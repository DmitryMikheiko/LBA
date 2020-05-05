using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace LED_Board_Access
{
    public class Project : IDocument
    {
    // public List<Theme> themes;
        public List<string> documents;
        public const string Extension = ".lbprj";
        public const string ExtensionName = "LED Board Project";
        public const string ThemesDirectory = "Themes\\";
        public const string AnimationsDirectory = "Animations\\";
        public const string ImagesDirectory = "Images\\";
        public const string OtherFilesDirectory = "Other\\";

        private List<string> project_directories;
        public TaskScheduler taskScheduler;
        public enum BoardType
        {
            LSB_24x24,
            LSB_53x28
        }
        private BoardType boardType;
        private Size boardSize;

     
        public string ProjectName;
     public Project()
     {
         documents = new List<string>();
         project_directories = new List<string>();         
         project_directories.Add(ThemesDirectory);
        // project_directories.Add(AnimationsDirectory);
        // project_directories.Add(ImagesDirectory);
        // project_directories.Add(OtherFilesDirectory);
         taskScheduler = new TaskScheduler(this);
         boardSize = new Size(0, 0);
        }
     public TaskScheduler GetTaskScheduler()
     {
         return taskScheduler;
     }
     public void AddDocumentName(Document document)
     {
         AddDocumentName(document.GetName());
     }
     public void AddDocumentName(string name)
     {
         if (name == "") return;
         name = Path.GetFileName(name);
         if (!documents.Exists(x => x == name)) documents.Add(name);
     }
     public void RemoveDocumentName(Document document)
     {
         RemoveDocumentName(document.GetName());
     }
     public void RemoveDocumentName(string name)
     {
         documents.Remove(name);
     }
     public void RenameDocumentName(Document document,string name)
     {
         documents.Remove(document.GetName());
         documents.Add(name);
     }
     public List<string> GetDocumentNames()
     {
         return documents;
     }
     public void ClearDocumentNames()
     {
         documents.Clear();
     }
     public void AddDocumentNames(List<string> names)
     {
         documents.AddRange(names);
     }
     public List<string> GetProjectDirectories()
     {
         return project_directories;
     }
     public string GetItemPath(string Extension)
     {
         switch(Extension)
         {
             case Theme.Extension:
                 return ThemesDirectory;
             default: return OtherFilesDirectory;
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
         return ProjectName;
     }
     public void Rename(string name)
     {
         ProjectName = name;
     }
        public void SetBoardType(BoardType type)
        {
            boardType = type;
            switch (boardType)
            {
                case BoardType.LSB_24x24:
                    boardSize.Width = 24;
                    boardSize.Height = 24;
                    break;
                case BoardType.LSB_53x28:
                    boardSize.Width = 53;
                    boardSize.Height = 28;
                    break;
                default:
                    boardSize.Width = 10;
                    boardSize.Height = 10;
                    break;
            }
        }
        public BoardType GetBoardType()
        {         
            return boardType;
        }

        public Size GetBoardSize()
        {
            return new Size(boardSize.Width, boardSize.Height);
        }
    }
}
