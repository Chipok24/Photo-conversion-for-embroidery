using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IDrawSymbols
{
    List<ModelColor> AssignmentSymbolForColor(List<ModelColor> colors);
}
