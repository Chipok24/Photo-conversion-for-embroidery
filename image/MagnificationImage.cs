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
    public class MagnificationImage
    {
        public static void Magnification(string FileWork, string SaveFilePath)
        {
            using (Image<Rgba32> imageI = Image.Load<Rgba32>(FileWork))
            using (Image<Rgba32> imageO = new Image<Rgba32>(imageI.Width*10, imageI.Height*10))
            {
                for (int hI = 1; hI < imageI.Height; hI++)
                {
                    for (int wI = 1; wI < imageI.Width; wI++)
                    {
                        var color = imageI[wI, hI];
                        for (int hO = (hI-1)*10+1; hO <= (hI-1)*10+10; hO++)
                        {
                            for (int wO = (wI-1)*10+1; wO <= (wI-1)*10+10; wO++)
                            {
                                imageO[wO, hO] = color;
                            }
                        }
                    }
                }

                imageO.SaveAsPng(SaveFilePath);
            }
        }
    }
}