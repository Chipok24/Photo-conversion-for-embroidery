using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Processing;

namespace image
{
    public class CreatePixelImage
    {
        private static ColorSpaceConverter converter = new ColorSpaceConverter();

        public static void CreateIamge(string FileInput, string FileOutput, int countColorsResult, int wsm, int hsm)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
            {
                Rgba32 colorRGBA;
                List<CieLab> colorsLAB = new List<CieLab>();
                double minColorEuclidian;

                image.Mutate(x => x.Resize(wsm * 6, hsm * 6));
                for (int i = 1; i < image.Height; i++)
                {
                    for (int j = 1; j < image.Width; j++)
                    {
                        colorRGBA = image[j, i];

                        Rgb colorRGB = ConvertToRgb(colorRGBA);
                        var colorCielab = converter.ToCieLab(colorRGB);
                        colorsLAB.Add(colorCielab);
                    }
                }

                CieLab[] colorsWork = AnalyticsPixel(countColorsResult, colorsLAB);
                CreateUsingPixel(colorsWork);


                for (int i = 1; i < image.Height; i++)
                {
                    for (int j = 1; j < image.Width; j++)
                    {
                        minColorEuclidian = double.MaxValue;
                        colorRGBA = image[j, i];
                        if (colorRGBA.A == 0) continue;
                        Rgb colorRGB = ConvertToRgb(colorRGBA);
                        var colorCielab = converter.ToCieLab(colorRGB);
                        Rgb minColorEuclidianRGB = colorRGBA;
                        foreach (var item in colorsWork)
                        {
                            var itemColor = item;
                            double result = Evclid(colorCielab, item);
                            if (result <= minColorEuclidian)
                            {
                                minColorEuclidian = result;
                                minColorEuclidianRGB = converter.ToRgb(item);
                            }
                        }
                        image[j, i] = minColorEuclidianRGB;
                    }
                }
                image.SaveAsPng(FileOutput);
            }
        }


        private static async void CreateUsingPixel(CieLab[] colorsWork)
        {
            double minColorEuclidian;
            CieLab color;
            using (StreamWriter writer = new StreamWriter("F:\\code\\images\\threadsUsed.txt", false))
            {
                for (int i = 0; i < colorsWork.Count(); i++)
                {
                    minColorEuclidian = double.MaxValue;
                    color = colorsWork[i];
                    string IdThread = string.Empty;
                    for (int j = 0; j < Dict.ColorsGammaList.Count; j++)
                    {
                        double result = Evclid(colorsWork[i], Dict.ColorsGammaList[j].CieLabColor);
                        if (result < minColorEuclidian)
                        {
                            minColorEuclidian = result;
                            color = Dict.ColorsGammaList[j].CieLabColor;
                            IdThread = Dict.ColorsGammaList[j].IdThread;
                        }
                    }
                    colorsWork[i] = color;
                    await writer.WriteLineAsync(IdThread);
                }
            }
            using (Image<Rgba32> testImage = new Image<Rgba32>(60, 30))
            {
                int i = 1;
                foreach (var item in colorsWork)
                {
                    //Console.WriteLine(item);
                    testImage[i, 15] = converter.ToRgb(item);
                    i++;
                }
                testImage.SaveAsPng("F:\\code\\images\\test2.png");
            }
        }

        private static CieLab[] AnalyticsPixel(int countColorsResult, List<CieLab> colorsLAB)
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
                        double result = Evclid(color, colorsWork[j]);
                        if (result < minColorEuclidian)
                        {
                            minColorEuclidian = result;
                            idColorWork = j;
                        }
                    }
                    colorsRelation[i] = idColorWork;
                }

                for (int i = 0; i < countColorsResult; i++)
                {
                    minColorEuclidian = double.MaxValue;
                    idColorWork = 0;
                    color = colorsWork[i];
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
            return colorsWork;
        }
        private static double Evclid(CieLab cieLab1, CieLab cieLab2)
        {
            return Math.Abs(Math.Pow(cieLab1.L - cieLab2.L, 2)) + Math.Pow(cieLab1.A - cieLab2.A, 2) + Math.Pow(cieLab1.B - cieLab2.B, 2);
        }
        private static Rgb ConvertToRgb(Rgba32 rgba)
        {
            Rgb colorRGB = new Rgb(rgba.R / 255f, rgba.G / 255f, rgba.B / 255f);
            return colorRGB;
        }
    }
}