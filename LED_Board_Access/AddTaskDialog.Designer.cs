namespace LED_Board_Access
{
    partial class AddTaskDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_themes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.maskedTextBox_time = new System.Windows.Forms.MaskedTextBox();
            this.checkBox_Monday = new System.Windows.Forms.CheckBox();
            this.groupBox_Days = new System.Windows.Forms.GroupBox();
            this.checkBox_Tuesday = new System.Windows.Forms.CheckBox();
            this.checkBox_Wednesday = new System.Windows.Forms.CheckBox();
            this.checkBox_Thursday = new System.Windows.Forms.CheckBox();
            this.checkBox_Friday = new System.Windows.Forms.CheckBox();
            this.checkBox_Saturday = new System.Windows.Forms.CheckBox();
            this.checkBox_Sunday = new System.Windows.Forms.CheckBox();
            this.groupBox_Days.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_themes
            // 
            this.comboBox_themes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_themes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox_themes.FormattingEnabled = true;
            this.comboBox_themes.Location = new System.Drawing.Point(115, 112);
            this.comboBox_themes.Name = "comboBox_themes";
            this.comboBox_themes.Size = new System.Drawing.Size(152, 28);
            this.comboBox_themes.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(112, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(112, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Theme";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(192, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(115, 169);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // maskedTextBox_time
            // 
            this.maskedTextBox_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox_time.Location = new System.Drawing.Point(141, 45);
            this.maskedTextBox_time.Mask = "00:00 - 00:00";
            this.maskedTextBox_time.Name = "maskedTextBox_time";
            this.maskedTextBox_time.Size = new System.Drawing.Size(100, 26);
            this.maskedTextBox_time.TabIndex = 9;
            this.maskedTextBox_time.TextChanged += new System.EventHandler(this.maskedTextBox_time_TextChanged);
            // 
            // checkBox_Monday
            // 
            this.checkBox_Monday.AutoSize = true;
            this.checkBox_Monday.Location = new System.Drawing.Point(6, 19);
            this.checkBox_Monday.Name = "checkBox_Monday";
            this.checkBox_Monday.Size = new System.Drawing.Size(64, 17);
            this.checkBox_Monday.TabIndex = 10;
            this.checkBox_Monday.Text = "Monday";
            this.checkBox_Monday.UseVisualStyleBackColor = true;
            // 
            // groupBox_Days
            // 
            this.groupBox_Days.Controls.Add(this.checkBox_Sunday);
            this.groupBox_Days.Controls.Add(this.checkBox_Saturday);
            this.groupBox_Days.Controls.Add(this.checkBox_Friday);
            this.groupBox_Days.Controls.Add(this.checkBox_Thursday);
            this.groupBox_Days.Controls.Add(this.checkBox_Wednesday);
            this.groupBox_Days.Controls.Add(this.checkBox_Tuesday);
            this.groupBox_Days.Controls.Add(this.checkBox_Monday);
            this.groupBox_Days.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Days.Name = "groupBox_Days";
            this.groupBox_Days.Size = new System.Drawing.Size(94, 181);
            this.groupBox_Days.TabIndex = 11;
            this.groupBox_Days.TabStop = false;
            this.groupBox_Days.Text = "Days";
            // 
            // checkBox_Tuesday
            // 
            this.checkBox_Tuesday.AutoSize = true;
            this.checkBox_Tuesday.Location = new System.Drawing.Point(6, 42);
            this.checkBox_Tuesday.Name = "checkBox_Tuesday";
            this.checkBox_Tuesday.Size = new System.Drawing.Size(67, 17);
            this.checkBox_Tuesday.TabIndex = 11;
            this.checkBox_Tuesday.Text = "Tuesday";
            this.checkBox_Tuesday.UseVisualStyleBackColor = true;
            this.checkBox_Tuesday.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox_Wednesday
            // 
            this.checkBox_Wednesday.AutoSize = true;
            this.checkBox_Wednesday.Location = new System.Drawing.Point(6, 65);
            this.checkBox_Wednesday.Name = "checkBox_Wednesday";
            this.checkBox_Wednesday.Size = new System.Drawing.Size(83, 17);
            this.checkBox_Wednesday.TabIndex = 12;
            this.checkBox_Wednesday.Text = "Wednesday";
            this.checkBox_Wednesday.UseVisualStyleBackColor = true;
            // 
            // checkBox_Thursday
            // 
            this.checkBox_Thursday.AutoSize = true;
            this.checkBox_Thursday.Location = new System.Drawing.Point(6, 88);
            this.checkBox_Thursday.Name = "checkBox_Thursday";
            this.checkBox_Thursday.Size = new System.Drawing.Size(70, 17);
            this.checkBox_Thursday.TabIndex = 13;
            this.checkBox_Thursday.Text = "Thursday";
            this.checkBox_Thursday.UseVisualStyleBackColor = true;
            // 
            // checkBox_Friday
            // 
            this.checkBox_Friday.AutoSize = true;
            this.checkBox_Friday.Location = new System.Drawing.Point(6, 111);
            this.checkBox_Friday.Name = "checkBox_Friday";
            this.checkBox_Friday.Size = new System.Drawing.Size(54, 17);
            this.checkBox_Friday.TabIndex = 14;
            this.checkBox_Friday.Text = "Friday";
            this.checkBox_Friday.UseVisualStyleBackColor = true;
            // 
            // checkBox_Saturday
            // 
            this.checkBox_Saturday.AutoSize = true;
            this.checkBox_Saturday.ForeColor = System.Drawing.Color.Red;
            this.checkBox_Saturday.Location = new System.Drawing.Point(6, 134);
            this.checkBox_Saturday.Name = "checkBox_Saturday";
            this.checkBox_Saturday.Size = new System.Drawing.Size(68, 17);
            this.checkBox_Saturday.TabIndex = 15;
            this.checkBox_Saturday.Text = "Saturday";
            this.checkBox_Saturday.UseVisualStyleBackColor = true;
            // 
            // checkBox_Sunday
            // 
            this.checkBox_Sunday.AutoSize = true;
            this.checkBox_Sunday.ForeColor = System.Drawing.Color.Red;
            this.checkBox_Sunday.Location = new System.Drawing.Point(6, 157);
            this.checkBox_Sunday.Name = "checkBox_Sunday";
            this.checkBox_Sunday.Size = new System.Drawing.Size(62, 17);
            this.checkBox_Sunday.TabIndex = 16;
            this.checkBox_Sunday.Text = "Sunday";
            this.checkBox_Sunday.UseVisualStyleBackColor = true;
            // 
            // AddTaskDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(281, 205);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox_Days);
            this.Controls.Add(this.maskedTextBox_time);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_themes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddTaskDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Task";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddTaskDialog_FormClosing);
            this.groupBox_Days.ResumeLayout(false);
            this.groupBox_Days.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_themes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_time;
        private System.Windows.Forms.CheckBox checkBox_Monday;
        private System.Windows.Forms.GroupBox groupBox_Days;
        private System.Windows.Forms.CheckBox checkBox_Friday;
        private System.Windows.Forms.CheckBox checkBox_Thursday;
        private System.Windows.Forms.CheckBox checkBox_Wednesday;
        private System.Windows.Forms.CheckBox checkBox_Tuesday;
        private System.Windows.Forms.CheckBox checkBox_Sunday;
        private System.Windows.Forms.CheckBox checkBox_Saturday;
    }
}