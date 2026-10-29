using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public interface IImageReplacingPixels
{
    public void ReplacingPixels(Image<Rgba32> image, List<ModelColor> WorkColors);
}
