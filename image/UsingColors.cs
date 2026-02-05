using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

public class UsingColors
{
    public async void CreateTXTUsingColorThread(List<ModelColor> colors)
    {
        using (StreamWriter writer = new StreamWriter("F:\\code\\images\\threadsUsed.txt", false))
        {
            for (int i = 0; i < colors.Count(); i++)
            {
                await writer.WriteLineAsync(colors[i].IdThread);
            }
        }
        
    }
    public void CreatePNGUsingColor(List<ModelColor> colors)
    {
        using (Image<Rgba32> image = new Image<Rgba32>(100, (colors.Count*10)+40))
        {
            int starti = 20;
            int startj = 20;
            foreach (var item in colors)
            {
                for (int i = starti; i < starti+10; i++)
                {
                    for (int j = startj; j < startj+10; j++)
                    {
                        if (item.Symbol[i%10, j%10] == 0)
                        {
                            image[j, i] = item.RgbColor;
                        }
                        else
                        {
                            image[j, i] = new Rgb(0, 0, 0);
                        }
                        
                    }
                }
                starti+=10;
            }
            
            image.SaveAsPng("F:\\code\\images\\test2.png");
        }
    }
}