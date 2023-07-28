using AWO.Networking;
using LevelGeneration;

namespace AWO.Sessions;

internal struct BlackoutStatus
{
    public bool blackoutEnabled;
}

internal static class BlackoutState
{
    public static bool BlackoutEnabled { get; private set; } = false;

    private static StateReplicator<BlackoutStatus> _Replicator;

    internal static void AssetLoaded()
    {
        if (_Replicator != null)
            return;

        _Replicator = StateReplicator<BlackoutStatus>.Create(1u, new() { blackoutEnabled = false }, LifeTimeType.Permanent);
        _Replicator.OnStateChanged += OnStateChanged;
        LevelEvents.OnLevelCleanup += LevelCleanup;
    }

    private static void LevelCleanup()
    {
        SetEnabled(false);
    }

    public static void SetEnabled(bool enabled)
    {
        _Replicator.SetState(new() { blackoutEnabled = enabled });
    }

    private static void OnStateChanged(BlackoutStatus _, BlackoutStatus state, bool isRecall)
    {
        var isNormal = !state.blackoutEnabled;

        foreach (var display in LG_Objects.LabDisplays)
        {
            if (display == null || display.m_Text == null)
                continue;

            display.m_Text.enabled = isNormal;
        }

        foreach (var terminal in LG_Objects.Terminals)
        {
            if (terminal == null)
                continue;

            // help terminals not brick
            terminal.OnProximityExit();

            var interaction = terminal.GetComponentInChildren<Interact_ComputerTerminal>(includeInactive: true);
            if (interaction != null) interaction.enabled = isNormal;

            var guixSceneLink = terminal.GetComponent<GUIX_VirtualSceneLink>();
            GUIX_VirtualScene guix = null;
            if (guixSceneLink != null) guix = guixSceneLink.m_virtualScene;

            if (guix != null) guix.gameObject.SetActive(isNormal);

            if (terminal.m_text != null) terminal.m_text.gameObject.SetActive(isNormal);
        }

        foreach (var doorButton in LG_Objects.DoorButtons)
        {
            if (doorButton == null)
                continue;

            doorButton.m_anim.gameObject.SetActive(isNormal);
            doorButton.m_enabled = isNormal;

            if (isNormal)
            {
                var weaklock = doorButton.GetComponentInChildren<LG_WeakLock>();
                if (weaklock == null) doorButton.m_enabled = true;
                else if (weaklock.Status == eWeakLockStatus.Unlocked) doorButton.m_enabled = true;
            }
        }

        foreach (var locks in LG_Objects.WeakLocks)
        {
            if (locks == null)
                continue;

            locks.m_intHack.m_isActive = isNormal;

            var display = locks.transform.FindChild("HackableLock/SecurityLock/g_WeakLock/Security_Display_Locked");
            if (display != null) display.gameObject.active = isNormal;
            else
            {
                display = locks.transform.FindChild("HackableLock/Security_Display_Locked");
                if (display != null) display.gameObject.active = isNormal;
            }
        }

        foreach (var activator in LG_Objects.HSUActivators)
        {
            if (activator == null)
                continue;

            if (!activator.m_isWardenObjective)
                continue;

            if (activator.m_stateReplicator.State.status == eHSUActivatorStatus.WaitingForInsert)
            {
                activator.m_insertHSUInteraction.SetActive(isNormal);

                foreach (var obj in activator.m_activateWhenActive)
                {
                    obj.SetActive(isNormal);
                }
            }
        }

        BlackoutEnabled = state.blackoutEnabled;
    }
}
