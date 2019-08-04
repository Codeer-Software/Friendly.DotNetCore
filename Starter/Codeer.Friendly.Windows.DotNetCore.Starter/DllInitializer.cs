using Codeer.Friendly.Windows.DotNetCore.Starter.Properties;
using System.IO;

namespace Codeer.Friendly.Windows.DotNetCore.Starter
{
    static class DllInitializer
    {
        internal static string Init()
        {
            var dir = Path.GetDirectoryName(typeof(WinFormsStarter).Assembly.Location);
            Write(Path.Combine(dir, "Codeer.Friendly.Windows.ConnectionServer.dll"), Resources.Codeer_Friendly_Windows_ConnectionServer);
            Write(Path.Combine(dir, "Friendly.TestHost.dll"), Resources.Friendly_TestHost_Dll);
            Write(Path.Combine(dir, "Friendly.TestHost.exe"), Resources.Friendly_TestHost_Exe);
            Write(Path.Combine(dir, "Friendly.TestHost.runtimeconfig.dev.json"), Resources.Friendly_TestHost_runtimeconfig_dev);
            Write(Path.Combine(dir, "Friendly.TestHost.runtimeconfig.json"), Resources.Friendly_TestHost_runtimeconfig);

            return Path.Combine(dir, "Friendly.TestHost.exe");
        }

        static void Write(string path, byte[] bin)
        {
            if (File.Exists(path))
            {
                try
                {
                    if (IsMatchBinary(File.ReadAllBytes(path), bin)) return;
                }
                catch { }
            }
            File.WriteAllBytes(path, bin);
        }

        static bool IsMatchBinary(byte[] buf1, byte[] buf2)
        {
            if (buf1.Length != buf2.Length)
            {
                return false;
            }
            for (int i = 0; i < buf1.Length; i++)
            {
                if (buf1[i] != buf2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
