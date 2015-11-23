using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Noear.UWP.Http {
    public class AsyncHttpResponse :IDisposable{
        protected IBuffer data { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }
        public string Cookies { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public Encoding Encoding { get; private set; }

        internal AsyncHttpResponse(HttpResponseMessage rsp, string encoding) {
            this.StatusCode = rsp.StatusCode;
            this.Encoding   = Encoding.GetEncoding(encoding);
            this.Headers    = new Dictionary<string, string>();
            this.Cookies    = null;
            init(rsp);
        }

        protected async void  init(HttpResponseMessage rsp) {
            if (rsp.StatusCode == HttpStatusCode.Ok) {
                data = await rsp.Content.ReadAsBufferAsync();
            }

            if (rsp.Headers != null) {
                foreach (var kv in rsp.Headers) {
                    if ("Set-Cookie".Equals(kv.Key)) {
                        Cookies = kv.Value;
                    }

                    Headers.Add(kv.Key, kv.Value);
                }
            }
        }

        public  IBuffer getBuffer() {
            return data;
        }

        public byte[] GetBytes() {
            if (data == null)
                return null;
            else
                return WindowsRuntimeBufferExtensions.ToArray(data, 0, (int)data.Length);
        }

        public string GetString() {
            var bytes = GetBytes();
            if (bytes == null)
                return null;
            else
                return this.Encoding.GetString(bytes);
        }

        public void Dispose() {
            data = null;
            Headers = null;
            Cookies = null;
            Encoding = null;
        }
    }
}
