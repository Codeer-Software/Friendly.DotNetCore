using Codeer.Friendly.DotNetExecutor;
using Friendly.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameworkStarter
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var path = args[0];
            var uri = args[1];
            var entry = args[2];

            var dir = Path.GetDirectoryName(path);
            var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories).
                        Concat(Directory.GetFiles(dir, "*.exe", SearchOption.AllDirectories)).ToArray();

            AssemblyManager.AddInterfaceType(typeof(FriendlyServer).Assembly);
            foreach (var e in dlls)
            {
                try
                {
                    var asm = Assembly.LoadFrom(e);
                    AssemblyManager.AddInterfaceType(asm);
                }
                catch { }
            }
            FriendlyServer.StartLoop(uri, (a) => Application.OpenForms[0].Invoke(a));

            var sp = entry.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var typeFullName = string.Join(".", sp.Take(sp.Length - 1));
            var method = sp[sp.Length - 1];
            var finder = new TypeFinder();
            finder.GetType(typeFullName).GetMethod(method, BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic).Invoke(null, args.Skip(3).Select(e=>(object)e).ToArray());
        }
    }
}
