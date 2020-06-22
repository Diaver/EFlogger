using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using EFlogger.EntityFramework6;
using EFloggerTestAppEF6.Models;

namespace EFloggerTestAppEF6.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

           
            EFloggerFor6.SetProfilerClientIP("127.0.0.1");
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            test();
        }

        private void test()
        {
            using (var usersContext = new UsersContext())
            {
                User user = usersContext.Users.First();
                user.Name = "test1";
                usersContext.SaveChanges();

             //   List<User> dbSet = usersContext.Users.ToList();
            }
        }

        private void writeMessageButton_Click(object sender, EventArgs e)
        {
            EFloggerFor6.WriteMessage("Text message");
        }

        private void startSendToClientButton_Click(object sender, EventArgs e)
        {
            EFloggerFor6.StartSendToClient();
        }

        private void stopSendToClientButton_Click(object sender, EventArgs e)
        {
            EFloggerFor6.StopSendToClient();
        }

        private void startSaveToLogButton_Click(object sender, EventArgs e)
        {
           // EFloggerFor6.StartSaveToLogFile();
        }

        private void stopSaveToLogButton_Click(object sender, EventArgs e)
        {
          //  EFloggerFor6.StopSaveToLogFile();
        }

        private void clearLogButton_Click(object sender, EventArgs e)
        {
            EFloggerFor6.ClearLog();
        }
    }
}