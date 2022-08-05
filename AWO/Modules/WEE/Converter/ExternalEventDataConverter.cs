﻿using AWO.Modules.WEE;
using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AWO.WEE.Converter
{
    public sealed class ExternalEventDataConverter : JsonConverter<WardenObjectiveEventData>
    {
        public override WardenObjectiveEventData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var doc = JsonDocument.ParseValue(ref reader);

                var json = doc.RootElement.GetRawText();
                var result = EventDataConverter.ParseJson(json, out var hasExtData, out var extData);
                if (hasExtData)
                {
                    WEEDataHolder.PushWEEData(result, extData);
                }
                return result;
            }
            else
            {
                Logger.Debug($"INVALID TOKEN: {reader.TokenType}");
                return new WardenObjectiveEventData()
                {
                    Type = eWardenObjectiveEventType.None
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, WardenObjectiveEventData value, JsonSerializerOptions options)
        {
            
        }
    }
}