namespace gajwa_businfo_err
{
    partial class ErrWindow
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
            this.ErrText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AutoRebootLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ErrText
            // 
            this.ErrText.AutoSize = true;
            this.ErrText.Font = new System.Drawing.Font("Noto Sans Korean Bold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ErrText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ErrText.Location = new System.Drawing.Point(3, 90);
            this.ErrText.Name = "ErrText";
            this.ErrText.Size = new System.Drawing.Size(13, 15);
            this.ErrText.TabIndex = 0;
            this.ErrText.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::gajwa_businfo_err.Properties.Resources.baseline_warning_white_48dp;
            this.pictureBox1.Location = new System.Drawing.Point(6, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 54);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // AutoRebootLabel
            // 
            this.AutoRebootLabel.AutoSize = true;
            this.AutoRebootLabel.Font = new System.Drawing.Font("Noto Sans Korean Bold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AutoRebootLabel.ForeColor = System.Drawing.Color.White;
            this.AutoRebootLabel.Location = new System.Drawing.Point(63, 0);
            this.AutoRebootLabel.Name = "AutoRebootLabel";
            this.AutoRebootLabel.Size = new System.Drawing.Size(697, 23);
            this.AutoRebootLabel.TabIndex = 2;
            this.AutoRebootLabel.Text = "Input an random signal from the text input device to terminate this exception ses" +
    "sion.";
            // 
            // ErrWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(900, 960);
            this.Controls.Add(this.AutoRebootLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ErrText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ErrWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ErrWindow";
            this.Load += new System.EventHandler(this.ErrWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ErrText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label AutoRebootLabel;
    }
}