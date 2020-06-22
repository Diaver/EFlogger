using System;
using System.Windows.Forms;
using EFlogger.EntityFramework4;


namespace EFloggerTestAppEF4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            EFloggerFor4.Initialize();

            EFloggerFor4.EnableDecompiling();
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}






