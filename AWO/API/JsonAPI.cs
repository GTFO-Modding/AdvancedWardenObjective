using AWO.WEE.Converter;
using System.Text.Json.Serialization;

namespace AWO.API;

public static class JsonAPI
{
    public static JsonConverter EventDataConverter => new ExternalEventDataConverter();
}
