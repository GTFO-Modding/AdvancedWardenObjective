using Localization;

namespace AWO;

internal static class LocalizedTextExtension
{
    public static string ToText(this LocalizedText text)
    {
        return text.HasTranslation ? Text.Get(text.Id) : text.UntranslatedText;
    }
}
