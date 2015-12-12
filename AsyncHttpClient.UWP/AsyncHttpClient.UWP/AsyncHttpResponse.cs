using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Storage.Streams;

namespace Noear.UWP.Http {
    public class AsyncHttpResponse :IDisposable{
        protected byte[] data { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }
        public string Cookies { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public Encoding Encoding { get; private set; }
        public Exception Exception { get; private set; }

        internal AsyncHttpResponse(HttpResponseMessage rsp, string encoding) {
            this.StatusCode = rsp.StatusCode;
            this.Encoding = Encoding.GetEncoding(encoding ?? "UTF-8");
            this.Headers = new Dictionary<string, string>();
            this.Cookies = null;
            this.Exception = null;
            init(rsp);
        }

        internal AsyncHttpResponse(Exception exp, string encoding) {
            this.StatusCode = 0;
            this.Encoding = Encoding.GetEncoding(encoding ?? "UTF-8");
            this.Headers = new Dictionary<string, string>();
            this.Cookies = null;
            this.Exception = exp;
        }

        protected async void  init(HttpResponseMessage rsp) {
            if (rsp.StatusCode == HttpStatusCode.OK) {
                data = await rsp.Content.ReadAsByteArrayAsync();
            }

            if (rsp.Headers != null) {
                foreach (var kv in rsp.Headers) {
                    if ("Set-Cookie".Equals(kv.Key)) {
                        Cookies = string.Join(";",kv.Value);
                    }

                    Headers[kv.Key] = string.Join(";", kv.Value);
                }
            }
        }

        public  IBuffer getBuffer() {
            if (data == null)
                return null;
            else
                return WindowsRuntimeBufferExtensions.AsBuffer(data);
        }

        public byte[] GetBytes() {
            if (data == null)
                return null;
            else
                return data;
        }

        public string GetString() {
            if (data == null)
                return null;
            else
                return Encoding.GetString(data);
        }

        public void Dispose() {
            data = null;
            Headers = null;
            Cookies = null;
            Encoding = null;
            Exception = null;
        }
    }
}
