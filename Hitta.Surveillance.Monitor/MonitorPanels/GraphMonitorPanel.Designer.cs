namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    partial class GraphMonitorPanel
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
            this.textPanelName = new Hitta.Surveillance.Monitor.MonitorPanels.TextPanel();
            this.textPanelDescription = new Hitta.Surveillance.Monitor.MonitorPanels.TextPanel();
            this.textPanelValue = new Hitta.Surveillance.Monitor.MonitorPanels.TextPanel();
            this.mainLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayoutTable.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textPanelName
            // 
            this.textPanelName.BackColor = System.Drawing.Color.Black;
            this.textPanelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelName.ForeColor = System.Drawing.Color.Silver;
            this.textPanelName.Location = new System.Drawing.Point(0, 0);
            this.textPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.textPanelName.Name = "textPanelName";
            this.textPanelName.Size = new System.Drawing.Size(464, 139);
            this.textPanelName.TabIndex = 1;
            // 
            // textPanelDescription
            // 
            this.textPanelDescription.BackColor = System.Drawing.Color.Black;
            this.textPanelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelDescription.ForeColor = System.Drawing.Color.Silver;
            this.textPanelDescription.Location = new System.Drawing.Point(0, 139);
            this.textPanelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.textPanelDescription.Name = "textPanelDescription";
            this.textPanelDescription.Size = new System.Drawing.Size(464, 35);
            this.textPanelDescription.TabIndex = 2;
            // 
            // textPanelValue
            // 
            this.textPanelValue.BackColor = System.Drawing.Color.Black;
            this.textPanelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanelValue.ForeColor = System.Drawing.Color.Green;
            this.textPanelValue.Location = new System.Drawing.Point(232, 0);
            this.textPanelValue.Margin = new System.Windows.Forms.Padding(0);
            this.textPanelValue.Name = "textPanelValue";
            this.textPanelValue.Size = new System.Drawing.Size(232, 174);
            this.textPanelValue.TabIndex = 0;
            // 
            // mainLayoutTable
            // 
            this.mainLayoutTable.ColumnCount = 3;
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutTable.Controls.Add(this.textPanelValue, 1, 0);
            this.mainLayoutTable.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.mainLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutTable.Location = new System.Drawing.Point(2, 2);
            this.mainLayoutTable.Margin = new System.Windows.Forms.Padding(0);
            this.mainLayoutTable.Name = "mainLayoutTable";
            this.mainLayoutTable.RowCount = 1;
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayoutTable.Size = new System.Drawing.Size(928, 174);
            this.mainLayoutTable.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.textPanelDescription, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.textPanelName, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(464, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(464, 174);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // GraphMonitorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutTable);
            this.Name = "GraphMonitorPanel";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(932, 178);
            this.mainLayoutTable.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextPanel textPanelValue;
        private TextPanel textPanelName;
        private TextPanel textPanelDescription;
        private System.Windows.Forms.TableLayoutPanel mainLayoutTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
