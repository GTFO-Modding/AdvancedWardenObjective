using System.Linq;

namespace AWO.Modules.WOE;

public static class WardenObjectiveExt
{
    static WardenObjectiveExt()
    {
        var contextTypes = typeof(WOE_ContextBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(WOE_ContextBase)));
    }
}
