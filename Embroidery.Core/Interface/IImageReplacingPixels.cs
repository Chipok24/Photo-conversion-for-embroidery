using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Embroidery.Core.Models;
public interface IImageReplacingPixels
{
    public void ReplacingPixels(Image<Rgba32> image, List<ModelColor> WorkColors);
}
