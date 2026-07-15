
namespace EmbroiderySaveAndLoad.Core.Models;
public class CieLabSave
{
    public float L { get; init; }
    public float A { get; init; }
    public float B { get; init; }

    public CieLabSave(float l, float a, float b)
    {
        L = l;
        A = a;
        B = b;
    }
}