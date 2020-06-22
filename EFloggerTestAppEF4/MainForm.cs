using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace EFloggerTestAppEF4
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            

        }

        private int couint = 13465;
        private void button1_Click(object sender, EventArgs e)
        {
            ModelContainer usersContext = new ModelContainer();

            User user = usersContext.Users.First();
            user.Name = "vasy" + couint;
                 usersContext.SaveChanges();
            couint++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
