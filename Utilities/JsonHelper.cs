using System.Text;
using Newtonsoft.Json;
using UnityEngine;

internal class JsonHelper
{
    public static string ToJson(object x)
    {
        var json = JsonConvert.SerializeObject(x);
        return json;
        //string str = JsonMapper.ToJson(x);
        //return str;
    }

    public static T ToObject<T>(string x)
    {
        Debug.Log(x);
        return JsonConvert.DeserializeObject<T>(x);
        //return JsonMapper.ToObject<T>(x);
    }

    public static T ToObject<T>(byte[] b)
    {
        string str = Encoding.UTF8.GetString(b, 0, b.Length);
        return ToObject<T>(str);
    }

}