using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Friendly.Core
{
    public class FriendlyServer
    {
        FriendlyControl _control;
        string _uri;
        bool _alive = false;
        HttpListener _listener;

        public static FriendlyServer Instance { get; private set; }
        
        public static void StartLoop(string uri, Action<Action> invoker)
        {
            Instance = new FriendlyServer();
            Instance.StartLoopCore(uri, invoker);
        }

        public void Stop() => _alive = false;

        void StartLoopCore(string uri, Action<Action> invoker)
        {
            _uri = uri;
            _control = new FriendlyControl(invoker);
            var tsk = Task.Factory.StartNew(Loop);
        }
        
        void Loop()
        {
            _alive = true;
            _listener = new HttpListener();
            _listener.Prefixes.Add(_uri);
            _listener.Start();
            while (_alive)
            {
                try
                {
                    LoopCore();
                }
                catch { }
            }
        }

        void LoopCore()
        {
            var context = _listener.GetContext();
            using (var response = context.Response)
            {
                ProtocolInfo protocolInfo;
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    string protocol;
                    protocol = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(protocol))
                    {
                        return;
                    }
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(protocol)))
                    {
                        var serializer = new DataContractSerializer(typeof(ProtocolInfo), AssemblyManager.GetDataContractableTypes());
                        protocolInfo = (ProtocolInfo)serializer.ReadObject(ms);
                    }
                }


                var returnInfo = _control.Execute(protocolInfo);


                byte[] bin = null;
                using (var ms = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(returnInfo.GetType(), AssemblyManager.GetDataContractableTypes());
                    serializer.WriteObject(ms, returnInfo);
                    bin = ms.ToArray();
                }
                response.OutputStream.Write(bin, 0, bin.Length);
            }
        }
    }
}
