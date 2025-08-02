using Newtonsoft.Json;

public static class ObjectExtension
{
    public static T CloneInstance<T>(this T obj) where T : class
    {
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(json);
    }
}