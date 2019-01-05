using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Inside.Protocol;
using Friendly.DotNetCore.Inside;
using System;
using System.Threading;

namespace Friendly.DotNetCore
{
    public class DotNetCoreAppFriend : AppFriend, IDisposable
    {
        Core _core;

        protected override IFriendlyConnector FriendlyConnector { get { return _core; } }
        
        public DotNetCoreAppFriend(string uri)
        {
            _core = new Core() { App = this };
            _core.Sender.Init(uri);
            Friendly.Core.ResourcesLocal.Initialize();

            while (true)
            {
                try
                {
                    this.Type<Friendly.Core.ResourcesLocal>().Instance = Friendly.Core.ResourcesLocal.Instance;
                    break;
                }
                catch { }
                Thread.Sleep(100);
            }

        }

        class Core : IFriendlyConnector
        {
            public FriendlySender Sender { get; set; } = new FriendlySender();
            public AppFriend App { get; set; }
            public object Identity { get { return App; } }

            public ReturnInfo SendAndReceive(ProtocolInfo info)
            {
                if (Sender == null) return null;
                return Converter.Convert(Sender.SendAndReceive(Converter.Convert(info)));
            }
        }

        public void Dispose()
        {
            _core.Sender.Stop();
            _core.Sender = null;//TODO
        }
    }
}
