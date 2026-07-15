using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Embroidery.Core.Models;
using Embroidery.Wpf.models;

namespace Embroidery.Wpf.Mappers;

public static class PatternMapper
{
    public static PatternView ToViewModel(Pattern pattern)
    {
        List<ThreadView> threadViews = new List<ThreadView>();
        foreach (var thread in pattern.Threads)
        {
            SolidColorBrush brush = new SolidColorBrush(
                Color.FromRgb(
                    (byte)Math.Clamp(Math.Round(thread.RgbColor.R * 255), 0, 255),
                    (byte)Math.Clamp(Math.Round(thread.RgbColor.G * 255), 0, 255),
                    (byte)Math.Clamp(Math.Round(thread.RgbColor.B * 255), 0, 255)
                )
            );
            brush.Freeze();

            threadViews.Add(new ThreadView
            {
                Brush = brush,
                IdThread = thread.IdThread,
                Symbol =  thread.Symbol
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