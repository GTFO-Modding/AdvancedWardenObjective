using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE.ReactorStartups;

internal sealed class WOE_ReactorStartupContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.Reactor_Startup;
}
