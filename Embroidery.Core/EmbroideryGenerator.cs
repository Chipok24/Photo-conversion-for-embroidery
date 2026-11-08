using SixLabors.ImageSharp.ColorSpaces.Conversion;
using image;
using System.Diagnostics;
using Embroidery.Core.Models;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace Embroidery.Core.EmbroideryGenerator;

public class EmbroideryGenerator
{
    //сделать сохранение картинки после createPixelImage.CreateIamge
    public Pattern ImageAlgoritm(Image<Rgba32> ImageFileInput, int countColorsResult, int wsm, int hsm)
    {
        ResizeImages resizeImages = new ResizeImages();
        ImageFileInput = resizeImages.DecreaseImage(ImageFileInput, wsm, hsm);
        CreatePixelImage createPixelImage = new CreatePixelImage(new AssemblingPixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols()), new ImageReadPixel(new ColorSpaceConverter()), new ImageReplacingPixels(new ColorSpaceConverter()));   
        var result = createPixelImage.CreateIamge(ImageFileInput, countColorsResult);

        ImageReadPixel imageReadPixel = new ImageReadPixel(new ColorSpaceConverter());
        var ColorQuantity = imageReadPixel.ReadPixelToQuantity(ImageFileInput, result);
        ColorQuantity = ColorQuantity.Where(x => x.Value > 0).ToDictionary();

        Pattern pattern = new Pattern() {Threads = result, ThreadsQuantity = ColorQuantity, Cells = imageReadPixel.ReadPixelToCell(ImageFileInput, result)};
        return pattern;
    }
}