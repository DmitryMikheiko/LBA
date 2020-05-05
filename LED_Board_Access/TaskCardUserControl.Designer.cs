namespace LED_Board_Access
{
    partial class TaskCardUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_time = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_theme_name = new System.Windows.Forms.Label();
            this.button_redo = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.pictureBox_theme_screen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_theme_screen)).BeginInit();
            this.SuspendLayout();
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_time.Location = new System.Drawing.Point(107, 23);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(94, 20);
            this.label_time.TabIndex = 1;
            this.label_time.Text = "00:00-01:00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(88, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Theme:";
            // 
            // label_theme_name
            // 
            this.label_theme_name.AutoSize = true;
            this.label_theme_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_theme_name.Location = new System.Drawing.Point(139, 56);
            this.label_theme_name.Name = "label_theme_name";
            this.label_theme_name.Size = new System.Drawing.Size(15, 16);
            this.label_theme_name.TabIndex = 5;
            this.label_theme_name.Text = "1";
            // 
            // button_redo
            // 
            this.button_redo.BackgroundImage = global::LED_Board_Access.Properties.Resources.Pen_1;
            this.button_redo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_redo.FlatAppearance.BorderSize = 0;
            this.button_redo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_redo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_redo.Location = new System.Drawing.Point(219, 32);
            this.button_redo.Name = "button_redo";
            this.button_redo.Size = new System.Drawing.Size(25, 25);
            this.button_redo.TabIndex = 4;
            this.button_redo.UseVisualStyleBackColor = true;
            this.button_redo.Visible = false;
            this.button_redo.MouseEnter += new System.EventHandler(this.button_redo_MouseEnter);
            this.button_redo.MouseLeave += new System.EventHandler(this.button_redo_MouseLeave);
            // 
            // button_close
            // 
            this.button_close.BackgroundImage = global::LED_Board_Access.Properties.Resources.Close_button_2;
            this.button_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_close.FlatAppearance.BorderSize = 0;
            this.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_close.Location = new System.Drawing.Point(219, 3);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(25, 25);
            this.button_close.TabIndex = 3;
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.MouseEnter += new System.EventHandler(this.button_close_MouseEnter);
            this.button_close.MouseLeave += new System.EventHandler(this.button_close_MouseLeave);
            // 
            // pictureBox_theme_screen
            // 
            this.pictureBox_theme_screen.BackgroundImage = global::LED_Board_Access.Properties.Resources.no_fees_icon_B;
            this.pictureBox_theme_screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_theme_screen.InitialImage = global::LED_Board_Access.Properties.Resources.no_fees_icon_B;
            this.pictureBox_theme_screen.Location = new System.Drawing.Point(10, 10);
            this.pictureBox_theme_screen.Name = "pictureBox_theme_screen";
            this.pictureBox_theme_screen.Size = new System.Drawing.Size(72, 72);
            this.pictureBox_theme_screen.TabIndex = 0;
            this.pictureBox_theme_screen.TabStop = false;
            // 
            // TaskCardUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label_theme_name);
            this.Controls.Add(this.button_redo);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.pictureBox_theme_screen);
            this.Name = "TaskCardUserControl";
            this.Size = new System.Drawing.Size(250, 90);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TaskCardUserControl_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_theme_screen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_theme_screen;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_theme_name;
        public System.Windows.Forms.Button button_close;
        public System.Windows.Forms.Button button_redo;
    }
}
