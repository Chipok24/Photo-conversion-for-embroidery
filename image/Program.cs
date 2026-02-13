
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using image;
using Microsoft.VisualBasic;
using System.Linq;
using SixLabors.ImageSharp.ColorSpaces;

string FileInput = "F:\\code\\images\\Gun.jpg";
string FileOutput = "F:\\code\\images\\Gun.png";
string SaveBIGFilePath = "F:\\code\\images\\bigtest.png";
int countColorsResult = 30; // количество цветов будет в итоговом изображении
int wsm = 15; //ширина в см
int hsm = 22; //высота в см

ResizeImages resizeImages = new ResizeImages();
resizeImages.DecreaseImage(FileInput, wsm, hsm);

CreatePixelImage createPixelImage = new CreatePixelImage(new AssemblingPixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols()), new ImageReadPixel(new ColorSpaceConverter()), new ImageReplacingPixels(new ColorSpaceConverter()));
var result = createPixelImage.CreateIamge("F:\\code\\images\\Work.png", FileOutput, countColorsResult, wsm, hsm);

UsingColors usingColors = new UsingColors();
usingColors.CreatePNGUsingColor(result);

resizeImages.CreateBigImages(FileOutput, SaveBIGFilePath, result);

/* var cieLab1 = new CieLab( 6.7747F, -0.2908F, -2.4247F);
var cieLab2 = new CieLab(5.8714F, -0.0985F, -2.2286F);

Console.WriteLine(Formulas.CIEDE2000(cieLab1, cieLab2)); */