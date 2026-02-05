using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DrawSymbols : IDrawSymbols
{
    public List<ModelColor> AssignmentSymbolForColor(List<ModelColor> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colors[i].Symbol = DATACOLOR.SpecialSymbol[i];
        }
        return colors;
    }
}
