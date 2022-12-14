using AWO.Jsons.Il2CppJson;
using AWO.Jsons.ManagedJson;
using AWO.Modules.WEE;
using GameData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace AWO.WEE.Converter;

using JSON = GTFO.API.JSON.JsonSerializer;

internal class EventDataConverter : Il2CppJsonReferenceTypeConverter<WardenObjectiveEventData>
{
    private readonly static JsonSerializerOptions _JsonOption;

    static EventDataConverter()
    {
        _JsonOption = new JsonSerializerOptions(JSON.DefaultSerializerSettings);
        _JsonOption.Converters.Add(new LocalizedTextConverter());
    }

    protected override WardenObjectiveEventData ReadJson(JsonReader reader, WardenObjectiveEventData existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            var json = JObject.Load(reader).ToString();
            var result = ParseJson(json, out var hasExtData, out var extData);
            if (hasExtData)
            {
                WEE_DataHolder.PushWEEData(result, extData);
            }
            return result;
        }
        else
        {
            Logger.Debug($"INVALID TOKEN: {reader.TokenType}");
            return existingValue;
        }
    }

    public static WardenObjectiveEventData ParseJson(string json, out bool hasExtData, out WEE_EventData extData)
    {
        try
        {
            using var doc = JsonDocument.Parse(json.ToLowerInvariant());

            if (doc.RootElement.TryGetProperty("type", out var typeProp))
            {
                switch (typeProp.ValueKind)
                {
                    case JsonValueKind.Number:
                        if (typeProp.TryGetInt32(out var id))
                        {
                            if (Enum.IsDefined((WEE_Type)id))
                            {
                                goto DeserializeToExtData;
                            }
                            else
                            {
                                goto DeserializeToDefaultData;
                            }
                        }
                        break;

                    case JsonValueKind.String:
                        var strValue = typeProp.GetString();
                        if (!string.IsNullOrWhiteSpace(strValue))
                        {
                            if (Enum.TryParse<WEE_Type>(strValue, ignoreCase: true, out var extType))
                            {
                                goto DeserializeToExtData;
                            }
                            else if (Enum.TryParse<eWardenObjectiveEventType>(strValue, ignoreCase: true, out var type))
                            {
                                goto DeserializeToDefaultData;
                            }
                        }
                        break;
                }
            }
            else
            {
                goto DeserializeToDefaultData;
            }
            hasExtData = false;
            extData = null;
            return new WardenObjectiveEventData()
            {
                Type = eWardenObjectiveEventType.None
            };


        DeserializeToExtData:
            extData = JSON.Deserialize<WEE_EventData>(json, _JsonOption);
            hasExtData = extData != null;
            return extData.CreateDummyEventData();

        DeserializeToDefaultData:
            var data = JSON.Deserialize<WardenObjectiveEventData>(json, _JsonOption);
            hasExtData = false;
            extData = null;
            return data;
        }
        catch
        {
            hasExtData = false;
            extData = null;
            return new WardenObjectiveEventData();
        }
    }

    protected override void WriteJson(JsonWriter writer, WardenObjectiveEventData value, Newtonsoft.Json.JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
