using Localization;
using System.Text.Json.Serialization;

namespace AWO.Jsons;

[JsonConverter(typeof(LocaleTextConverter))]
public struct LocaleText
{
    public uint ID;
    public string RawText;

    public LocaleText(LocalizedText baseText)
    {
        if (baseText.HasTranslation)
        {
            RawText = string.Empty;
            ID = baseText.Id;
        }
        else
        {
            RawText = baseText.UntranslatedText;
            ID = 0u;
        }
    }

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
            return ID == 0 ? RawText : Text.Get(ID);
        }
    }

    public LocalizedText ToLocalizedText()
    {
        return ID == 0
            ? new LocalizedText()
            {
                Id = 0,
                UntranslatedText = RawText
            }
            : new LocalizedText()
            {
                Id = ID,
                UntranslatedText = string.Empty
            };
    }

    public override string ToString()
    {
        return Translated;
    }

    public static explicit operator LocaleText(LocalizedText localizedText) => new(localizedText);
    public static explicit operator LocaleText(string text) => new(text);

    public static implicit operator LocalizedText(LocaleText localeText) => localeText.ToLocalizedText();
    public static implicit operator string(LocaleText localeText) => localeText.Translated;

    public static readonly LocaleText Empty = new(string.Empty);
}
