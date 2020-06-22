using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows.Forms;
using EFloggerApp.Controllers;
using EFloggerApp.Properties;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace EFloggerApp
{
    static class Program
    {
        private const string EfloggerLog = "EFlogger.log";
        public static Logger Logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupLogger();


           // if (!Debugger.IsAttached)
           // {
                Application.ThreadException += (sender, e) => HandleError(e.Exception);

                //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, true);
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleError((Exception) e.ExceptionObject);
        //    }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainFormController mainFormController = new MainFormController();


            Application.Run(mainFormController.Run());
        }

        private static void SetupLogger()
        {
            try
            {
                ConfigureNlog();
                Logger = LogManager.GetCurrentClassLogger();
                Logger.Trace("Version: {0}", Environment.Version.ToString());
                Logger.Trace("OS: {0}", Environment.OSVersion.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Error work with logger!\n Send to developer" + e.Message);
                if (MessageBox.Show("Attach debugger? \n Only for developer!!!", "Debugging...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Debugger.Launch();
                    throw;
                }
            }
        }

        private static void HandleError(Exception exception)
        {
            if (exception is SocketException)
            {
                MessageBox.Show("Looks like profiler already runned; " + exception.Message, "SocketException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            try
            {

                new ErrorHandlerController(exception).Run();
            }
            catch (Exception e)
            {

                MessageBox.Show("Error processing exception. Please send log file EFlogger.log to developer: " + Settings.Default.ProgrammerEmail + " \r\n Exception:" + e);
                Logger.Error(e);
                if (MessageBox.Show("Attach debugger? \n Only for developer!!!", "Debugging...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Debugger.Launch();
                    throw;
                }
            }
            finally
            {
                Environment.Exit(1);
            }
        }

        private static void ConfigureNlog()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            fileTarget.Layout = @"${longdate} ${message}";
            fileTarget.FileName = "${basedir}/" + EfloggerLog;

            var rule2 = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
        }
    }
}
