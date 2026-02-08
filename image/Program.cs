
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using image;

string FileInput = "F:\\code\\images\\tony2.png";
string FileOutput = "F:\\code\\images\\bir.png";
string SaveBIGFilePath = "F:\\code\\images\\bigtest.png";
int countColorsResult = 40; // количество цветов будет в итоговом изображении (из-за алгоритма подбора, их будет возможно меньше)
int wsm = 45; //ширина в см
int hsm = 50; //высота в см

ResizeImages resizeImages = new ResizeImages();
resizeImages.DecreaseImage(FileInput, wsm, hsm);

CreatePixelImage createPixelImage = new CreatePixelImage(new AssemblingPixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols()), new ImageReadPixel(new ColorSpaceConverter()), new ImageReplacingPixels(new ColorSpaceConverter()));
var result = createPixelImage.CreateIamge("F:\\code\\images\\Work.png", FileOutput, countColorsResult, wsm, hsm);

UsingColors usingColors = new UsingColors();
usingColors.CreatePNGUsingColor(result);

resizeImages.CreateBigImages(FileOutput, SaveBIGFilePath, result);