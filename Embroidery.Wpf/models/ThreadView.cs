using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Embroidery.Wpf.models;

public class ThreadView
{
    public SolidColorBrush Brush { get; init; }
    public SolidColorBrush BrushTransparent { get; init; }
    public string IdThread { get; init; }
    public byte[,] Symbol { get; init; }
    public DrawingGroup Symbol2 { get; init; }
    public required BitmapSource TestSymbol { get; init; }
    public required BitmapSource TestSymbolTransparent { get; init; }
}