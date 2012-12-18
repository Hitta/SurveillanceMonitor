namespace Hitta.Surveillance.Monitor
{
    partial class MainForm
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
            this.monitorLayoutPanel1 = new Hitta.Surveillance.Monitor.MonitorLayoutPanel();
            this.SuspendLayout();
            // 
            // monitorLayoutPanel1
            // 
            this.monitorLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.monitorLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.monitorLayoutPanel1.Name = "monitorLayoutPanel1";
            this.monitorLayoutPanel1.Size = new System.Drawing.Size(954, 441);
            this.monitorLayoutPanel1.TabIndex = 0;
            this.monitorLayoutPanel1.DoubleClick += new System.EventHandler(this.monitorLayoutPanel1_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 441);
            this.ControlBox = false;
            this.Controls.Add(this.monitorLayoutPanel1);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        private MonitorLayoutPanel monitorLayoutPanel1;



    }
}

