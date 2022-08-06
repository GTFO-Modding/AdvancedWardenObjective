using BepInEx.IL2CPP.Hook;
using GTFO.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace AWO.Jsons.Il2CppJson.Detour;

internal static class Detour_RerouteJSON_ColorConverter
{
    public unsafe delegate IntPtr ReadJsonDel(IntPtr _this, IntPtr reader, IntPtr objectType, IntPtr existingValue, IntPtr serializer);
    public unsafe delegate void WriteJsonDel(IntPtr _this, IntPtr writer, IntPtr value, IntPtr serializer);

    private static INativeDetour _ReadDetour, _WriteDetour;
    private static ReadJsonDel _ReadOriginal;
    private static WriteJsonDel _WriteOriginal;


    public unsafe static void Patch()
    {
        var readmethod = Il2CppAPI.GetIl2CppMethod<ColorConverter>(
            nameof(ColorConverter.ReadJson),
            "System.Object",
            isGeneric: false,
            new string[]
            {
                "Newtonsoft.Json.JsonReader",
                "System.Type",
                "System.Object",
                "Newtonsoft.Json.JsonSerializer"
            });

        var writemethod = Il2CppAPI.GetIl2CppMethod<ColorConverter>(
            nameof(ColorConverter.WriteJson),
            typeof(void).FullName,
            isGeneric: false,
            new string[]
            {
                "Newtonsoft.Json.JsonWriter",
                "System.Object",
                "Newtonsoft.Json.JsonSerializer"
            });

        _ReadDetour = INativeDetour.CreateAndApply((nint)readmethod, Detour_Read, out _ReadOriginal);
        _WriteDetour = INativeDetour.CreateAndApply((nint)writemethod, Detour_Write, out _WriteOriginal);
    }

    private unsafe static IntPtr Detour_Read(IntPtr _this, IntPtr reader, IntPtr objectType, IntPtr existingValue, IntPtr serializer)
    {
        if (Il2CppJsonConverters.TryGetConverterForType(objectType, out var converter))
        {
            var result = converter.ReadJson(new JsonReader(reader), existingValue, new JsonSerializer(serializer));
            return result.Pointer;
        }
        return _ReadOriginal(_this, reader, objectType, existingValue, serializer);
    }

    private unsafe static void Detour_Write(IntPtr _this, IntPtr writer, IntPtr value, IntPtr serializer)
    {
        if (Il2CppJsonConverters.IsRerouteConverter(_this, out var objectType))
        {
            if (Il2CppJsonConverters.TryGetConverterForType(objectType, out var converter))
            {
                converter.WriteJson(new JsonWriter(writer), value, new JsonSerializer(serializer));
                return;
            }
        }

        _WriteOriginal(_this, writer, value, serializer);
    }
}
