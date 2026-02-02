using SixLabors.ImageSharp.ColorSpaces;

public class ModelColor
{
    public CieLab CieLabColor { get; init; }
    public Rgb RgbColor { get; init; }
    public string IdThread { get; init; }
    public byte[,] Symbol { get; set; }
    public ModelColor(CieLab cieLabColor, Rgb rgbColor, string idThread)
    {
        CieLabColor = cieLabColor;
        RgbColor = rgbColor;
        IdThread = idThread;
    }
}