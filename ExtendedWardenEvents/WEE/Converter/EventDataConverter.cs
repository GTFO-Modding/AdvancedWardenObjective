using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.Jsons.ManagedJson;
using GameData;
using Il2CppInterop.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace ExtendedWardenEvents.WEE.Converter
{
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
                    WEEDataHolder.PushWEEData(result, extData);
                }
                return result;
            }
            else
            {
                Logger.Debug($"INVALID TOKEN: {reader.TokenType}");
                return existingValue;
            }
        }

        private static WardenObjectiveEventData ParseJson(string json, out bool hasExtData, out WEE_EventData extData)
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
                                if (Enum.IsDefined((WEEType)id))
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
                                if (Enum.TryParse<WEEType>(strValue, ignoreCase: true, out var extType))
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
                return new WardenObjectiveEventData()
                {
                    Type = (eWardenObjectiveEventType)(int)extData.Type
                };

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
}
