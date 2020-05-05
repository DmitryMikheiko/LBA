using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
         V1_23x23,
         V2_24x24
     }
     public BoardType Board;
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
    }
}
