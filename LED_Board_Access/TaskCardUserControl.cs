using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace LED_Board_Access
{
    public partial class TaskCardUserControl : UserControl
    {
        private LB_Task task;
        public TaskCardUserControl()
        {
            InitializeComponent();
        }
        public TaskCardUserControl(LB_Task task)
        {
            this.task = task;
            InitializeComponent();
        }
        public LB_Task GetTask()
        {
            return task;
        }
        public void SetTask(LB_Task task)
        {
            this.task = task;
            this.Invalidate();
        }
        private void button_redo_MouseLeave(object sender, EventArgs e)
        {
            button_redo.BackgroundImage = Properties.Resources.Pen_1;
        }
        public void PenSelect()
        {
            button_redo.BackgroundImage = Properties.Resources.Pen_1_yellow;
        }
        public void PedDeselect()
        {
            button_redo.BackgroundImage = Properties.Resources.Pen_1;
        }

        private void button_redo_MouseEnter(object sender, EventArgs e)
        {
            button_redo.BackgroundImage = Properties.Resources.Pen_1_red;
        }

        private void button_close_MouseEnter(object sender, EventArgs e)
        {
            button_close.BackgroundImage = Properties.Resources.Close_button_red;
        }

        private void button_close_MouseLeave(object sender, EventArgs e)
        {
            button_close.BackgroundImage = Properties.Resources.Close_button_2;
        }

        private void TaskCardUserControl_Paint(object sender, PaintEventArgs e)
        {
            label_time.Text = task.TimeFrom.hour.ToString("d2") + ":" + task.TimeFrom.minute.ToString("d2") + " - " + task.TimeTo.hour.ToString("d2") + ":" + task.TimeTo.minute.ToString("d2");
            label_theme_name.Text = task.ThemeName;
        }
    }
}
