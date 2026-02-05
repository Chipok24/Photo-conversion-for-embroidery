using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using System.Reflection.Emit;
using System.IO;
using image;

string FileInput = "F:\\code\\images\\Duck2.png";
string FileOutput = "F:\\code\\images\\Duck.png";
string SaveBIGFilePath = "F:\\code\\images\\bigtest.png";
int countColorsResult = 25; // количество цветов будет в итоговом изображении (из-за алгоритма подбора, их будет возможно меньше)
int wsm = 25; //ширина в см
int hsm = 20; //высота в см

CreatePixelImage createPixelImage = new CreatePixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols());
var result = createPixelImage.CreateIamge(FileInput, FileOutput, countColorsResult, wsm, hsm);

UsingColors usingColors = new UsingColors();
usingColors.CreatePNGUsingColor(result);

ResizeImages resizeImages = new ResizeImages();
resizeImages.CreateBigImages(FileOutput, SaveBIGFilePath, result);


//MagnificationImage.Magnification(FileOutput, SaveBIGFilePath);











/* Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile("F:\\code\\images\\bigtest.bmp");
using(Graphics graphics = Graphics.FromImage(bitmap))
{
    graphics.DrawString("A", new Font("Geometria", 7), Brushes.Black, 0, 10); 
}
bitmap.Save("F:\\code\\images\\bigtest2.bmp"); */
