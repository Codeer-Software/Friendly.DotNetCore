using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace TestStarter
{
    public class Class1
    {
        public static void Start(string path, string classFullName, string methodName)
        {
            var starter = @"C:\GitHub\Friendly.DotNetCore\Starter\Codeer.Friendly.Windows.DotNetCore.Starter\Resources\Friendly.TestHost.dll";

            var info = new ProcessStartInfo
            {
                FileName = "dotnet",
                WorkingDirectory = Path.GetDirectoryName(path),
                Arguments = "\"" + starter + "\" winforms " + "\"" + path + "\" " + classFullName + " " + methodName,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(info);
        }

        public static void StartWpf(string path, string appFullName, string mainWndFullName)
        {
            var starter = @"C:\GitHub\Friendly.DotNetCore\Starter\Codeer.Friendly.Windows.DotNetCore.Starter\Resources\Friendly.TestHost.dll";

            var info = new ProcessStartInfo
            {
                FileName = "dotnet",
                WorkingDirectory = Path.GetDirectoryName(path),
                Arguments = "\"" + starter + "\" wpf " + "\"" + path + "\" " + appFullName + " " + mainWndFullName,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(info);
        }

        [TestCase]
        public void TestWinFormws()
        {
            Start(@"C:\Users\ishik\Desktop\CoreWinForms\bin\Debug\netcoreapp3.0\CoreWinForms.dll", "CoreWinForms.Program", "Main");
        }



        [TestCase]
        public void TestWPF()
        {
            StartWpf(@"C:\Users\ishik\Desktop\CoreWPF\bin\Debug\netcoreapp3.0\CoreWPF.dll", "CoreWPF.App", "CoreWPF.MainWindow");
        }
    }
}
