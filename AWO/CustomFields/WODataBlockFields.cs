using AWO.Modules.WOE;
using GameData;
using InjectLib.FieldInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.CustomFields;
internal static class WODataBlockFields
{
    static WODataBlockFields()
    {
        FieldInjector<WardenObjectiveDataBlock>.DefineManagedField<WOE_DataBase>("m_WOEDataRef");
    }

    public static void SetWEEData(this WardenObjectiveDataBlock target, WOE_DataBase data)
    {
        FieldInjector<WardenObjectiveDataBlock>.TrySetManagedField(target, "m_WOEDataRef", data);
    }

    public static WOE_DataBase GetWEEData(this WardenObjectiveDataBlock target)
    {
        FieldInjector<WardenObjectiveDataBlock>.TryGetManagedField<WOE_DataBase>(target, "m_WOEDataRef", out var data);
        return data;
    }
}
