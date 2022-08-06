using BepInEx.IL2CPP.Hook;
using GTFO.API;
using Il2CppInterop.Runtime.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace AWO.Jsons.Il2CppJson.Detour;

internal static class Detour_RerouteJSON_ConverterInjector
{
    public unsafe delegate IntPtr GetMatchingConverterDel(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo);
    private static INativeDetour _Detour;
    private static GetMatchingConverterDel _Original;

    public unsafe static void Patch()
    {
        var method = Il2CppAPI.GetIl2CppMethod<JsonSerializer>(
            nameof(JsonSerializer.GetMatchingConverter),
            typeof(JsonConverter).FullName,
            isGeneric: false,
            new string[]
            {
                "System.Collections.Generic.IList<Newtonsoft.Json.JsonConverter>",
                typeof(Type).FullName
            });

        _Detour = INativeDetour.CreateAndApply((nint)method, Detour, out _Original);
    }

    private unsafe static IntPtr Detour(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo)
    {
        if (Il2CppJsonConverters.TryGetConverterForType(objectType, out var converter))
        {
            Logger.Debug($"Type: '{new Il2CppSystem.Type(objectType).Name}' now Convert by '{converter.Name}'!");
            var converterProxy = new ColorConverter();
            GC.KeepAlive(converterProxy);

            var ptr = converterProxy.Pointer;
            Il2CppJsonConverters.RegisterNativeConverter(ptr, objectType);
            return ptr;
        }


        return IntPtr.Zero;
    }
}
