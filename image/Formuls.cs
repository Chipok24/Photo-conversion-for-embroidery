using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.Arm;
using SixLabors.ImageSharp.ColorSpaces;
public static class Formulas
{
    public static double Evclid(CieLab cieLab1, CieLab cieLab2)
    {
        return Math.Abs(Math.Pow(cieLab1.L - cieLab2.L, 2)) + Math.Pow(cieLab1.A - cieLab2.A, 2) + Math.Pow(cieLab1.B - cieLab2.B, 2);
    }

    public static double CIEDE94(CieLab cieLab1, CieLab cieLab2)
    {
        const double KL = 1.0;
        const double K1 = 0.045;
        const double K2 = 0.015;
        double delL = cieLab1.L - cieLab2.L;
        double delA = cieLab1.A - cieLab2.A;
        double delB = cieLab1.B - cieLab2.B;
        double C1 = Math.Sqrt(cieLab1.A * cieLab1.A + cieLab1.B * cieLab1.B);
        double C2 = Math.Sqrt(cieLab2.A * cieLab2.A + cieLab2.B * cieLab2.B);
        double delC = C1 - C2;
        double delH = Math.Sqrt(delA * delA + delB * delB - delC * delC);
        double result = Math.Sqrt(
            Math.Pow(delL / KL, 2) +
            Math.Pow(delC / (1 + K1 * C1), 2) +
            Math.Pow(delH / (1 + K2 * C1), 2)
        );

        return result;
    }

    public static double CIEDE2000(CieLab cieLab1, CieLab cieLab2)
    {
        const double kL = 1.0;
        const double kC = 1.0;
        const double kH = 1.0;

        double delL = cieLab2.L - cieLab1.L;

        double Gg = G(cieLab1, cieLab2);

        double a1 = (1 + Gg) * cieLab1.A;
        double a2 = (1 + Gg) * cieLab2.A;


        double C1 = Math.Sqrt(Math.Pow(a1, 2) + Math.Pow(cieLab1.B, 2));
        double C2 = Math.Sqrt(Math.Pow(a2, 2) + Math.Pow(cieLab2.B, 2));

        double delC = C2 - C1;


        double h1 = cieLab1.B == 1e-12 && a1 == 1e-12 ? 0 : RadianToDegree(Math.Atan2(cieLab1.B, a1));
        if (h1 < 0) h1 += 360;
        double h2 = cieLab2.B == 1e-12 && a2 == 1e-12 ? 0 : RadianToDegree(Math.Atan2(cieLab2.B, a2));
        if (h2 < 0) h2 += 360;
        

        double delh;
        if (C1 * C2 == 1e-12)
        {
            delh = 0;
        }
        else
        {
            if (Math.Abs(h2 - h1) <= 180) delh = h2 - h1;
            else
            {
                if (h2 - h1 > 180) delh = h2 - h1 - 360;
                else delh = h2 - h1 + 360;
            }
        }

        double delH = 2 * Math.Sqrt(C1 * C2) * Math.Sin(DegreeToRadian(delh / 2));



        double divC = (C1 + C2) / 2;

        double Rc = 2 * Math.Sqrt(Math.Pow(divC, 7) / (Math.Pow(divC, 7) + Math.Pow(25, 7)));

        double divH;
        if (C1 * C2 == 1e-12)
        {
            divH = h1 + h2;
        }
        else
        {
            if (Math.Abs(h1 - h2) <= 180) divH = (h1 + h2) / 2;
            else
            {
                if (Math.Abs(h1 - h2) > 180 && h1 + h2 < 360) divH = (h1 + h2 + 360) / 2;
                else divH = (h1 + h2 - 360) / 2;
            }
        }

        double delO = 30 * Math.Exp(-Math.Pow((divH - 275) / 25, 2));

        double Rt = -Math.Sin(DegreeToRadian(2 * delO)) * Rc;

        double divL = (cieLab1.L + cieLab2.L) / 2;

        double Sl = 1 + ((0.015 * Math.Pow(divL - 50, 2)) / Math.Sqrt(20 +  Math.Pow(divL - 50, 2)));
        double Sc = 1 + 0.045 * divC;

        double T = 1 -
            (0.17 * Math.Cos(DegreeToRadian(divH - 30))) +
            (0.24 * Math.Cos(DegreeToRadian(2 * divH))) +
            (0.32 * Math.Cos(DegreeToRadian(3 * divH + 6))) -
            (0.20 * Math.Cos(DegreeToRadian(4 * divH - 63)));
        double Sh = 1 + 0.015 * divC * T;




        double result = Math.Sqrt(
            Math.Pow(delL / (kL * Sl), 2) +
            Math.Pow(delC / (kC * Sc), 2) +
            Math.Pow(delH / (kH * Sh), 2) +
            (Rt * (delC / (kC * Sc)) * (delH / (kH * Sh)))
        );

        return result;



        double RadianToDegree(double radian)
        {
            return radian * (180 / Math.PI);
        }

        double DegreeToRadian(double Degree)
        {
            return Degree * (Math.PI / 180);
        }

        double G(CieLab cL1, CieLab cL2)
        {
            double C1ab = Math.Sqrt(Math.Pow(cL1.A, 2) + Math.Pow(cL1.B, 2));
            double C2ab = Math.Sqrt(Math.Pow(cL2.A, 2) + Math.Pow(cL2.B, 2));

            double divCab = (C1ab + C2ab) / 2;

            double G = 0.5 * (1 - Math.Sqrt(Math.Pow(divCab, 7) / (Math.Pow(divCab, 7) + Math.Pow(25, 7))));

            return G;
        }
    }
}
