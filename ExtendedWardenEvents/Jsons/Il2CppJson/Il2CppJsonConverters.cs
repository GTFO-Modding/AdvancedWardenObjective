using ExtendedWardenEvents.Jsons.Il2CppJson.Detour;
using Il2CppInterop.Runtime;
using System;
using System.Collections.Generic;

namespace ExtendedWardenEvents.Jsons.Il2CppJson
{
    public static class Il2CppJsonConverters
    {
        private static readonly Dictionary<IntPtr, IntPtr> _ConverterCanConvertTypeOf = new();
        private static readonly Dictionary<IntPtr, INativeJsonConverter> _RegisteredConverters = new();

        internal static void Initialize()
        {
            Detour_RerouteJSON_ConverterInjector.Patch();
            Detour_RerouteJSON_ColorConverter.Patch();
        }

        public static void RegisterConverter<T>(Il2CppJsonReferenceTypeConverter<T> converter) where T : Il2CppSystem.Object
        {
            var typePtr = Il2CppType.Of<T>().Pointer;

            if (typePtr != IntPtr.Zero)
            {
                _RegisteredConverters[typePtr] = converter;
            }
            else
            {
                Logger.Error($"Type: '{typeof(T).Name}' was not registered to il2cpp domain!");
            }
        }

        public static bool TryGetConverterForType(IntPtr typePtr, out INativeJsonConverter converter)
        {
            return _RegisteredConverters.TryGetValue(typePtr, out converter);
        }

        public static bool IsRerouteConverter(IntPtr converter, out IntPtr convertableType)
        {
            return _ConverterCanConvertTypeOf.TryGetValue(converter, out convertableType);
        }

        internal static void RegisterNativeConverter(IntPtr converter, IntPtr convertableType)
        {
            _ConverterCanConvertTypeOf[converter] = convertableType;
        }
    }
}
