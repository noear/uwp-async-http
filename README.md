# uwp-async-http

觉得Windows.Web.Http在涉及Cookie及编码时使用比较麻烦
所以就封装了一个

```java
//GET DEMO
var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
    .Encoding("UTF-8")
    .Get();

return rsp.GetString();

//GET AND HttpHeader,Cookie DEMO
var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
    .Header("Referer",referer)
    .Header("User-Agent","xxxxxxxxxx")
    .Cookie("userID","1")
    .Get();

return rsp.GetBytes();

//POST DEMO
Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("OrderID", "1");

            var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
                .Post(postData);

            return rsp.GetString();

```
