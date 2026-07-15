using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;
using Embroidery.Core.Models;
public interface IAssemblingPixelImage
{
    List<ModelColor> AssemblingPixel(int countColorsResult, List<CieLab> colorsLAB);
}
