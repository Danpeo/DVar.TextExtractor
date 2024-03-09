using System;
using System.IO;

namespace DVar.TextExtractor.Gui.Utils;

public class Data
{
    public static string GetTessdataDirectory()
    {
        string assetsDirectory = AppDomain.CurrentDomain.BaseDirectory;
        assetsDirectory = Path.Combine(assetsDirectory, "Data");

        string tessdataDirectory = Path.Combine(assetsDirectory, "tessdata");
        return tessdataDirectory;
    }
}