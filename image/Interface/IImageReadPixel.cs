using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
public interface IImageReadPixel
{
    List<CieLab> ReadPixleToCieLAb(Image<Rgba32> image);
}
