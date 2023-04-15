global using AWO.Events;
global using BepInEx.Unity.IL2CPP.Utils.Collections;
global using Il2CppInterop.Runtime.Attributes;
global using WOManager = WardenObjectiveManager;
using AWO.Modules.WEE;
using AWO.Modules.WOE;
using AWO.Sessions;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using GTFO.API;
using HarmonyLib;

namespace AWO;

[BepInPlugin("GTFO.AWO", "AWO", VersionInfo.Version)]
[BepInDependency("GTFO.InjectLib", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
internal class EntryPoint : BasePlugin
{
    public unsafe override void Load()
    {
        WardenEventExt.Initialize();
        WardenObjectiveExt.Initialize();

        new Harmony("AWO.Harmony").PatchAll();

        AssetAPI.OnStartupAssetsLoaded += AssetAPI_OnStartupAssetsLoaded;
    }

    private void AssetAPI_OnStartupAssetsLoaded()
    {
        BlackoutState.AssetLoaded();
        LevelFailUpdateState.AssetLoaded();
    }
}