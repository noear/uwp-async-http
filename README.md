# uwp-async-http

Windows.Web.Http 的简化封装

在使用Windows.Web.Http时，觉得涉及Cookie及编码操作时比较麻烦
所以就封装了AsyncHttpClient

文件结构：
```javascript
Noear.UWP.Http{
    AsyncHttpClient
    AsyncHttpResponse
}
```

示例代码：
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

//Response DEMO
Dictionary<string, string> postData = new Dictionary<string, string>();
postData.Add("OrderID", "1");

var rsp = await new AsyncHttpClient().Url("http://api.xxx.ddd/get")
    .Post(postData);

if (rsp.StatusCode == HttpStatusCode.Ok) {
    string text = rsp.GetString();
    string name = rsp.Headers["name"];
    string cokie = rsp.Cookies;
}

```
