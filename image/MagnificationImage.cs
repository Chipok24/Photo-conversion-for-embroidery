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
                int numSymbol = 0;
                for (int hI = 1; hI < imageI.Height; hI++)
                {
                    for (int wI = 1; wI < imageI.Width; wI++)
                    {
                        var color = imageI[wI, hI];
                        var symbol = DATACOLOR.SpecialSymbol[numSymbol];
                        int x = 0;
                        int y = 0;
                        for (int hO = (hI-1)*10+1; hO <= (hI-1)*10+10; hO++)
                        {
                            for (int wO = (wI-1)*10+1; wO <= (wI-1)*10+10; wO++)
                            {
                                if (x < 10 && y < 10 && symbol[x, y] == 0) imageO[wO, hO] = color;
                                else imageO[wO, hO] = new Rgba32(0,0,0);
                                x++;
                            }
                            y++;
                        }
                        numSymbol++;
                    }
                }

                imageO.SaveAsPng(SaveFilePath);
            }
        }
    }
}