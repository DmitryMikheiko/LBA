using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LED_Board_Access
{
 public  class TaskScheduler //: IDocument
    {

     private TaskSchedulerUserControl taskSchedulerControl;
     private TabPage tabpage;
     public List<LB_Task> lb_tasks;
     private AddTaskDialog addTaskDialog;
     private Project project;
     private Solution solution;
     public enum DayOfTheWeek
     {
         Monday,
         Tuesday,
         Wednesday,
         Thursday,
         Friday,
         Saturday,
         Sunday
     }
     public struct LB_DateTime
     {
         public DayOfWeek day;
         public int hour;
         public int minute;
     }
     public TaskScheduler(Project project)
     {
         this.project = project;
         lb_tasks = new List<LB_Task>();
                
     }
     public void SetSolution(Solution solution)
     {
         this.solution = solution;
     }
     public Project GetProjet()
     {
         return project;
     }
     public Solution GetSolution()
     {
         return solution;
     }
     public void Show(TabControl control)
     {
         tabpage = new TabPage("Task Scheduler");
         tabpage.Tag = this;
         taskSchedulerControl = new TaskSchedulerUserControl();
         taskSchedulerControl.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
         taskSchedulerControl.addTask_Monday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Tuesday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Wednesday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Thursday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Friday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Saturday_panel.DoubleClick += new EventHandler(AddTaskButtonClick);
         taskSchedulerControl.addTask_Sunday_panel.DoubleClick += new EventHandler(AddTaskButtonClick); 
         tabpage.Controls.Add(taskSchedulerControl);

         control.TabPages.Add(tabpage);
         control.SelectedTab = tabpage;
         LoadTaskCards();
     }
     private void LoadTaskCards()
     {
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Monday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Monday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Tuesday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Tuesday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Wednesday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Wednesday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Thursday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Thursday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Friday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Friday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Saturday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Saturday.Controls.Remove(c);
             }
             foreach (Control c in taskSchedulerControl.flowLayoutPanel_Sunday.Controls)
             {
                 if (c.GetType() == typeof(TaskCardUserControl)) taskSchedulerControl.flowLayoutPanel_Sunday.Controls.Remove(c);
             } 

         lb_tasks.Sort(delegate(LB_Task a, LB_Task b)
         {
             return (a.TimeFrom.hour * 60 + a.TimeFrom.minute) - (b.TimeFrom.hour * 60 + b.TimeFrom.minute);
         });
         foreach (LB_Task task in lb_tasks)
         {
             TaskCardUserControl card = new TaskCardUserControl(task);
             card.button_close.Click += new EventHandler(DeleteTaskButtonClick);
             switch (task.TimeFrom.day)
             {
                 case DayOfWeek.Monday:
                     taskSchedulerControl.flowLayoutPanel_Monday.Controls.Add(card);
                     break;
                 case DayOfWeek.Tuesday:
                     taskSchedulerControl.flowLayoutPanel_Tuesday.Controls.Add(card);
                     break;
                 case DayOfWeek.Wednesday:
                     taskSchedulerControl.flowLayoutPanel_Wednesday.Controls.Add(card);
                     break;
                 case DayOfWeek.Thursday:
                     taskSchedulerControl.flowLayoutPanel_Thursday.Controls.Add(card);
                     break;
                 case DayOfWeek.Friday:
                     taskSchedulerControl.flowLayoutPanel_Friday.Controls.Add(card);
                     break;
                 case DayOfWeek.Saturday:
                     taskSchedulerControl.flowLayoutPanel_Saturday.Controls.Add(card);
                     break;
                 case DayOfWeek.Sunday:
                     taskSchedulerControl.flowLayoutPanel_Sunday.Controls.Add(card);
                     break;
                 default: break;
             }
         }
     }
     public void AddTask(LB_Task task)
     {
         lb_tasks.Add(task);
     }
     public void DeleteTask(LB_Task task)
     {
         lb_tasks.Remove(task);
     }
     public List<LB_Task> GetTasks()
     {
         return new List<LB_Task>(lb_tasks);
     }
     public bool AddTask(LB_DateTime TimeFrom, LB_DateTime TimeTo, Theme theme)
     {
         LB_Task task = new LB_Task(TimeFrom, TimeTo, theme);

         return true;
     }
    /* public bool DeleteTask(int task_id)
     {
         LB_Task task = lb_tasks.Find(x => x.TaskID == task_id);
         if (task == null) return false;
         return lb_tasks.Remove(task);
     }*/

     public void DeleteTaskButtonClick(object sender,EventArgs e)
     {
         Button b = sender as Button;
         TaskCardUserControl card = b.Parent as TaskCardUserControl;
         LB_Task task = card.GetTask();
         lb_tasks.Remove(task);

         switch (task.TimeFrom.day)
         {
             case DayOfWeek.Monday:
                 taskSchedulerControl.flowLayoutPanel_Monday.Controls.Remove(card);
                 break;
             case DayOfWeek.Tuesday:
                 taskSchedulerControl.flowLayoutPanel_Tuesday.Controls.Remove(card);
                 break;
             case DayOfWeek.Wednesday:
                 taskSchedulerControl.flowLayoutPanel_Wednesday.Controls.Remove(card);
                 break;
             case DayOfWeek.Thursday:
                 taskSchedulerControl.flowLayoutPanel_Thursday.Controls.Remove(card);
                 break;
             case DayOfWeek.Friday:
                 taskSchedulerControl.flowLayoutPanel_Friday.Controls.Remove(card);
                 break;
             case DayOfWeek.Saturday:
                 taskSchedulerControl.flowLayoutPanel_Saturday.Controls.Remove(card);
                 break;
             case DayOfWeek.Sunday:
                 taskSchedulerControl.flowLayoutPanel_Sunday.Controls.Remove(card);
                 break;
             default: break;
         }
     }
     public void AddTaskButtonClick(object sender,EventArgs e)
     {
         if(solution == null || project == null) return;
         Panel addPanel = sender as Panel;
         DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), (string)(addPanel.Tag), true);
        
         addTaskDialog = new AddTaskDialog(solution.GetProjectThemeNames(project), lb_tasks, day);
         if(addTaskDialog.ShowDialog() == DialogResult.OK)
         {
             LoadTaskCards();
             /*       
             TaskCardUserControl card = new TaskCardUserControl(addTaskDialog.GetTask());
             card.button_close.Click += new EventHandler(DeleteTaskButtonClick);
             switch (addTaskDialog.GetTask().TimeFrom.day)
             {
                 case DayOfWeek.Monday:
                     taskSchedulerControl.flowLayoutPanel_Monday.Controls.Add(card);
                     break;
                 case DayOfWeek.Tuesday:
                     taskSchedulerControl.flowLayoutPanel_Tuesday.Controls.Add(card);
                     break;
                 case DayOfWeek.Wednesday:
                     taskSchedulerControl.flowLayoutPanel_Wednesday.Controls.Add(card);
                     break;
                 case DayOfWeek.Thursday:
                     taskSchedulerControl.flowLayoutPanel_Thursday.Controls.Add(card);
                     break;
                 case DayOfWeek.Friday:
                     taskSchedulerControl.flowLayoutPanel_Friday.Controls.Add(card);
                     break;
                 case DayOfWeek.Saturday:
                     taskSchedulerControl.flowLayoutPanel_Saturday.Controls.Add(card);
                     break;
                 case DayOfWeek.Sunday:
                     taskSchedulerControl.flowLayoutPanel_Sunday.Controls.Add(card);
                     break;
                 default: break;
             }*/
         }

         
         
         /*switch((string)addPanel.Tag)
         {
             case "Monday":
                 time.day = DayOfWeek.Monday;
                 break;             
             case "Tuesday":
                 time.day = DayOfWeek.Tuesday;
                 Enum.
                 break;
             case "Wednesday":

                 break;
             case "Thursday":

                 break;
             case "Friday":

                 break;
             case "Saturday":

                 break;
             case "Sunday":

                 break;
             case "AllDays":

                 break;
             default: break;
         }*/
     }
    }
}
