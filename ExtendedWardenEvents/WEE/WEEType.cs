namespace ExtendedWardenEvents.WEE
{
    public enum WEEType
    {
        CloseSecurityDoor //Done
            = WEEEnumInjector.ExtendedIndex + 0,

        LockSecurityDoor, //Done
        SetDoorInteraction,
        TriggerSecurityDoorAlarm, //Done
        SolveSecurityDoorAlarm, //TODO: Partially works
        StartReactor, //Done
        ModifyReactorWaveState, //Done
        ForceCompleteReactor, //Done
        ForceCompleteLevel, //Done
        ForceFailLevel, //Done
        Countdown, //Done
        KillPlayersWithForceComplete,
        KillPlayersWithForceFail,
        SolveSingleObjectiveItem, //Done
        SetLightDataInZone, //TODO: Partially Done, Need to Work on Animation
        AlertEnemiesInZone, //Done
        CleanupEnemiesInZone, //Done, Kill, Despawn Has Merged with this
        SaveCheckpoint, //Done
        MoveExtractionWorldPosition //Done
    }
}
