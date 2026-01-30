using SixLabors.ImageSharp.ColorSpaces;

public class ModelColorGamma
{
    public CieLab CieLabColor { get; init; }
    public Rgb RgbColor { get; init; }
    public string IdThread { get; init; }
    public ModelColorGamma(CieLab cieLabColor, Rgb rgbColor, string idThread)
    {
        CieLabColor = cieLabColor;
        RgbColor = rgbColor;
        IdThread = idThread;
    }
}