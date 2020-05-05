using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LED_Board_Access
{
    public partial class TaskSchedulerUserControl : UserControl
    {
        public TaskSchedulerUserControl()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addTask_panel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackgroundImage = Properties.Resources.add_green;
        }

        private void addTask_panel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackgroundImage = Properties.Resources.add_blue;
        }
    }
}
