using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codeer.Friendly.Windows.ConnectionServer
{
    public static class NamedPipeConnectionServer
    {
        static string PipeName => "Codeer.Friendly.Windows.NamedPipeConnection:" + Process.GetCurrentProcess().Id;

        public static Task Start() => Task.Factory.StartNew(() => WaitRequest(PipeName));

        static void WaitRequest(string pipeName)
        {
            while (true)
            {
                using (var stream = new NamedPipeServerStream(pipeName))
                {
                    // クライアントからの接続を待つ
                    stream.WaitForConnectionAsync().Wait();

                    var bin = new byte[1024 * 1024];
                    var len = stream.Read(bin, 0, bin.Length);
                    if (len == 0) continue;

                    var msg = Encoding.Unicode.GetString(bin.Take(len).ToArray());
                    var data = msg.Split('|');

                    //初期化
                    var asm1 = Assembly.LoadFrom(data[0]);
                    var asm2 = Assembly.LoadFrom(data[1]);
                    asm2.GetType(data[2]).GetMethod(data[3]).Invoke(null, new object[] { data[4] });
                }
            }
        }
    }
}
