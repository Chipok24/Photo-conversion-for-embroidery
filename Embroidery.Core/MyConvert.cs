using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
public static class MyConvert
{
    public static Rgb ConvertToRgb(Rgba32 rgba)
    {
        Rgb colorRGB = new Rgb(rgba.R / 255f, rgba.G / 255f, rgba.B / 255f);
        return colorRGB;
    }
}
