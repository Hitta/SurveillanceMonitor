namespace Hitta.Surveillance.Monitor
{
    partial class MonitorPanel
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textPanelName = new Hitta.Surveillance.Monitor.TextPanel();
            this.textPanelDescription = new Hitta.Surveillance.Monitor.TextPanel();
            this.textPanelValue = new Hitta.Surveillance.Monitor.TextPanel();
            this.graph1 = new Hitta.Surveillance.Monitor.Graph();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textPanelName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textPanelDescription, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textPanelValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.graph1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(703, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textPanelName
            // 
            this.textPanelName.BackColor = System.Drawing.Color.Black;
            this.textPanelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelName.ForeColor = System.Drawing.Color.Silver;
            this.textPanelName.Location = new System.Drawing.Point(353, 3);
            this.textPanelName.Name = "textPanelName";
            this.textPanelName.Size = new System.Drawing.Size(347, 128);
            this.textPanelName.TabIndex = 1;
            // 
            // textPanelDescription
            // 
            this.textPanelDescription.BackColor = System.Drawing.Color.Black;
            this.textPanelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelDescription.ForeColor = System.Drawing.Color.Silver;
            this.textPanelDescription.Location = new System.Drawing.Point(353, 137);
            this.textPanelDescription.Name = "textPanelDescription";
            this.textPanelDescription.Size = new System.Drawing.Size(347, 52);
            this.textPanelDescription.TabIndex = 2;
            // 
            // textPanelValue
            // 
            this.textPanelValue.BackColor = System.Drawing.Color.Black;
            this.textPanelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelValue.ForeColor = System.Drawing.Color.Green;
            this.textPanelValue.Location = new System.Drawing.Point(178, 3);
            this.textPanelValue.Name = "textPanelValue";
            this.tableLayoutPanel1.SetRowSpan(this.textPanelValue, 2);
            this.textPanelValue.Size = new System.Drawing.Size(169, 186);
            this.textPanelValue.TabIndex = 0;
            // 
            // graph1
            // 
            this.graph1.BackColor = System.Drawing.Color.Black;
            this.graph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graph1.GridColor = System.Drawing.Color.DarkGreen;
            this.graph1.Interval = 1;
            this.graph1.Location = new System.Drawing.Point(3, 3);
            this.graph1.Name = "graph1";
            this.tableLayoutPanel1.SetRowSpan(this.graph1, 2);
            this.graph1.Size = new System.Drawing.Size(169, 186);
            this.graph1.TabIndex = 3;
            this.graph1.Text = "graph1";
            this.graph1.Value = 0;
            this.graph1.YScale = 100;
            // 
            // MonitorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MonitorPanel";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(707, 196);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private TextPanel textPanelValue;
        private TextPanel textPanelName;
        private TextPanel textPanelDescription;
        private Graph graph1;
    }
}
