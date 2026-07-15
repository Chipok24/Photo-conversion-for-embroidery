using EmbroiderySaveAndLoad.Core.Models;
using SixLabors.ImageSharp.ColorSpaces;
public class ModelColorSave
{
    public CieLabSave CieLabColor { get; init; }
    public RgbSave RgbColor { get; init; }
    public string IdThread { get; init; }
    public byte[][] Symbol { get; init; }
    public ModelColorSave (CieLabSave cieLabColor, RgbSave rgbColor, string idThread, byte[][] symbol)
    {
        CieLabColor = cieLabColor;
        RgbColor = rgbColor;
        IdThread = idThread;
        Symbol = symbol;
    }
}