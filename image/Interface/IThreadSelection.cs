using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IThreadSelection
{
    List<ModelColor> ThreadGamma(List<ModelColor> colorsWork);
}
