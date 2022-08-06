using AWO.Modules.WEE;
using GTFO.API.Utilities;
using System.Collections;
using UnityEngine;

namespace AWO.WEE.Events.HUD;

internal sealed class CountdownEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.Countdown;

    protected override void TriggerCommon(WEE_EventData e)
    {
        CoroutineDispatcher.StartCoroutine(DoCountdown(e));
    }

    static IEnumerator DoCountdown(WEE_EventData e)
    {
        var cd = e.Countdown;
        var duration = cd.Duration;
        GuiManager.PlayerLayer.m_objectiveTimer.SetTimerActive(true, true);
        GuiManager.PlayerLayer.m_objectiveTimer.UpdateTimerTitle(cd.TimerText.ToString());
        GuiManager.PlayerLayer.m_objectiveTimer.SetTimerTextEnabled(true);

        var time = 0.0f;
        while (time <= duration)
        {
            if (GameStateManager.CurrentStateName != eGameStateName.InLevel)
            {
                break;
            }

            GuiManager.PlayerLayer.m_objectiveTimer.UpdateTimerText(duration - time, duration, cd.TimerColor);
            time += Time.deltaTime;
            yield return null;
        }

        GuiManager.PlayerLayer.m_objectiveTimer.SetTimerActive(false, false);
    }
}
