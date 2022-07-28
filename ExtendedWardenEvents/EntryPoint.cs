using BepInEx;
using BepInEx.IL2CPP;
using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.WEE;

namespace ExtendedWardenEvents
{
    [BepInPlugin("WardenEventExtension", "EventsExt", VersionInfo.Version)]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    internal class EntryPoint : BasePlugin
    {
        public unsafe override void Load()
        {
            WardenEventExt.Initialize();
            Il2CppJsonConverters.Initialize();
        }
    }
}