using AWO.Networking;
using AWO.Networking.CommonReplicator.Inject;
using LevelGeneration;
using System;

namespace AWO.Sessions;

internal enum LevelFailMode
{
    Default,
    Never,
    AnyPlayerDown
}

internal struct LevelFailCheck
{
    public LevelFailMode mode;
}

internal sealed class LevelFailUpdateState
{
    private static StateReplicator<LevelFailCheck> _Replicator;

    internal static void AssetLoaded()
    {
        if (_Replicator != null)
            return;

        _Replicator = StateReplicator<LevelFailCheck>.Create(1u, new() { mode = LevelFailMode.Default }, LifeTimeType.Permanent);
        LG_Factory.add_OnFactoryBuildStart(new Action(() =>
        {
            _Replicator.ClearAllRecallSnapshot();
            _Replicator.SetState(new()
            {
                mode = LevelFailMode.Default
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
            mode = allowed ? LevelFailMode.Default : LevelFailMode.Never
        });
    }

    public static void SetFailWhenAnyPlayerDown(bool enabled)
    {
        _Replicator.SetState(new()
        {
            mode = enabled ? LevelFailMode.AnyPlayerDown : LevelFailMode.Default
        });
    }

    private static void OnStateChanged(LevelFailCheck _, LevelFailCheck state, bool __)
    {
        switch (state.mode)
        {
            case LevelFailMode.Default:
                Inject_LevelFailCheck.LevelFailAllowed = true;
                Inject_LevelFailCheck.LevelFailWhenAnyPlayerDown = false;
                break;

            case LevelFailMode.Never:
                Inject_LevelFailCheck.LevelFailAllowed = false;
                Inject_LevelFailCheck.LevelFailWhenAnyPlayerDown = false;
                break;

            case LevelFailMode.AnyPlayerDown:
                Inject_LevelFailCheck.LevelFailAllowed = true;
                Inject_LevelFailCheck.LevelFailWhenAnyPlayerDown = true;
                break;
        }
    }
}
