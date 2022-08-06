using Newtonsoft.Json;
using System;

namespace AWO.Jsons.Il2CppJson;

public interface INativeJsonConverter
{
    public string Name { get; }
    public Il2CppSystem.Object ReadJson(JsonReader reader, IntPtr existingValue, JsonSerializer serializer);
    public void WriteJson(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer);
}

public abstract class Il2CppJsonReferenceTypeConverter<T> : INativeJsonConverter where T : Il2CppSystem.Object
{
    public virtual string Name => GetType().Name;

    public Il2CppSystem.Object ReadJson(JsonReader reader, IntPtr existingValue, JsonSerializer serializer)
    {
        if (existingValue == IntPtr.Zero)
        {
            return ReadJson(reader, null, serializer);
        }
        var wrappedValue = (T)Activator.CreateInstance(typeof(T), existingValue);
        return ReadJson(reader, wrappedValue, serializer);
    }

    public void WriteJson(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer)
    {
        if (valueToWrite == IntPtr.Zero)
        {
            WriteJson(writer, null, serializer);
            return;
        }
        var wrappedValue = (T)Activator.CreateInstance(typeof(T), valueToWrite);
        WriteJson(writer, wrappedValue, serializer);
    }

    protected abstract T ReadJson(JsonReader reader, T existingValue, JsonSerializer serializer);
    protected abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);
}
