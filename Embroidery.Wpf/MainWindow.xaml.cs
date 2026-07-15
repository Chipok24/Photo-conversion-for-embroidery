using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Embroidery.Core.Models;
using Embroidery.Core.EmbroideryGenerator;
using EmbroiderySaveAndLoad.Core.FileWork;
using System.Drawing;
using Embroidery.Wpf.Controls;
using Embroidery.Wpf.models;
using Embroidery.Wpf.Mappers;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
namespace Embroidery.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly EmbroideryGenerator embroideryGenerator = new EmbroideryGenerator();
    private FileWork fileWork = new FileWork();
    private Pattern pattern;
    PatternView patternView;
    public MainWindow()
    {
        InitializeComponent();
        
        //


        Loaded += MainWindow_Save;

    }

    public async void Button_Save(object? sender, RoutedEventArgs e)
    {
        if (patternView is null || pattern is null)
        {
            return;
        }
        await fileWork.PatternSaveAsync(PatternMapper.ToPattern(patternView , pattern));
    }

    public void Button_Open(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        bool? result = openFileDialog.ShowDialog();
        if (result == true)
        {
            string selectPathFile = openFileDialog.FileName;

            Debug.WriteLine(selectPathFile);
            DisplayPattern(selectPathFile);
        }
    }

    private void DisplayPattern(string PathFile)
    {
        
        pattern = embroideryGenerator.ImageAlgoritm(PathFile, 30, 20, 20);
        patternView = PatternMapper.ToViewModel(pattern);
        PatternViewCanvas.SetPatternVies(patternView);
    }

    private async void MainWindow_Save(object? sender, RoutedEventArgs e)
    {
        //await fileWork.PatternSaveAsync(PatternMapper.ToPattern(patternView , pattern));

        await MainWindow_Loaded();
    }

    public async Task MainWindow_Loaded()
    {
        pattern = await fileWork.PatternLoadAsync();
        patternView = PatternMapper.ToViewModel(pattern);
        PatternViewCanvas.SetPatternVies(patternView);
        PatternViewCanvas.SetCamera(new Camera
        {
            CameraX = 0,
            CameraY = 0,
            Zoom = 5
        });
    }
}