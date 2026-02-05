using SixLabors.ImageSharp.ColorSpaces;
public static class Formulas
{
    public static double Evclid(float x1, float x2, float y1, float y2, float z1, float z2)
    {
        return Math.Abs(Math.Pow(x1 - x2, 2)) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2);
    }
}
