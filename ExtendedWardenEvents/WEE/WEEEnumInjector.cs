using GameData;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;

namespace ExtendedWardenEvents.WEE
{
    internal static class WEEEnumInjector
    {
        public const int ExtendedIndex = 10000;
        private readonly static Dictionary<string, object> _EventTypes = new();
        private static int _CurrentIndex = 0;


        static WEEEnumInjector()
        {
            foreach (var value in Enum.GetValues<WEEType>())
            {
                var name = value.ToString();
                AddEvent(name);
                Logger.Debug($"Injecting EWOEvent: '{name}'");
            }
        }

        private static void AddEvent(string name)
        {
            _EventTypes[name] = _CurrentIndex + ExtendedIndex;
            _CurrentIndex++;
        }

        internal static void Inject()
        {
            EnumInjector.InjectEnumValues<eWardenObjectiveEventType>(_EventTypes);
        }
    }
}
