namespace LED_Board_Access
{
    partial class LED_Board_Manager
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
            this.IP_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_UpdateSKD = new System.Windows.Forms.Button();
            this.button_check = new System.Windows.Forms.Button();
            this.button_Restart = new System.Windows.Forms.Button();
            this.button_LoadProject = new System.Windows.Forms.Button();
            this.progressBar_Files = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar_File = new System.Windows.Forms.ProgressBar();
            this.label_filesProgress = new System.Windows.Forms.Label();
            this.label_fileProgress = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // IP_textBox
            // 
            this.IP_textBox.Location = new System.Drawing.Point(28, 89);
            this.IP_textBox.Name = "IP_textBox";
            this.IP_textBox.Size = new System.Drawing.Size(100, 20);
            this.IP_textBox.TabIndex = 26;
            this.IP_textBox.Text = "10.160.248.15";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "IP:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button_UpdateSKD);
            this.groupBox2.Controls.Add(this.button_check);
            this.groupBox2.Controls.Add(this.IP_textBox);
            this.groupBox2.Controls.Add(this.button_Restart);
            this.groupBox2.Controls.Add(this.button_LoadProject);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 125);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Commands";
            // 
            // button_UpdateSKD
            // 
            this.button_UpdateSKD.BackColor = System.Drawing.Color.Red;
            this.button_UpdateSKD.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button_UpdateSKD.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.button_UpdateSKD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button_UpdateSKD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_UpdateSKD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_UpdateSKD.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_UpdateSKD.Location = new System.Drawing.Point(253, 31);
            this.button_UpdateSKD.Name = "button_UpdateSKD";
            this.button_UpdateSKD.Size = new System.Drawing.Size(113, 36);
            this.button_UpdateSKD.TabIndex = 3;
            this.button_UpdateSKD.Text = "Update SKD";
            this.button_UpdateSKD.UseVisualStyleBackColor = false;
            this.button_UpdateSKD.Click += new System.EventHandler(this.button_UpdateSchedule_Click);
            // 
            // button_check
            // 
            this.button_check.BackColor = System.Drawing.Color.Blue;
            this.button_check.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button_check.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.button_check.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_check.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_check.Location = new System.Drawing.Point(15, 31);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(113, 36);
            this.button_check.TabIndex = 2;
            this.button_check.Text = "Check";
            this.button_check.UseVisualStyleBackColor = false;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // button_Restart
            // 
            this.button_Restart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_Restart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button_Restart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.button_Restart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button_Restart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Restart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Restart.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Restart.Location = new System.Drawing.Point(134, 31);
            this.button_Restart.Name = "button_Restart";
            this.button_Restart.Size = new System.Drawing.Size(113, 36);
            this.button_Restart.TabIndex = 1;
            this.button_Restart.Text = "Restart";
            this.button_Restart.UseVisualStyleBackColor = false;
            this.button_Restart.Click += new System.EventHandler(this.button_Restart_Click);
            // 
            // button_LoadProject
            // 
            this.button_LoadProject.BackColor = System.Drawing.Color.Red;
            this.button_LoadProject.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button_LoadProject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.button_LoadProject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button_LoadProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_LoadProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_LoadProject.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_LoadProject.Location = new System.Drawing.Point(253, 73);
            this.button_LoadProject.Name = "button_LoadProject";
            this.button_LoadProject.Size = new System.Drawing.Size(113, 36);
            this.button_LoadProject.TabIndex = 0;
            this.button_LoadProject.Text = "Load Project";
            this.button_LoadProject.UseVisualStyleBackColor = false;
            this.button_LoadProject.Click += new System.EventHandler(this.button_LoadProject_Click);
            // 
            // progressBar_Files
            // 
            this.progressBar_Files.Location = new System.Drawing.Point(19, 173);
            this.progressBar_Files.Name = "progressBar_Files";
            this.progressBar_Files.Size = new System.Drawing.Size(368, 24);
            this.progressBar_Files.Step = 1;
            this.progressBar_Files.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 32;
            this.label1.Text = "Files:";
            // 
            // progressBar_File
            // 
            this.progressBar_File.Location = new System.Drawing.Point(19, 229);
            this.progressBar_File.Name = "progressBar_File";
            this.progressBar_File.Size = new System.Drawing.Size(368, 24);
            this.progressBar_File.Step = 1;
            this.progressBar_File.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_File.TabIndex = 34;
            // 
            // label_filesProgress
            // 
            this.label_filesProgress.AutoSize = true;
            this.label_filesProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_filesProgress.Location = new System.Drawing.Point(61, 150);
            this.label_filesProgress.Name = "label_filesProgress";
            this.label_filesProgress.Size = new System.Drawing.Size(11, 16);
            this.label_filesProgress.TabIndex = 35;
            this.label_filesProgress.Text = ".";
            // 
            // label_fileProgress
            // 
            this.label_fileProgress.AutoSize = true;
            this.label_fileProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_fileProgress.Location = new System.Drawing.Point(18, 210);
            this.label_fileProgress.Name = "label_fileProgress";
            this.label_fileProgress.Size = new System.Drawing.Size(11, 16);
            this.label_fileProgress.TabIndex = 36;
            this.label_fileProgress.Text = ".";
            // 
            // LED_Board_Manager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(409, 269);
            this.Controls.Add(this.label_fileProgress);
            this.Controls.Add(this.label_filesProgress);
            this.Controls.Add(this.progressBar_File);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar_Files);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LED_Board_Manager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LED Board Manager";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox IP_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Restart;
        private System.Windows.Forms.Button button_LoadProject;
        private System.Windows.Forms.ProgressBar progressBar_Files;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar_File;
        private System.Windows.Forms.Label label_filesProgress;
        private System.Windows.Forms.Label label_fileProgress;
        private System.Windows.Forms.Button button_UpdateSKD;
        private System.Windows.Forms.Button button_check;
    }
}