using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LED_Board_Access
{
    public partial class AddTaskDialog : Form
    {
        LB_Task new_task;
        List<string> ThemeNames;
        List<LB_Task> Tasks;
        DayOfWeek day;
        public AddTaskDialog(List<string> ThemeNames,List<LB_Task> tasks,DayOfWeek day)
        {
            InitializeComponent();
            this.ThemeNames = ThemeNames;
            this.Tasks = tasks;
            this.day = day;
            new_task = new LB_Task();
            comboBox_themes.Items.Clear();
            foreach (string s in ThemeNames) comboBox_themes.Items.Add(s);
            if(comboBox_themes.Items.Count>0) comboBox_themes.SelectedIndex = 0;
            foreach(CheckBox cb in groupBox_Days.Controls)
            {
                if (day == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), cb.Text))
                {
                    cb.Checked = true;
                }
                else cb.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ThemeSetError = false;
            foreach (CheckBox cb in groupBox_Days.Controls)
            {
                if (!cb.Checked) continue;
                DayOfWeek s_day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), cb.Text);
                new_task = new LB_Task();
                new_task.SetDay(s_day);
                new_task.ThemeName = comboBox_themes.Text;
                if (!ConvertTime())
                {
                    maskedTextBox_time.BackColor = System.Drawing.Color.Red;
                    ThemeSetError = true;
                    MessageBox.Show("Can't set this theme on " + s_day.ToString()+". Change the time range please", "Error", MessageBoxButtons.OK);
                }
                else
                {
                    cb.Checked = false;
                    Tasks.Add(new_task);
                }          
            }
            if(!ThemeSetError) DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void AddTaskDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        public LB_Task GetTask()
        {
            return new_task;
        }
        private bool ConvertTime()
        {
            int hours_f ,hours_t,minutes_f,minutes_t;
            string[] s = maskedTextBox_time.Text.Split('-');
            if (s.Length != 2) return false;
            
                string[] s_f = s[0].Split(':');
                if (s_f.Length != 2) return false;
                if (!Int32.TryParse(s_f[0], out hours_f)) return false;
                if (!Int32.TryParse(s_f[1], out minutes_f)) return false;

                string[] s_t = s[1].Split(':');
                if (s_t.Length != 2) return false;
                if (!Int32.TryParse(s_t[0], out hours_t)) return false;
                if (!Int32.TryParse(s_t[1], out minutes_t)) return false;
                if (hours_f > 23 || hours_t > 23 || minutes_f > 59 || minutes_t > 59) return false;
                new_task.SetTimeFrom(hours_f, minutes_f);
                new_task.SetTimeTo(hours_t, minutes_t);

                return CheckTaskTime();
        }
        private bool CheckTaskTime()
        {
            return TaskTimeIsRight(new_task);
        }
        private bool TaskTimeIsRight(LB_Task task)
        {

            int time_f = task.TimeFrom.hour * 60 + task.TimeFrom.minute;
            int time_t = task.TimeTo.hour * 60 + task.TimeTo.minute;
            if (time_f >= time_t) return false;
/*
            int time_from,time_to;
            foreach(LB_Task x in Tasks)
            {
                if(task.TimeFrom.day == task.TimeFrom.day ) //???
                {
                    time_from = x.TimeFrom.hour * 60 + x.TimeFrom.minute;
                    time_to  = x.TimeTo.hour * 60 + x.TimeTo.minute;
                    return !((time_f >= time_from && time_f < time_to) || (time_t > time_from && time_t< time_to)) ;
                }
            }*/
            return true;
        }

        private void maskedTextBox_time_TextChanged(object sender, EventArgs e)
        {
            maskedTextBox_time.BackColor = System.Drawing.Color.White;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
