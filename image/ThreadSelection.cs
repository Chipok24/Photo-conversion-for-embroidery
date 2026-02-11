using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;

public class ThreadSelection : IThreadSelection
{
    public List<ModelColor> ThreadGamma(List<ModelColor> colorsWork)
    {
        List<ModelColor> colorsFull = new List<ModelColor>();
        double minColorEuclidian;

        for (int i = 0; i < colorsWork.Count(); i++)
        {
            ModelColor colorModel = new ModelColor(new CieLab(0,0,0));
            minColorEuclidian = double.MaxValue;
            var color = colorsWork[i].CieLabColor;
            for (int j = 0; j < DATACOLOR.ColorsGammaList.Count; j++)
            {
                var dataColor = DATACOLOR.ColorsGammaList[j];
                double result = Formulas.Evclid(color.L, dataColor.CieLabColor.L, color.A, dataColor.CieLabColor.A, color.B, dataColor.CieLabColor.B);
                if (result < minColorEuclidian && !colorsFull.Any(x =>x.IdThread == dataColor.IdThread))
                {
                    minColorEuclidian = result;
                    colorModel = DATACOLOR.ColorsGammaList[j];
                }
            }
            colorsFull.Add(new ModelColor(colorModel.CieLabColor, colorModel.RgbColor, colorModel.IdThread));
        }
        return colorsFull;
    }
}
