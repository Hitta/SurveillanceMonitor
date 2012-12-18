using System;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(GetPanelConfig(args)));
        }

        static string GetPanelConfig(string[] args)
        {
            if(args.Length != 1) throw new ArgumentException("Invalid startup argument! Please specify which panel configuration to use!");

            return args[0];
        }

    }
}
