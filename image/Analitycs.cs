using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces;
public class Analitycs : IAnalitycs
{
    public List<ModelColor> KMeans(int countColorsResult, List<CieLab> colorsLAB)
    {
        Random random = new Random();
        CieLab[] colorsWork = new CieLab[countColorsResult];
        for (int i = 0; i < countColorsResult; i++)
        {
            colorsWork[i] = colorsLAB[random.Next(colorsLAB.Count)];
        }

        int[] colorsRelation = new int[colorsLAB.Count];
        int idColorWork;
        double minColorEuclidian;
        CieLab color;

        float sumL = 0, sumA = 0, sumB = 0;
        int countLAB = 0;
        for (int _ = 0; _ < 20; _++)
        {
            for (int i = 0; i < colorsLAB.Count; i++)
            {
                minColorEuclidian = double.MaxValue;
                idColorWork = 0;
                color = colorsLAB[i];
                for (int j = 0; j < countColorsResult; j++)
                {
                    double resultEvclid = Formulas.Evclid(color.L, colorsWork[j].L, color.A, colorsWork[j].A, color.B, colorsWork[j].B);
                    if (resultEvclid < minColorEuclidian)
                    {
                        minColorEuclidian = resultEvclid;
                        idColorWork = j;
                    }
                }
                colorsRelation[i] = idColorWork;
            }

            for (int i = 0; i < countColorsResult; i++)
            {
                for (int j = 0; j < colorsLAB.Count; j++)
                {
                    if (colorsRelation[j] == i)
                    {
                        sumL += colorsLAB[j].L;
                        sumA += colorsLAB[j].A;
                        sumB += colorsLAB[j].B;
                        countLAB++;
                    }
                }
                if (countLAB > 0)
                {
                    colorsWork[i] = new CieLab(sumL / countLAB, sumA / countLAB, sumB / countLAB);
                }
                sumL = 0; sumA = 0; sumB = 0; countLAB = 0;
            }
        }

        List<ModelColor> KMeansColors = new List<ModelColor>();
        foreach (var item in colorsWork)
        {
            KMeansColors.Add(new ModelColor(item));
        }
        return KMeansColors;
    }
}
