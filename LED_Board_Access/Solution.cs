using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LED_Board_Access
{
    public class Solution
    {
        private List<Document> documents;
        private const int DocumentsDefaultSize = 30;

        public Solution()
        {
            documents = new List<Document>(DocumentsDefaultSize);
        }
        public void AddDocument(Document document)
        {
            documents.Add(document);
        }
        public bool RemoveDocument(Document document,bool Erase)
        {
            documents.Remove(document);
            if(Erase)
            {
                string path = document.GetPath();
                if(!File.Exists(path)) return false;
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return true;
        }
        public List<Document> GetDocuments()
        {
            return documents; // new List<Document>(documents);
        }
        public int GetDocumentsCount()
        {
            return documents.Count;
        }
        public void SetPath(IDocument item,string path)
        {
            Document document = GetItemDocument(item);
            if(document!=null) document.SetPath(path);
        }
        public IDocument AddItem(string path)
        {
            IDocument item=(IDocument)LoadObjectFromFile(path);
            if(item!=null)
            {       
                AddItem(item);
                SetPath(item, path);
            }
            return item;
        }
        public void AddItem(IDocument item)
        {
            Document document = new Document(item);
            AddDocument(document);
        }
        public IDocument AddItem(string path,Project project)
        {
            if(project == null || !File.Exists(path)) return null;
            Document project_document = GetItemDocument(project);
            if (project_document == null) return null;
            string path2 = project_document.GetDirectory() + project.GetItemPath(Path.GetExtension(path)) + Path.GetFileName(path);
            if (path != path2)
            {
                bool overwrite = false;
                if (File.Exists(path2))
                {
                    if (MessageBox.Show("File " + Path.GetFileName(path) + " exists in project directory.To overwrite it?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        overwrite = true;
                }
                DirectoryInfo dir_info = new DirectoryInfo(Path.GetDirectoryName(path2));
                if (!dir_info.Exists) dir_info.Create();
                try
                {
                    File.Copy(path, path2, overwrite);
                }
                catch
                {
                    MessageBox.Show("Can't add file to project!", "Error");
                    return null;
                }
            }
            IDocument item =(IDocument) LoadObjectFromFile(path2);
            if (item != null)
            {
                AddItem(item,project);
                SetPath(item, path2);
               // project.AddDocumentName(path2);
            }
            return item;
        }
        public void AddItem(IDocument item, Project project)
        {
            Document project_document = GetItemDocument(project);
            if (project_document == null) return;
            string path = project_document.GetDirectory();
            Document document = new Document(item, project);
            AddDocument(document);
            //project.AddDocumentName(document);
        }
        public Project GetItemProject(IDocument item)
        {
            Document document = GetItemDocument(item);
            return document.GetProject();
        }
        public Project GetProject()
        {
            Document project_document = (Document) documents.Find(x => x.GetItem().GetType() == typeof(Project));
            if (project_document == null) return null;
            return (Project)project_document.GetItem();
        }
        public List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();
            List<Document> project_documents = documents.FindAll(x => x.GetItem().GetType() == typeof(Project));
            if (project_documents == null || project_documents.Count == 0) return projects;
            foreach(Document document in project_documents)
            {
                projects.Add((Project)document.GetItem());
            }
            return projects;
        }
        public List<Theme> GetProjectThemes(Project project)
        {
            List<Theme> themes = new List<Theme>();
            foreach(Document document in documents)
            {
                if (document.GetProject() == project && document.GetItem().GetType() == typeof(Theme)) themes.Add((Theme)document.GetItem());
            }
            return themes;
        }
        public void RemoveItem(IDocument item)
        {
            Document document = GetItemDocument(item);
            RemoveDocument(document, false);
        }
        public void RemoveItem(IDocument item, bool Erase)
        {
            Document document = GetItemDocument(item);
            RemoveDocument(document,Erase);
        }

        public Document GetItemDocument(IDocument item)
        {
            if (item == null) return null;
            Document document = documents.Find(x => x.CompareItems(item));
            return document;
        }
        public string GetItemPath(IDocument item)
        {
            Document document = GetItemDocument(item);
            if (document != null) return document.GetPath();
            else return "";
        }
        public void SetItemPath(IDocument item, string path)
        {
            Document document = GetItemDocument(item);
            if (document == null || path == "") return;
           // if(document.GetProject()==null)
            {
                document.SetPath(path);
                return;
            }
            
        }
        public void SetDocumentPath(Document document,string path)
        {
            if (document == null) return;
            document.SetPath(path);
        }
        public void RenameDocument(Document document,string name)
        {
            string path = document.GetPath(); // save old path
            if(path=="" || !File.Exists(path)) return;
            document.Rename(name);
            if(!File.Exists(document.GetPath()))
            {
                File.Move(path, document.GetPath());
                document.GetItem().Rename(name);
            }
            else
            {
                document.SetPath(path);
            }                        
        }
        public string SaveItem(IDocument item)
        {
            return SaveDocument(GetItemDocument(item));
        }
        public string SaveItemAs(IDocument item)
        {
            return SaveDocumentAs(GetItemDocument(item));
        }
        public string SaveDocumentAs(Document document)
        {
            if (document != null)
            {
                IDocument item = document.GetItem();
                string path = ShowSaveFileDialog(item.GetExtensionName(), item.GetExtension());
                if (path != "")
                {
                    item.Rename(Path.GetFileNameWithoutExtension(path));
                    SaveObjectToFile(item, path);
                    if (document.GetPath() == "") document.SetPath(path);
                    else
                    {

                    }
                    return path;
                }
            }
            return ""; //user message
        }
        public string SaveDocument(Document document)
        {
            if (document != null)
            {
                Project project;
                IDocument item = document.GetItem();
                string path = document.GetPath();

                if(document.GetItem().GetType() == typeof(Project))
                {
                    project = (Project) document.GetItem();
                    project.ClearDocumentNames();
                    project.AddDocumentNames(GetProjectFileNames(project));
                }
                if (path == "")
                {
                    if(document.GetProject()==null)  path = ShowSaveFileDialog(item.GetExtensionName(), item.GetExtension());
                    else
                    {
                        project = document.GetProject();
                        Document project_document = GetItemDocument(project);
                        path = project_document.GetDirectory() + project.GetItemPath(item.GetExtension()) + item.GetName() + item.GetExtension();                        
                    }
                }
                if (path != "")
                {
                    item.Rename(Path.GetFileNameWithoutExtension(path));
                    SaveObjectToFile(item, path);
                    SetDocumentPath(document, path);
                   // if (document.GetProject() != null) SaveDocument(GetItemDocument(document.GetProject()));//!!! save project also, because the name of file might have changed 
                    return path;
                }
            }
            return "";//user message
        }
        public void SaveAll()
        {
            foreach(Document document in documents)
            {
                SaveDocument(document);
            }
        }
        public void CloseDocument(Document document,bool Save)
        {
            if(Save)SaveDocument(document);
            if(document.GetItem().GetType() == typeof(Project))
            {
                if (Save)
                {
                    List<Document> project_documents = documents.FindAll(x => x.GetProject() == document.GetItem());
                    foreach (Document project_document in project_documents)
                    {
                        SaveDocument(project_document);
                    }
                }
                documents.RemoveAll(x => x.GetProject() == document.GetItem());
            }
            RemoveDocument(document,false);
            document = null;
        }
        public bool CloseDocumentWithQuestion(Document document)
        {
            DialogResult result = MessageBox.Show("Do you want to SAVE files?", "Warning", MessageBoxButtons.YesNoCancel);
            switch(result)
            {
                case DialogResult.Yes:
                    CloseDocument(document, true);
                    return true;
                case DialogResult.No:
                    CloseDocument(document, false);
                    return true;
                case DialogResult.Cancel:
                    return false;
                default: return false;
            }
        }
        public bool CloseAll()
        {
            DialogResult result = MessageBox.Show("Do you want to SAVE files?", "Warning", MessageBoxButtons.YesNoCancel);
            bool Save;
            switch (result)
            {
                case DialogResult.Yes:
                    Save = true;
                    break;
                case DialogResult.No:
                    Save = false;
                    break;
                case DialogResult.Cancel:
                    return false;
                default: return false;
            }
            if(Save)
            foreach(Document document in documents)
            {
                SaveDocument(document);
            }
            documents.Clear();
            return true;
        }
        public List<String> GetProjectFileNames(Project project)
        {
            List<string> names = new List<string>();
            foreach(Document document in documents.FindAll(x =>x.GetProject() == project))
            {
                names.Add(document.GetName());
            }
            return names;
        }
        public List<IDocument> GetProjectItems(Project project)
        {
            List<IDocument> items = new List<IDocument>();
            foreach(Document document in documents.FindAll(x => x.GetProject() == project))
            {
                items.Add(document.GetItem());
            }
            return items;
        }
        public List<string> GetProjectThemeNames(Project project)
        {
            List<string> names = new List<string>();
            foreach (Document document in documents.FindAll(x => x.GetProject() == project && x.GetItem().GetType() == typeof(Theme)))
            {
                names.Add(document.GetName());
            }
            return names;

        }
        private string ShowSaveFileDialog(string FileExtensionName,string FileExtension)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = FileExtensionName+" (*"+FileExtension+")|*"+FileExtension;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else return "";
        }
        private void SaveObjectToFile(object obj,string path)
        {
            if (path == "") return;
            string serialized = JsonConvert.SerializeObject(obj);
          
            System.IO.File.WriteAllText(path, serialized);
        }
        private object LoadObjectFromFile(string path)
        {
            if(File.Exists(path))
            {
                string value = File.ReadAllText(path);
                switch(Path.GetExtension(path))
                {
                    case Theme.Extension:
                        return JsonConvert.DeserializeObject<Theme>(value,new ToolsConverter());
                        
                    case Project.Extension:
                        return JsonConvert.DeserializeObject<Project>(value);
                    default:
                        return null;
                }
            }
            return null;
        }
    }
    public class ToolsConverter : JsonCreationConverter<LB_Tools_Interface>
    {
        protected override LB_Tools_Interface Create(Type objectType, JObject jObject)
        {
            if (FieldExists("LB_Tools_Animation_Name", jObject))
            {
                return new LB_Tools_Animation();
            }
            else if (FieldExists("LB_Tools_Clock_Name", jObject))
            {
                return new LB_Tools_Clock();
            }
            else if (FieldExists("LB_Tools_Image_Name", jObject))
            {
                return new LB_Tools_Image();
            }
            else if (FieldExists("LB_Tools_RunningText_Name", jObject))
            {
                return new LB_Tools_RunningText();
            }
            else if (FieldExists("LB_Tools_Text_Name", jObject))
            {
                return new LB_Tools_Text();
            }
            else if (FieldExists("LB_Tools_TimeSecProgressBar_Name", jObject))
            {
                return new LB_Tools_TimeSecProgressBar();
            }
            else return null;
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            bool s = typeof(T).IsAssignableFrom(objectType);
            return s;
        }

        public override bool CanWrite
        {
            get { return false; }
        }
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {

        }
        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
