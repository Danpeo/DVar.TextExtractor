namespace DVar.TextExtractor.Gui.Utils;

public static class Lang
{
    public static string GetLangModeStr(LangMode langMode)
    {
        return langMode switch
        {
            LangMode.JapHorizontal => "jpn",
            LangMode.JapVertical => "jpn_vert",
            LangMode.JapHorizontalAlt => "Japanese",
            LangMode.JapVerticalAlt => "Japanese_vert",
            _ => "jpn"
        };
    }
}

public enum LangMode
{
    JapHorizontal,
    JapVertical,
    JapHorizontalAlt,
    JapVerticalAlt
}