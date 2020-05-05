namespace SOE_IDE
{
    partial class PictureBoxToolTip
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
            this.label_Position = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Position
            // 
            this.label_Position.AutoSize = true;
            this.label_Position.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Position.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label_Position.Location = new System.Drawing.Point(5, 0);
            this.label_Position.Name = "label_Position";
            this.label_Position.Size = new System.Drawing.Size(47, 19);
            this.label_Position.TabIndex = 0;
            this.label_Position.Text = "( 5; 5)";
            // 
            // PictureBoxToolTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.Controls.Add(this.label_Position);
            this.Name = "PictureBoxToolTip";
            this.Size = new System.Drawing.Size(60, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Position;

    }
}
