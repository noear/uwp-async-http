using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noear.UWP.Http {
    class _demo {
        public async Task<string> get_string() {
            var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
                .Encoding("UTF-8")
                .Get();

            return rsp.GetString();
        }

        public async Task<byte[]> get_header(string referer) {
            var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
                .Header("Referer",referer)
                .Header("User-Agent","xxxxxxxxxx")
                .Cookie("userID","1")
                .Get();

            return rsp.GetBytes();
        }

        public async Task<string> post_string() {

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("OrderID", "1");

            var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
                .Post(postData);

            return rsp.GetString();
        }
    }
}
