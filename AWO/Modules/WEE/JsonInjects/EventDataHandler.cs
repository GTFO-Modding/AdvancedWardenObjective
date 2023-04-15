using AWO.CustomFields;
using AWO.Jsons.ManagedJson;
using AWO.Modules.WEE;
using GameData;
using Il2CppJsonNet.Linq;
using InjectLib.FieldInjection;
using InjectLib.JsonNETInjection.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using JSON = GTFO.API.JSON.JsonSerializer;

namespace AWO.WEE.JsonInjects;
internal class EventDataHandler : Il2CppJsonReferenceTypeHandler<WardenObjectiveEventData>
{
    private readonly static JsonSerializerOptions _JsonOption;
    static EventDataHandler()
    {
        _JsonOption = new JsonSerializerOptions(JSON.DefaultSerializerSettings);
        _JsonOption.Converters.Add(new LocalizedTextConverter());
    }

    public override void OnRead(in Il2CppSystem.Object result, in JToken jToken)
    {
        var data = result.Cast<WardenObjectiveEventData>();
        if (Enum.IsDefined((WEE_Type)data.Type))
        {
            var extData = JSON.Deserialize<WEE_EventData>(jToken.ToString(), _JsonOption);
            data.SetWEEData(extData);
        }
    }
}
