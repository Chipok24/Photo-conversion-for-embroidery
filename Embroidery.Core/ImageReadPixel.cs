using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using System.Security.Cryptography.X509Certificates;
using Embroidery.Core.Models;

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

    public Dictionary<byte, int> ReadPixelToQuantity(string FileInput, List<ModelColor> modelColors)
    {
        using (Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
        {
            Dictionary<byte, int> colorQuantity = new Dictionary<byte, int>(); 
            for (int i = 1; i < image.Height; i++)
            {
                for (int j = 1; j < image.Width; j++)
                {
                    if (image[j, i].A == 0) continue;
                    Rgb color = image[j, i];
                    var rrrr = modelColors.FindIndex(x => x.RgbColor == color);

                    if (colorQuantity.ContainsKey((byte)rrrr))
                    {
                        colorQuantity[(byte)rrrr] += 1;
                    }
                    else
                    {
                        colorQuantity[(byte)rrrr] = 1;
                    }
                }
            }
            return colorQuantity;
        }
    }

    public Cell[,] ReadPixelToCell(string FileInput, List<ModelColor> modelColors)
    {
        using (Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
        {
            Cell[,] cells = new Cell[image.Width, image.Height];
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (image[j, i].A == 0)
                    {
                        continue;
                    }
                    Rgb color = image[j, i];
                    byte ThreadIndex = (byte)modelColors.FindIndex(x => x.RgbColor == color);
                    cells[j, i] = new Cell() {ThreadIndex = ThreadIndex, IsCompleted = false};
                }
            }
            return cells;
        }
    }
}
