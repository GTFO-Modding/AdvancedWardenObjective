using BepInEx;
using BepInEx.IL2CPP;
using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.WEE;

namespace ExtendedWardenEvents
{
    [BepInPlugin("WardenEventExtension", "EventsExt", VersionInfo.Version)]
    internal class EntryPoint : BasePlugin
    {
        public unsafe override void Load()
        {
            WardenEventExt.Initialize();
            Il2CppJsonConverters.Initialize();
        }
    }
}