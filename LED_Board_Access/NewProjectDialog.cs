using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LED_Board_Access
{
    public partial class NewProjectDialog : Form
    {
        string path;
        private struct FieldsDef
        {
            public bool Name;
            public bool Location;
            public bool BoardVersion;
        }
        FieldsDef Fields;
        public NewProjectDialog()
        {
            InitializeComponent();
            comboBox_BoardVersion.Items.AddRange(Enum.GetNames(typeof(Project.BoardType)));
            comboBox_BoardVersion.SelectedItem = Project.BoardType.LSB_24x24;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            path += "\\" + textBox_Name.Text + "\\";
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Enable to create project directory");
                this.Close();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;            
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                this.textBox_Location.Text = path;
            }
        }
        private void textBox_Name_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Name.Text == "")
            {
                textBox_Name.BackColor = Color.Red;
                Fields.Name = false;
            }
            else
            {
                if (textBox_Name.BackColor == Color.Red)
                    textBox_Name.BackColor = Color.White;
                Fields.Name = true;
            }
            CheckFields();
        }

        private void textBox_Location_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Location.Text == "" || !Directory.Exists(@textBox_Location.Text))
            {
                textBox_Location.BackColor = Color.Red;
                Fields.Location = false;
            }
            else
            {
                if (textBox_Location.BackColor == Color.Red)
                    textBox_Location.BackColor = Color.White;
                Fields.Location = true;
            }
            CheckFields();
        }
        private void comboBox_BoardVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_BoardVersion.SelectedIndex != -1)
                Fields.BoardVersion = true;
            else Fields.BoardVersion = false;
            CheckFields();
        }
        private void CheckFields()
        {
            button_OK.Enabled = Fields.Name && Fields.Location && Fields.BoardVersion;
        }
        public Project.BoardType GetBoard()
        {
            return (Project.BoardType)comboBox_BoardVersion.SelectedIndex;
        }
        public string GetPath()
        {
            return path + textBox_Name.Text + Project.Extension;
        }
        public string GetName()
        {
            return textBox_Name.Text;
        }
    }
}
