using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;

public class ImageReplacingPixels : IImageReplacingPixels
{
    private readonly ColorSpaceConverter _converter;
    public ImageReplacingPixels(ColorSpaceConverter converter)
    {
        _converter = converter;
    }
    public void ReplacingPixels(Image<Rgba32> image, List<ModelColor> WorkColors)
    {
        double minColorEuclidian;
        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                minColorEuclidian = double.MaxValue;
                if (image[j, i].A == 0) continue;

                Rgb colorRGB = image[j, i];
                var colorCielab = _converter.ToCieLab(colorRGB);

                Rgb minColorEuclidianRGB = colorRGB;
                foreach (var item in WorkColors)
                {
                    double result = Formulas.Evclid(colorCielab.L, item.CieLabColor.L, colorCielab.A, item.CieLabColor.A, colorCielab.B, item.CieLabColor.B);
                    if (result <= minColorEuclidian)
                    {
                        minColorEuclidian = result;
                        minColorEuclidianRGB = item.RgbColor;
                    }
                }
                image[j, i] = minColorEuclidianRGB;
            }
        }
    }
}
