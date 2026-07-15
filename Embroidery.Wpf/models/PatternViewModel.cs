using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Embroidery.Core.Models;
namespace Embroidery.Wpf.models;

public class PatternView
{
    public List<ThreadView> Threads { get; init; }
    public Dictionary<byte, int> ThreadsQuantity { get; set; }
    public Cell[,] Cells { get; init; }
}
