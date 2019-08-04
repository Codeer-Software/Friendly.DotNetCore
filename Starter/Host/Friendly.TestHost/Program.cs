using Codeer.Friendly.Windows.ConnectionServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Friendly.TestHost
{
    static class Program
    {
        static Dictionary<string, Assembly> _asmDic = new Dictionary<string, Assembly>();

        [STAThread]
        static void Main(string[] args)
        {
            var option = args[0];
            var targetDllPath = args[1];

            //assembly resolver.
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                Assembly resolve;
                return _asmDic.TryGetValue(args.Name, out resolve) ? resolve : null;
            };

            //load all dlls.
            var dir = Path.GetDirectoryName(targetDllPath);
            var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories).
                        Concat(Directory.GetFiles(dir, "*.exe", SearchOption.AllDirectories)).ToArray();
            foreach (var e in dlls)
            {
                try
                {
                    var asm = Assembly.LoadFrom(e);
                    if (asm != null)
                    {
                        _asmDic[asm.FullName] = asm;
                    }
                }
                catch { }
            }

            //start server.
            NamedPipeConnectionServer.Start();

            //start app.
            if (option == "winforms")
            {
                var programTypeFullName = args[2];
                var mainMethod = args[3];
                Assembly.LoadFrom(targetDllPath).
                    GetType(programTypeFullName).
                    GetMethod(mainMethod, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args.Skip(4).Select(e => (object)e).ToArray());
            }
            else if (option == "wpf")
            {
                var appTypeFullname = args[2];
                var windowTypeFullName = args[3];

                var targetAsm = Assembly.LoadFrom(targetDllPath);
                var appType = targetAsm.GetType(appTypeFullname);
                var app = (System.Windows.Application)Activator.CreateInstance(appType);

                var windowType = targetAsm.GetType(windowTypeFullName);
                var window = (System.Windows.Window)Activator.CreateInstance(windowType);

                app.Run(window);
            }
        }
    }
}
