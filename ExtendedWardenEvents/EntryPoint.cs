global using BepInEx.IL2CPP.Utils.Collections;
global using Il2CppInterop.Runtime.Attributes;
global using WOManager = WardenObjectiveManager;

using BepInEx;
using BepInEx.IL2CPP;
using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.Sessions;
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

            new Harmony("EventsExt.Harmony").PatchAll();

            AssetAPI.OnStartupAssetsLoaded += AssetAPI_OnStartupAssetsLoaded;
        }

        private void AssetAPI_OnStartupAssetsLoaded()
        {
            BlackoutState.AssetLoaded();
            LevelFailUpdateState.AssetLoaded();
        }
    }
}