using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel;

namespace image
{
    public class CreatePixelImage
    {
        private readonly IAnalitycs _analitycs;
        private readonly IThreadSelection _threadSelection;
        private readonly IDrawSymbols _drawSymbols;
        private readonly ColorSpaceConverter converter = new ColorSpaceConverter();
        public CreatePixelImage(IAnalitycs analitycs, IThreadSelection threadSelection, IDrawSymbols drawSymbols)
        {
            _analitycs = analitycs;
            _threadSelection = threadSelection;
            _drawSymbols = drawSymbols;
        }

        public List<ModelColor> CreateIamge(string FileInput, string FileSmallOutput, int countColorsResult, int wsm, int hsm)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
            {
                Rgba32 colorRGBA;
                List<CieLab> colorsLAB = new List<CieLab>();
                // подсчет всех пикселей
                image.Mutate(x => x.Resize(wsm * 6, hsm * 6));
                for (int i = 1; i < image.Height; i++)
                {
                    for (int j = 1; j < image.Width; j++)
                    {
                        colorRGBA = image[j, i];

                        Rgb colorRGB = MyConvert.ConvertToRgb(colorRGBA);
                        var colorCielab = converter.ToCieLab(colorRGB);
                        colorsLAB.Add(colorCielab);
                    }
                }


                List<ModelColor> WorkColors = _analitycs.KMeans(countColorsResult, colorsLAB);
                WorkColors = _threadSelection.ThreadGamma(WorkColors);
                WorkColors = _drawSymbols.AssignmentSymbolForColor(WorkColors);
                WorkColors = WorkColors.GroupBy(x => x.IdThread).Select(y => y.First()).ToList();
                
                //CreateUsingPixel(colorsWork);



                double minColorEuclidian;
                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        minColorEuclidian = double.MaxValue;
                        if (image[j, i].A == 0) continue;

                        Rgb colorRGB = image[j, i];
                        var colorCielab = converter.ToCieLab(colorRGB);

                        Rgb minColorEuclidianRGB = colorRGB;
                        foreach (var item in WorkColors)
                        {
                            double result = Formulas.Evclid(colorCielab.L, item.CieLabColor.L, colorCielab.A, item.CieLabColor.A, colorCielab.B, item.CieLabColor.B);
                            if (result <= minColorEuclidian)
                            {
                                minColorEuclidian = result;
                                minColorEuclidianRGB = item.RgbColor;
                            }
                        }
                        image[j, i] = minColorEuclidianRGB;
                    }
                }
                image.SaveAsPng(FileSmallOutput);
                return WorkColors;
            }
        }
    }
}