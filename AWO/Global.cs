global using AWO.Events;
global using AWO.Utils;
global using AWO.CustomFields;
global using BepInEx.Unity.IL2CPP.Utils.Collections;
global using Il2CppInterop.Runtime.Attributes;
global using WOManager = WardenObjectiveManager;

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "UNT0026:GetComponent always allocates / Use TryGetComponent", Justification = "TryGetComponent is broken in GTFO Il2Cpp Environment")]
