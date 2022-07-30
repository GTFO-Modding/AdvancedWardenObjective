using Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Jsons
{
    [JsonConverter(typeof(LocaleTextConverter))]
    public struct LocaleText
    {
        public uint ID;
        public string RawText;

        public LocaleText(string text)
        {
            RawText = text;
            ID = 0;
        }

        public LocaleText(uint id)
        {
            RawText = string.Empty;
            ID = id;
        }

        public string Translated
        {
            get
            {
                if (ID != 0)
                {
                    return Text.Get(ID);
                }
                else
                {
                    return RawText;
                }
            }
        }

        public LocalizedText ToLocalizedText()
        {
            if (ID != 0)
            {
                return new LocalizedText()
                {
                    Id = ID,
                    UntranslatedText = string.Empty
                };
            }
            else
            {
                return new LocalizedText()
                {
                    Id = 0,
                    UntranslatedText = RawText
                };
            }
        }

        public static readonly LocaleText Empty = new(string.Empty);
    }
}
