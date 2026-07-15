using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbroiderySaveAndLoad.Core.Models;

public class RgbSave
{
    public float R { get; init; }
    public float G { get; init; }
    public float B { get; init; }

    public RgbSave(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }
}
