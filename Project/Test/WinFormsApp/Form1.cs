using Friendly.Core;
using Friendly.DotNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            AssemblyManager.AddInterfaceType(GetType().Assembly);
            AssemblyManager.AddInterfaceType(typeof(FriendlyServer).Assembly);
        //    AssemblyManager.AddInterfaceType(Application.OpenForms[0].GetType().Assembly);
            FriendlyServer.StartLoop("http://localhost:8081/", (a) => Invoke(a));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => 
            {
                using (var app = new DotNetCoreAppFriend("http://localhost:8081/"))
                {
                    int val = app.Type("WinFormsApp.Form1").SFunc();
                }
            });
        }

        public static int SFunc()
        {
            return 100;
        }
    }
}
