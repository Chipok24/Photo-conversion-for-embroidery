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
        ModelColor colorModel = new ModelColor(new CieLab(0,0,0));
        for (int i = 0; i < colorsWork.Count(); i++)
        {
            minColorEuclidian = double.MaxValue;
            var color = colorsWork[i].CieLabColor;
            for (int j = 0; j < DATACOLOR.ColorsGammaList.Count; j++)
            {
                var dataColor = DATACOLOR.ColorsGammaList[j].CieLabColor;
                double result = Formulas.Evclid(color.L, dataColor.L, color.A, dataColor.A, color.B, dataColor.B);
                if (result < minColorEuclidian)
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
