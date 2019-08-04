using System.Diagnostics;
using System.IO;

namespace Friendly.DotNetCore
{
    public static class WinFormsStarter
    {
        public static void Start(string uri, string target, string entry)
        {
            var starter = @"C:\ProgramData\Friendly\Friendly.DotNetCore.WinFormsStarter.exe";

            var info = new ProcessStartInfo
            {
                FileName = starter,
                WorkingDirectory = Path.GetDirectoryName(target),
                Arguments = "\"" + target + "\" " + uri + " " + entry
            };

            Process.Start(info);
        }
    }
}
