using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using Embroidery.Core.Models;
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

/*         List<ModelColor> test = new List<ModelColor>{new ModelColor( new CieLab(69.76918f, 5.480051f, 2.585721f), new Rgb (0.7137255f, 0.654902f, 0.6509804f), "0053"),
new ModelColor( new CieLab(62.262627f, -1.2571216f, 4.759598f), new Rgb (0.5921569f, 0.5921569f, 0.5568628f), "0060"),
new ModelColor( new CieLab(83.07563f, -0.44348836f, 3.1176686f), new Rgb (0.8156863f, 0.8117647f, 0.7882353f), "0150"),
new ModelColor( new CieLab(88.671005f, 0.9691715f, -0.7302046f), new Rgb (0.8784314f, 0.87058824f, 0.8784314f), "0416"),
new ModelColor( new CieLab(88.30072f, 2.339989f, 1.3857722f), new Rgb (0.8901961f, 0.8627451f, 0.85882354f), "0670"),
new ModelColor( new CieLab(75.27953f, 5.2607656f, 3.0489445f), new Rgb (0.77254903f, 0.7137255f, 0.7058824f), "0672"),
new ModelColor( new CieLab(64.14194f, 3.7682354f, 4.834652f), new Rgb (0.64705884f, 0.6f, 0.5764706f), "0673"),
new ModelColor( new CieLab(59.658455f, 2.5181174f, 5.004239f), new Rgb (0.5921569f, 0.5568628f, 0.5294118f), "0674"),
new ModelColor( new CieLab(87.65489f, -0.6632805f, 2.463019f), new Rgb (0.8627451f, 0.8627451f, 0.84313726f), "3038"),
new ModelColor( new CieLab(82.05768f, -0.2143979f, 3.7549138f), new Rgb (0.80784315f, 0.8f, 0.77254903f), "3044"),
new ModelColor( new CieLab(4.1131516f, -1.8943846f, -9.352058f), new Rgb (0.007843138f, 0.0627451f, 0.10980392f), "3120"),
new ModelColor( new CieLab(41.399506f, -1.3074577f, 8.87906f), new Rgb (0.39215687f, 0.38431373f, 0.3254902f), "3229"),
new ModelColor( new CieLab(33.723892f, 0.5837679f, -0.43960214f), new Rgb (0.3137255f, 0.30980393f, 0.3137255f), "3230"),
new ModelColor( new CieLab(36.46008f, -0.33570826f, -2.0430028f), new Rgb (0.32941177f, 0.3372549f, 0.34901962f), "3232"),
new ModelColor( new CieLab(97.91983f, -0.17485023f, 2.0475268f), new Rgb (0.98039216f, 0.9764706f, 0.9607843f), "3250"),
new ModelColor( new CieLab(83.43404f, -0.016748905f, -2.7376413f), new Rgb (0.80784315f, 0.8156863f, 0.8352941f), "3255"),
new ModelColor( new CieLab(59.846954f, 0.5142987f, -3.3246994f), new Rgb (0.56078434f, 0.5647059f, 0.5882353f), "3257"),
new ModelColor( new CieLab(23.407906f, -2.7357192f, 1.1085749f), new Rgb (0.20392157f, 0.22352941f, 0.21176471f), "3260"),
new ModelColor( new CieLab(24.994503f, 2.390191f, 2.2415102f), new Rgb (0.2509804f, 0.22745098f, 0.21960784f), "4195"),
new ModelColor( new CieLab(12.207859f, 0.9591654f, -12.41726f), new Rgb (0.09803922f, 0.1254902f, 0.19215687f), "5172"),
new ModelColor( new CieLab(81.144135f, -0.85002184f, 0.24588108f), new Rgb (0.78431374f, 0.7921569f, 0.7882353f), "5182"),
new ModelColor( new CieLab(18.378323f, -3.0536504f, 6.007272f), new Rgb (0.16862746f, 0.18039216f, 0.14117648f), "5184"),
new ModelColor( new CieLab(17.428093f, -1.9715428f, -8.585387f), new Rgb (0.13333334f, 0.17254902f, 0.21568628f), "5185"),
new ModelColor( new CieLab(15.967949f, 12.372166f, -2.758491f), new Rgb (0.21568628f, 0.12941177f, 0.17254902f), "5205")}; */
        WorkColors = _drawSymbols.AssignmentSymbolForColor(WorkColors);
        return WorkColors;
    }
}
