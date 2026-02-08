using SixLabors.ImageSharp.ColorSpaces;

public class ModelColor
{
    public CieLab CieLabColor { get; init; }
    public Rgb RgbColor { get; set; }
    public string IdThread { get; set; }
    public byte[,] Symbol { get; set; }
    public ModelColor(CieLab cieLabColor, Rgb rgbColor, string idThread)
    {
        CieLabColor = cieLabColor;
        RgbColor = rgbColor;
        IdThread = idThread;
    }
    public ModelColor(CieLab cieLabColor)
    {
        CieLabColor = cieLabColor;
    }
}