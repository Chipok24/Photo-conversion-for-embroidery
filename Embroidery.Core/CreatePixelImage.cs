using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using Embroidery.Core.Models;
namespace image
{
    public class CreatePixelImage
    {
        private readonly IAssemblingPixelImage _assemblingPixelImage;
        private readonly IImageReadPixel _imageReadPixel;
        private readonly IImageReplacingPixels _imageReplacingPixels;
        public CreatePixelImage(IAssemblingPixelImage assemblingPixelImage, IImageReadPixel imageReadPixel, IImageReplacingPixels imageReplacingPixels)
        {
            _assemblingPixelImage = assemblingPixelImage;
            _imageReadPixel = imageReadPixel;
            _imageReplacingPixels = imageReplacingPixels;
        }

        public List<ModelColor> CreateIamge(Image<Rgba32> image, string FileSmallOutput, int countColorsResult, int wsm, int hsm)
        {
            List<CieLab> colorsLAB = _imageReadPixel.ReadPixleToCieLAb(image); 

            List<ModelColor> WorkColors = _assemblingPixelImage.AssemblingPixel(countColorsResult, colorsLAB);

            _imageReplacingPixels.ReplacingPixels(image, WorkColors);
            image.SaveAsPng(FileSmallOutput);
            return WorkColors;
        }
    }
}