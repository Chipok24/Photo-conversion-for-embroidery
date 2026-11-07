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
using Embroidery.Wpf.Settings;
using EmbroiderySaveAndLoad.Core.Models;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Microsoft.VisualBasic;
namespace Embroidery.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly EmbroideryGenerator _embroideryGenerator = new EmbroideryGenerator();
    public readonly SettingsManager _settingsManager = new SettingsManager();
    private readonly FileWork _fileWork = new FileWork();
    private List<ProjectFile> _projectFiles;
    private Setting _seting;
    private Pattern? _pattern;
    private PatternView? _patternView;
    public MainWindow()
    {
        InitializeComponent();

        Loaded += MainWindow_Loaded;
        PatternViewCanvas.MouseWheel += PatternCanvasMouseWheel;
        PatternViewCanvas.PatternViewChanged2 += PatternViewCanvas_PatternViewChanged2;
        ThreadInfoView.ThreadIndexDisplay += ThreadInfoView_ThreadIndexDisplay;
        ThreadInfoView.ThreadIndexChange += ThreadInfoVies_ThreadIndexChange;
    }

    public async void Button_Save(object? sender, RoutedEventArgs e)
    {
        if (_patternView is null || _pattern is null)
        {
            return;
        }
        await PatternViewSave();
    }

    public async void Button_Open(object? sender, RoutedEventArgs e)
    {
        ImportImageWindow importImageWindow = new ImportImageWindow();
        if (importImageWindow.ShowDialog()==true)
        {
            DisplayPattern(
                importImageWindow.FilePath,
                importImageWindow.HeightSM,
                importImageWindow.WidthSM,
                importImageWindow.ColorCount
            );
            if (_pattern is null)
            {
                return;
            }
            _seting.LastOpenedProject = await _fileWork.PrimarySavePatternAsync(_pattern, _seting.ProjectsFolder ,importImageWindow.FilePath);
            if (_pattern is not null && _seting.Backups is not null)
            {
                await _fileWork.BackupSaveAsync(_pattern, _seting.Backups, _seting.LastOpenedProject);
            }
            await AutoLoadProjectFiles();
        }
    }

    public async void Button_Setting(object? sender, RoutedEventArgs e)
    {
        SettingView settingView = new SettingView(_seting);
        if (settingView.ShowDialog() is true)
        {
            await AutoLoadProjectFiles();
        }
    }

    public async void ProjectsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (Projects.SelectedIndex is int project)
        {
            await PatternViewSave();
            if (_pattern is not null && _seting.Backups is not null)
            {
                await _fileWork.BackupSaveAsync(_pattern, _seting.Backups, _seting.LastOpenedProject);
            }

            PatternViewCanvas.ResetParameters();
            ThreadInfoView.ResetParameters();

            await ProjectFileDisplay(_projectFiles[project].FilePath);
            _seting.LastOpenedProject = _projectFiles[project].FilePath;
            await _settingsManager.SaveSetting(_seting);
        }
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        _seting = await _settingsManager.LoadSetting();
        await AutoLoadProjectFiles();
        await ProjectFileDisplay(_seting.LastOpenedProject);
        await AutoSavePatternAsync();
    }

    private async void MainWindow_Save(object? sender, RoutedEventArgs e)
    {
        await PatternViewSave();
    }

    private void DisplayPattern(string PathFile, int HSM, int WSM, int CountColor)
    {
        SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(PathFile);
        _pattern = _embroideryGenerator.ImageAlgoritm(image, CountColor, WSM, HSM);
        PatternViewDisplay();
    }

    private async Task ProjectFileDisplay(string ProjectFile)
    {
        _pattern = await _fileWork.PatternLoadAsync(ProjectFile);

        if (_pattern is null)
        {
            return;
        }
        PatternViewDisplay();
    }

    private void PatternViewDisplay()
    {
        if (_pattern is null)
        {
            return;
        }
        _patternView = PatternMapper.ToViewModel(_pattern);
        PatternViewCanvas.SetPatternVies(_patternView);
        PatternViewCanvas.SetCamera(new Camera
        {
            CameraX = 0,
            CameraY = 0,
            Zoom = 10
        });
        ThreadInfoViewDisplay();
    }

    private void PatternCanvasMouseWheel(object sender, MouseWheelEventArgs e)
    {
        int zoom = e.Delta / 120;
        PatternViewCanvas.SetZoom(zoom, e.GetPosition(PatternViewCanvas));
        ThreadInfoView.SetZoom(zoom);
    }

    private void ThreadInfoViewDisplay()
    {
        if (_patternView is null)
        {
            return;
        }
        ThreadInfoView.SetPatternVies(_patternView);
    }
    

    private async Task PatternViewSave()
    {
        if (_pattern is null || _patternView is null)
        {
            return;
        }
        _seting.LastOpenedProject = await _fileWork.PatternSaveAsync(PatternMapper.ToPattern(_patternView , _pattern), _seting.LastOpenedProject);
    }

    private async Task AutoLoadProjectFiles()
    {
        string folder = _seting.ProjectsFolder;
        if (folder is null)
        {
            return;
        }
        _projectFiles = await _fileWork.GetProjectFiles(folder);
        Projects.ItemsSource = _projectFiles.Select(x => x.FileName);;
    }

    private void PatternViewCanvas_PatternViewChanged2(object? sender, Cell e)
    {
        ThreadInfoView.RecalculateCountThread(e);
    }

    private void ThreadInfoView_ThreadIndexDisplay(object? sender, int? e)
    {
        PatternViewCanvas.SetChosenThreadIndex(e);
    }

    private void ThreadInfoVies_ThreadIndexChange(object? sender, sbyte? e)
    {
        PatternViewCanvas.SetThreadIndexChange(e);
    }

    private async Task AutoSavePatternAsync()
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync())
        {
            await PatternViewSave();
        }
    }
}