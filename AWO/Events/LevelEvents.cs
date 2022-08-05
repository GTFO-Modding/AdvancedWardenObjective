using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Events
{
    internal static class LevelEvents
    {
        public static event Action OnLevelBuildDone;
        public static event Action OnLevelBuildDoneLate;
        public static event Action OnLevelCleanup;


        internal static void Invoke_OnLevelCleanup() => OnLevelCleanup?.Invoke();
        internal static void Invoke_OnLevelBuildDone() => OnLevelBuildDone?.Invoke();
        internal static void Invoke_OnLevelBuildDoneLate() => OnLevelBuildDoneLate?.Invoke();
    }
}
