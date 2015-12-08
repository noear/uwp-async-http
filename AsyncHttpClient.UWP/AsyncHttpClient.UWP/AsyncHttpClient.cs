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
            _headers[name] = value;

            return this;
        }
        
        public AsyncHttpClient Cookie(string name, string value) {
            if (_cookies == null) {
                _cookies = new Dictionary<string, string>();
            }
            _cookies[name] = value;

            return this;
        }

        public AsyncHttpClient Cookies(string cookies) {
            if (string.IsNullOrEmpty(cookies) == false) {
                foreach (var kv in cookies.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
                    if (kv.IndexOf('=') > 0) {
                        var name = kv.Split('=')[0].Trim();
                        var value = kv.Split('=')[1].Trim();
                        Cookie(name, value);
                    }
                }
            }

            return this;
        }



        public async Task<AsyncHttpResponse> Get() {
            var client = DoBuildHttpClient();

            try {
                using (var rsp = await client.GetAsync(new Uri(_url))) {
                    return new AsyncHttpResponse(rsp, _encoding);
                }
            }
            catch (Exception ex) {
                return new AsyncHttpResponse(ex, _encoding);
            }
        }
        
        public async Task<AsyncHttpResponse> Post(Dictionary<string, string> args) {
            var client = DoBuildHttpClient();

            var postData = new HttpFormUrlEncodedContent(args);

            try {
                using (var rsp = await client.PostAsync(new Uri(_url), postData)) {
                    return new AsyncHttpResponse(rsp, _encoding);
                }
            }
            catch (Exception ex) {
                return new AsyncHttpResponse(ex, _encoding);
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

                string domain = null;
                if (_cookies.ContainsKey("Domain"))
                    domain = _cookies["Domain"];
                else if (_cookies.ContainsKey("domain"))
                    domain = _cookies["domain"];
                else
                    domain = new Uri(_url).Host;

                foreach (var kv in _cookies) {
                    var key = kv.Key.ToLower();
                    if (key == "domain" || key == "path" || key == "expires")
                        continue;

                    var cookie = new HttpCookie(kv.Key.Trim(), domain, "/") { Value = kv.Value.Trim() };
                    cm.SetCookie(cookie);
                }

                client = new HttpClient(bpf);
            }
            else {
                client = new HttpClient();
            }

            client.DefaultRequestHeaders.Add("Encoding", _encoding);
           

            if (_headers != null) {
                foreach (var kv in _headers) {
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                }
            }

            return client;
        }
    }
}
