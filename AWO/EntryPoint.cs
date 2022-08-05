global using BepInEx.IL2CPP.Utils.Collections;
global using Il2CppInterop.Runtime.Attributes;
global using AWO.Events;
global using WOManager = WardenObjectiveManager;

using BepInEx;
using BepInEx.IL2CPP;
using AWO.Jsons.Il2CppJson;
using AWO.Sessions;
using GTFO.API;
using HarmonyLib;
using AWO.Modules.WEE;

namespace AWO
{
    [BepInPlugin("GTFO.AWO", "AWO", VersionInfo.Version)]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    internal class EntryPoint : BasePlugin
    {
        public unsafe override void Load()
        {
            WardenEventExt.Initialize();
            Il2CppJsonConverters.Initialize();

            new Harmony("AWO.Harmony").PatchAll();

            AssetAPI.OnStartupAssetsLoaded += AssetAPI_OnStartupAssetsLoaded;
        }

        private void AssetAPI_OnStartupAssetsLoaded()
        {
            BlackoutState.AssetLoaded();
            LevelFailUpdateState.AssetLoaded();
        }
    }
}