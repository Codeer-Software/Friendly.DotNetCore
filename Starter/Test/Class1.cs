using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.DotNetCore.Starter;
using NUnit.Framework;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Threading;

namespace Test
{
    public class Class1
    {
        [TestCase]
        public void Test()
        {
            var process = WinFormsStarter.Start(@"C:\Users\ishik\Desktop\CoreWinForms\bin\Debug\netcoreapp3.0\CoreWinForms.dll", "CoreWinForms.Program", "Main");

            var pipe = new NamedPipeConnection(process);
            var app = new WindowsAppFriend(pipe);
            app.Type<Application>().OpenForms[0].Text = "abc";
        }
    }
}
