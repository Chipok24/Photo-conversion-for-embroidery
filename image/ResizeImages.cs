using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Processing;
public class ResizeImages
{
    public void CreateBigImages(string FileInput, string FileBigOutput, List<ModelColor> Colors)
    {
        using (Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
        using (Image<Rgba32> imageO = new Image<Rgba32>(image.Width*10, image.Height*10))
        {
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (image[j, i].A == 0) continue;
                    byte[,] symbolWork = null;
                    Rgb color = image[j, i];
                    foreach (var item in Colors)
                    {
                        if (item.RgbColor == color) symbolWork = item.Symbol;
                    }
                    
                    for (int hO = i*10; hO < i*10+10; hO++)
                    {
                        for (int wO = j*10; wO < j*10+10; wO++)
                        {
                            if (symbolWork[hO%10, wO%10] == 0)
                            {
                                imageO[wO, hO] = color;
                            }
                            else
                            {
                                imageO[wO, hO] = new Rgb(0, 0, 0);
                            }
                        }
                    }
                }
            }
            imageO.SaveAsPng(FileBigOutput);
        }
    }
    public void DecreaseImage(string FileInput, string FileWork, int wsm, int hsm)
    {
        using(Image<Rgba32> image = Image.Load<Rgba32>(FileInput))
        {
            image.Mutate(x => x.Resize(wsm * 6, hsm * 6));
            image.SaveAsPng(FileWork);
        }
    }
}
