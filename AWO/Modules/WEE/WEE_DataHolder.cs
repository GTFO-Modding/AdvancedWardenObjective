using GameData;
using System.Collections.Generic;

namespace AWO.Modules.WEE;

internal static class WEE_DataHolder
{
    private readonly static Dictionary<int, DataSet> _Lookup = new();
    private static int _Key = int.MinValue;

    public static void PushWEEData(WardenObjectiveEventData original, WEE_EventData data)
    {
        _Lookup[_Key] = new DataSet()
        {
            OriginalData = original,
            EventData = data
        };

        original.Count = _Key;

        _Key++;
    }

    public static bool TryGetWEEData(WardenObjectiveEventData original, out WEE_EventData data)
    {
        if (_Lookup.TryGetValue(original.Count, out var dataSet))
        {
            data = dataSet.EventData;
            return data != null;
        }
        data = null;
        return false;
    }

    private class DataSet
    {
        public WardenObjectiveEventData OriginalData;
        public WEE_EventData EventData;
    }
}
