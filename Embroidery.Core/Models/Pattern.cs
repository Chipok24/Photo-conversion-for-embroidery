using SixLabors.ImageSharp.ColorSpaces;

namespace Embroidery.Core.Models;
public class Pattern
{
    public List<ModelColor> Threads { get; init; }
    public Dictionary<byte, int> ThreadsQuantity { get; set; }
    public Cell[,] Cells { get; init; }
}
