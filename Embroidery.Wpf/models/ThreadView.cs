using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Embroidery.Wpf.models;

public class ThreadView
{
    public SolidColorBrush Brush { get; init; }
    public string IdThread { get; init; }
    public byte[,] Symbol { get; init; }
}
