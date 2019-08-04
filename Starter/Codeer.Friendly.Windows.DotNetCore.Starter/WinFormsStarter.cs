using System.Diagnostics;
using System.IO;

namespace Codeer.Friendly.Windows.DotNetCore.Starter
{
    public static class WinFormsStarter
    {
        public static Process Start(string targetDllPath, string classFullName, string methodName)
        {
            var starter = DllInitializer.Init();

          //  var starter = @"C:\GitHub\Friendly.DotNetCore\Starter\Codeer.Friendly.Windows.DotNetCore.Starter\Resources\Friendly.TestHost.exe";
           // var starter = @"C:\GitHub\Friendly.DotNetCore\Starter\Test\bin\Debug\Friendly.TestHost.exe";

            var info = new ProcessStartInfo
            {
                FileName = starter,
                WorkingDirectory = Path.GetDirectoryName(targetDllPath),
                Arguments = "winforms " + "\"" + targetDllPath + "\" " + classFullName + " " + methodName,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            return Process.Start(info);
        }


        public static Process StartX(string targetDllPath, string classFullName, string methodName)
        {
            var starter = @"C:\GitHub\Friendly.DotNetCore\Starter\Codeer.Friendly.Windows.DotNetCore.Starter\Resources\Friendly.TestHost.dll";

            var info = new ProcessStartInfo
            {
                FileName = "dotnet",
                WorkingDirectory = Path.GetDirectoryName(targetDllPath),
                Arguments = "\"" + starter + "\" winforms " + "\"" + targetDllPath + "\" " + classFullName + " " + methodName,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            return Process.Start(info);
        }
    }
}
