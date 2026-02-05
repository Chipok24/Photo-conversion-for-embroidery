using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;
public interface IAnalitycs
{
    List<ModelColor> KMeans(int countColorsResult, List<CieLab> colorsLAB);
}
