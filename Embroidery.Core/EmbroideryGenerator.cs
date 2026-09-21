using SixLabors.ImageSharp.ColorSpaces.Conversion;
using image;
using System.Diagnostics;
using Embroidery.Core.Models;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace Embroidery.Core.EmbroideryGenerator;

public class EmbroideryGenerator
{
    private readonly string FileWork = "F:\\code\\images\\Work.png";
    public Pattern ImageAlgoritm(string ImageFileInput, int countColorsResult, int wsm, int hsm)
    {
        Image<Rgba32> image = Image.Load<Rgba32>(ImageFileInput);
        ResizeImages resizeImages = new ResizeImages();
        resizeImages.DecreaseImage(image, FileWork, wsm, hsm);
        CreatePixelImage createPixelImage = new CreatePixelImage(new AssemblingPixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols()), new ImageReadPixel(new ColorSpaceConverter()), new ImageReplacingPixels(new ColorSpaceConverter()));   
        var result = createPixelImage.CreateIamge(image, FileWork, countColorsResult, wsm, hsm);

        ImageReadPixel imageReadPixel = new ImageReadPixel(new ColorSpaceConverter());
        var ColorQuantity = imageReadPixel.ReadPixelToQuantity(image, result);
        ColorQuantity = ColorQuantity.Where(x => x.Value > 0).ToDictionary();

        Pattern pattern = new Pattern() {Threads = result, ThreadsQuantity = ColorQuantity, Cells = imageReadPixel.ReadPixelToCell(FileWork, result)};
        return pattern;
    }

/* UsingColors usingColors = new UsingColors();
usingColors.CreatePNGUsingColor(ColorQuantity);

resizeImages.CreateBigImages(FileOutput, SaveBIGFilePath, result); */
}