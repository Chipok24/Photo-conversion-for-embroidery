using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;


public class ImageReadPixel : IImageReadPixel
{
    private readonly ColorSpaceConverter _converter;
    public ImageReadPixel(ColorSpaceConverter converter)
    {
        _converter = converter;
    }
    public List<CieLab> ReadPixleToCieLAb(Image<Rgba32> image)
    {
        Rgba32 colorRGBA;
        List<CieLab> colorsSieLab = new List<CieLab>();
        for (int i = 1; i < image.Height; i++)
        {
            for (int j = 1; j < image.Width; j++)
            {
                colorRGBA = image[j, i];

                Rgb colorRGB = MyConvert.ConvertToRgb(colorRGBA);
                var colorCielab = _converter.ToCieLab(colorRGB);
                colorsSieLab.Add(colorCielab);
            }
        }
        return colorsSieLab;
    }
}
