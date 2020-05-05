using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LED_Board_Access
{
  public class Document
    {
      private string FilePath;
      private Project project;
      private bool ItemChanged;
      private bool ItemSaved;
      private IDocument item;      
      public Document(IDocument item,Project project,string FilePath)
      {
          ItemSaved = false;
          ItemChanged = true;
          this.item = item;
          this.project = project;
          this.FilePath = FilePath;
      }
      public Document(IDocument item, Project project)
      {
          ItemSaved = false;
          ItemChanged = true;
          this.item = item;
          this.project = project;
          this.FilePath = "";
      }
      public Document(IDocument item)
      {
          ItemSaved = false;
          ItemChanged = true;
          project = null;
          FilePath = "";
          this.item = item;
      }
      public bool CompareItems(IDocument item)
      {
          return this.item.Equals(item);
      }

      public Project GetProject()
      {
          return project;
      }
      public string GetName()
      {
          return Path.GetFileName(GetPath());
      }
      public string Rename(string name)
      {
          if (FilePath != "")
          {
              //string old_name = Path.GetFileNameWithoutExtension(FilePath);
              FilePath = Path.GetDirectoryName(FilePath) + "\\" + name + item.GetExtension();
          }
          return FilePath;
      }
      public string GetPath()
      {
          return FilePath;
      }
      public string GetDirectory()
      {
          return Path.GetDirectoryName(FilePath) + "\\";
      }
      public void SetPath(string path)
      {
          FilePath = path;
      }
      public IDocument GetItem()
      {
          return item;
      }
      public void Saved()
      {
          ItemSaved = true;
          ItemChanged=false;
      }
      public void Changed()
      {
          ItemChanged = true;
          ItemSaved = false;
      }
      public bool IsChanged()
      {
          return ItemChanged;
      }
      public bool IsSaved()
      {
          return ItemSaved;
      }
    }
}

