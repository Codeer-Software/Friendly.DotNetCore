using System.IO;
using System.Net;
using System.Text;
using Friendly.Core;

namespace Friendly.DotNetCore.Inside
{
    class FriendlySender
    {
        string _uri;
        ProtocolSerializer _serializer;

        internal void Init(string uri)
        {
            _uri = uri;
            _serializer = new ProtocolSerializer();
        }

        internal ReturnInfo SendAndReceive(ProtocolInfo info)
        {
            var bin = _serializer.WriteObject(info);

            var request = (HttpWebRequest)WebRequest.Create(_uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var requestStream = request.GetRequestStreamAsync();
            requestStream.Wait();
            using (var reqStream = requestStream.Result)
            {
                reqStream.Write(bin, 0, bin.Length);
            }

            var respons = request.GetResponseAsync();
            respons.Wait();

            using (var httpWebResponsex = (HttpWebResponse)respons.Result)
            using (var reader = new StreamReader(httpWebResponsex.GetResponseStream()))
            {
                string requestString;
                requestString = reader.ReadToEnd();
                if (string.IsNullOrEmpty(requestString))
                {
                    return null;
                }
                return (ReturnInfo)_serializer.ReadObject(Encoding.UTF8.GetBytes(requestString));
            }
        }

        internal void Stop() { }
        
    }
}
