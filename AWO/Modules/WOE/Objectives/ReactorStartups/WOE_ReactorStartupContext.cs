using System;

namespace AWO.Modules.WOE.Objectives.ReactorStartups;

internal sealed class WOE_ReactorStartupContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.Reactor_Startup;

    public override Type DataType => typeof(WOE_ReactorStartupData);
}
