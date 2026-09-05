using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Embroidery.Core.Models;
using Embroidery.Wpf.models;

namespace Embroidery.Wpf.Mappers;

public static class PatternMapper
{
    public static PatternView ToViewModel(Pattern pattern)
    {
        const byte A = 50;
        List<ThreadView> threadViews = new List<ThreadView>();
        foreach (var thread in pattern.Threads)
        {
            byte backGroundR = (byte)Math.Clamp(Math.Round(thread.RgbColor.R * 255), 0, 255);
            byte backGroundG = (byte)Math.Clamp(Math.Round(thread.RgbColor.G * 255), 0, 255);
            byte backGroundB = (byte)Math.Clamp(Math.Round(thread.RgbColor.B * 255), 0, 255);

            SolidColorBrush brush = new SolidColorBrush(
                Color.FromArgb(
                    255,
                    backGroundR,
                    backGroundG,
                    backGroundB
                )
            );

            SolidColorBrush brushTransparent = new SolidColorBrush(
                Color.FromArgb(
                    A,
                    backGroundR,
                    backGroundG,
                    backGroundB
                )
            );
            brush.Freeze();
            brushTransparent.Freeze();

            

            byte SymbolR = 0;

            List<byte> bytes = new List<byte> ();

            byte[,] matrix3 = thread.Symbol;
            for (int i = 0; i < 20; i++)
            {
                bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
            }

            for (int symbolX = 0; symbolX < matrix3.GetLength(0); symbolX++)
            {
                bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
                        bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
                for (int symbbolY = 0; symbbolY < matrix3.GetLength(1); symbbolY++)
                {
                    if (matrix3[symbolX, symbbolY] == 1)
                    {
                        bytes.Add(SymbolR);
                        bytes.Add(SymbolR);
                        bytes.Add(SymbolR);
                        bytes.Add(255);
                        continue;
                    }
                    bytes.Add(backGroundB);
                    bytes.Add(backGroundG);
                    bytes.Add(backGroundR);
                    bytes.Add(255);
                }
                bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
                        bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
                
            }

            for (int i = 0; i < 20; i++)
            {
                bytes.Add(backGroundB);
                        bytes.Add(backGroundG);
                        bytes.Add(backGroundR);
                        bytes.Add(255);
            }


            BitmapSource bitmap = BitmapSource.Create(10, 10, 96, 96, PixelFormats.Bgra32, null, bytes.ToArray(), 40);

            for (int i = 3; i < bytes.Count; i+=4)
            {
                bytes[i] = A;
            }

            BitmapSource bitmap2 = BitmapSource.Create(10, 10, 96, 96, PixelFormats.Bgra32, null, bytes.ToArray(), 40);


            threadViews.Add(new ThreadView
            {
                Brush = brush,
                BrushTransparent = brushTransparent,
                IdThread = thread.IdThread,
                Symbol =  thread.Symbol,
                TestSymbol = bitmap,
                TestSymbolTransparent = bitmap2
            });
        }

        return new PatternView
        {
          Threads =  threadViews,
          ThreadsQuantity = pattern.ThreadsQuantity,
          Cells = pattern.Cells
        };
    }

    public static Pattern ToPattern(PatternView patternViewModel, Pattern pattern)
    {
        return new Pattern
        {
            Threads = pattern.Threads,
            ThreadsQuantity = patternViewModel.ThreadsQuantity,
            Cells = patternViewModel.Cells
        };
    }

    
}