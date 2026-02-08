using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;

public class AssemblingPixelImage : IAssemblingPixelImage
{
    private readonly IAnalitycs _analitycs;
    private readonly IThreadSelection _threadSelection;
    private readonly IDrawSymbols _drawSymbols;
    public AssemblingPixelImage(IAnalitycs analitycs, IThreadSelection threadSelection, IDrawSymbols drawSymbols)
    {
            _analitycs = analitycs;
            _threadSelection = threadSelection;
            _drawSymbols = drawSymbols;
    }
    public List<ModelColor> AssemblingPixel(int countColorsResult, List<CieLab> colorsLAB)
    {
        List<ModelColor> WorkColors = _analitycs.KMeans(countColorsResult, colorsLAB);
        WorkColors = _threadSelection.ThreadGamma(WorkColors);
        WorkColors = _drawSymbols.AssignmentSymbolForColor(WorkColors);
        WorkColors = WorkColors.GroupBy(x => x.IdThread).Select(y => y.First()).ToList();
        return WorkColors;
    }
}
