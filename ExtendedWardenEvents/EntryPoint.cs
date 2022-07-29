using BepInEx;
using BepInEx.IL2CPP;
using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.Networking.Test;
using ExtendedWardenEvents.WEE;
using GTFO.API;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

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

            ClassInjector.RegisterTypeInIl2Cpp<LightStateHolder>();

            new Harmony("EventsExt.Harmony").PatchAll();

            AssetAPI.OnStartupAssetsLoaded += AssetAPI_OnStartupAssetsLoaded;
        }

        private void AssetAPI_OnStartupAssetsLoaded()
        {
            var obj = new GameObject();
            var holder = obj.AddComponent<LightStateHolder>();
            UnityEngine.Object.DontDestroyOnLoad(obj);
        }
    }
}