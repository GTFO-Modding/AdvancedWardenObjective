using System;

namespace AWO.Modules.WOE.ReactorStartups;

internal sealed class WOE_ReactorStartupContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.Reactor_Startup;

    public override Type DataType => throw new NotImplementedException();
}
