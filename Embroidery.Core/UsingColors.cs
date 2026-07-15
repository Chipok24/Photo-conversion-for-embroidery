using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using Embroidery.Core.Models;

public class UsingColors
{
    public void CreatePNGUsingColor(Dictionary<ModelColor, int> colors)
    {
        using (Image<Rgba32> image = new Image<Rgba32>(200, (colors.Count*10)+500))
        {
            colors = colors.OrderBy(x => x.Key.IdThread).ToDictionary(x => x.Key, x => x.Value);
            Font font = SystemFonts.CreateFont("Arial", 10);
            int starti = 20;
            int startj = 20;
            foreach (var item in colors)
            {
                image.Mutate(x => x.DrawText($"{item.Key.IdThread} KOL: {item.Value}", font, Color.White, new PointF(startj+40, starti)));
                for (int i = starti; i < starti+10; i++)
                {
                    for (int j = startj; j < startj+10; j++)
                    {
                        if (item.Key.Symbol[i%10, j%10] == 0)
                        {
                            image[j, i] = item.Key.RgbColor;
                        }
                        else
                        {
                            image[j, i] = new Rgb(0, 0, 0);
                        }
                        
                    }
                }
                starti+=20;
            }
            
            
            image.SaveAsPng("F:\\code\\images\\test2.png");
        }
    }
}