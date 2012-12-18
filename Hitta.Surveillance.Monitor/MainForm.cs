using System;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.MonitorPanels;

namespace Hitta.Surveillance.Monitor
{
    public partial class MainForm : Form
    {
        public MainForm(string panelConfiguration)
        {
            InitializeComponent();
            try
            {
                var objectFactoryWrapper = new ObjectFactoryWrapper();
                var panels = objectFactoryWrapper.GetObject<PanelsCollection>(panelConfiguration);

                foreach (var panel in panels.Panels)
                {
                    monitorLayoutPanel1.Controls.Add(panel);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Faild to create monitors. The error was: " + ex.Message);
                throw;
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                ControlBox = false;
            }
        }

        private void monitorLayoutPanel1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                ControlBox = true;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                ControlBox = false;
            }
        }
    }
}
