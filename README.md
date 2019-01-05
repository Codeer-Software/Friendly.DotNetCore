# Friendly.DotNetCore
Friendly for .NetCore WinForms & WPF.<br>
This is prototype.
```cs  
using Friendly.DotNetCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly.Dynamic;
using System.Windows.Forms;

namespace ExecuteTest
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void Test()
        {
            //Start 
            var uri = "http://localhost:8081/";
            var target = @"c:\Test\CoreForms.exe";
            var entry = "CoreForms.Program.Main";
            WinFormsStarter.Start(uri, target, entry);

            //Attach
            using (var app = new DotNetCoreAppFriend(uri))
            {
                //Friendly operation.
                var form = app.Type<Application>().OpenForms[0];
                form.label1.Text = "Hello Friendly!";
            }
        }
    }
}
```