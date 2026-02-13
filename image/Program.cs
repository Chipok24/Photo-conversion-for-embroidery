using SixLabors.ImageSharp.ColorSpaces.Conversion;
using image;

string FileInput = "путь к изображению, которое необходимо преобразовать";
string FileOutput = "путь, в который необходимо сохранить изображение";
string SaveBIGFilePath = "путь, в который необходимо сохранить изображение с символами";
int countColorsResult = 30; // количество цветов будет в итоговом изображении
int wsm = 15; //ширина в см
int hsm = 22; //высота в см

ResizeImages resizeImages = new ResizeImages();
resizeImages.DecreaseImage(FileInput, wsm, hsm);

CreatePixelImage createPixelImage = new CreatePixelImage(new AssemblingPixelImage(new Analitycs(), new ThreadSelection(), new DrawSymbols()), new ImageReadPixel(new ColorSpaceConverter()), new ImageReplacingPixels(new ColorSpaceConverter()));
var result = createPixelImage.CreateIamge("Work.png", FileOutput, countColorsResult, wsm, hsm);

UsingColors usingColors = new UsingColors();
usingColors.CreatePNGUsingColor(result);

resizeImages.CreateBigImages(FileOutput, SaveBIGFilePath, result);