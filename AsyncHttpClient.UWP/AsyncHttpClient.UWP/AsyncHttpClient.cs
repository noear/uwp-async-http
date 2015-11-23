using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Noear.UWP.Http
{
    public class AsyncHttpClient
    {
        private string _url;
        private Dictionary<string, string> _headers;
        private Dictionary<string, string> _cookies;
        private string _encoding;

        public AsyncHttpClient Url(string url) {
            _url = url;
            return this;
        }

       
        public AsyncHttpClient Encoding(string encoding) {
            _encoding = encoding;
            return this;
        }

       
        public AsyncHttpClient Header(string name, string value) {
            if (_headers == null) {
                _headers = new Dictionary<string, string>();
            }
            _headers.Add(name, value);
            
            return this;
        }
        
        public AsyncHttpClient Cookie(string name, string value) {
            if (_cookies == null) {
                _cookies = new Dictionary<string, string>();
            }
            _cookies.Add(name, value);

            return this;
        }

        public async Task<AsyncHttpResponse> Get() {
            var client = DoBuildHttpClient();

            using (var rsp = await client.GetAsync(new Uri(_url))) {
                return new AsyncHttpResponse(rsp, _encoding);
            }
        }
        
        public async Task<AsyncHttpResponse> Post(Dictionary<string, string> args) {
            var client = DoBuildHttpClient();

            var postData = new HttpFormUrlEncodedContent(args);
            
            using (var rsp = await client.PostAsync(new Uri(_url), postData)) {
                return new AsyncHttpResponse(rsp, _encoding);
            }
        }

        private HttpClient DoBuildHttpClient() {
            if (string.IsNullOrEmpty(_encoding)) {
                _encoding = "UTF-8";
            }

            HttpClient client = null;
            if (_cookies != null) {
                var bpf = new HttpBaseProtocolFilter();
                var cm = bpf.CookieManager;
                foreach (var kv in _cookies) {
                    var cookie = new HttpCookie(kv.Key, _url, "/") { Value = kv.Value };
                    cm.SetCookie(cookie);
                }

                client = new HttpClient(bpf);
            }
            else {
                client = new HttpClient();
            }

            client.DefaultRequestHeaders.Add("Encoding", encoding);

            if (_headers != null) {
                foreach (var kv in _headers) {
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                }
            }

            return client;
        }
    }
}
