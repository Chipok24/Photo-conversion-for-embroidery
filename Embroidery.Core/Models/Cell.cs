using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Embroidery.Core.Models;
public struct Cell
{
    public byte? ThreadIndex { get; set; }
    public bool? IsCompleted { get; set; }
}