using AWO.Networking;
using AWO.Networking.CommonReplicator.Inject;
using LevelGeneration;
using System;

namespace AWO.Sessions;

internal struct LevelFailCheck
{
    public bool failAllowed;
}

internal sealed class LevelFailUpdateState
{
    private static StateReplicator<LevelFailCheck> _Replicator;

    internal static void AssetLoaded()
    {
        if (_Replicator != null)
            return;

        _Replicator = StateReplicator<LevelFailCheck>.Create(1u, new() { failAllowed = true }, LifeTimeType.Permanent);
        LG_Factory.add_OnFactoryBuildStart(new Action(() =>
        {
            _Replicator.ClearAllRecallSnapshot();
            _Replicator.SetState(new()
            {
                failAllowed = true
            });
        }));
        _Replicator.OnStateChanged += OnStateChanged;
        LevelEvents.OnLevelCleanup += LevelCleanup;
    }

    private static void LevelCleanup()
    {
        SetFailAllowed(true);
    }

    public static void SetFailAllowed(bool allowed)
    {
        _Replicator.SetState(new()
        {
            failAllowed = allowed
        });
    }

    private static void OnStateChanged(LevelFailCheck _, LevelFailCheck state, bool __)
    {
        Inject_LevelFailCheck.LevelFailAllowed = state.failAllowed;
    }
}
