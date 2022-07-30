using Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents
{
    internal static class LocalizedTextExtension
    {
        public static string ToText(this LocalizedText text)
        {
            if (!text.HasTranslation)
            {
                return text.UntranslatedText;
            }
            return Text.Get(text.Id);
        }
    }
}
