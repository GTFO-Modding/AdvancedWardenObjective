namespace ExtendedWardenEvents.WEE
{
    internal enum WEEType
    {
        CloseSecurityDoor //Done
            = WEEEnumInjector.ExtendedIndex + 0,

        LockSecurityDoor,
        SetDoorInteraction,
        TriggerSecurityDoorAlarm, //Done
        SolveSecurityDoorAlarm, //TODO: Partially works
        StartReactor, //Done
        JumpToReactorWave,
        ForceCompleteReactor,
        ForceCompleteLevel,
        ForceFailLevel,
        Countdown,
        KillPlayersWithForceComplete,
        KillPlayersWithForceFail,
        SolveSingleObjectiveItem,
        SetLightDataInZone,
        AlertEnemiesInZone,
        DespawnEnemiesInZone,
        KillEnemiesInZone,
        SaveCheckpoint,
        MoveExtractionWorldPosition
    }
}
