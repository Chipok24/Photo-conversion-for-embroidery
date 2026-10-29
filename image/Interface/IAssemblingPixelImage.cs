using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;

public interface IAssemblingPixelImage
{
    List<ModelColor> AssemblingPixel(int countColorsResult, List<CieLab> colorsLAB);
}
