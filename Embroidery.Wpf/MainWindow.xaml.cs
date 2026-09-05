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

    }

    public async void Button_Save(object? sender, RoutedEventArgs e)
    {
        if (_patternView is null || _pattern is null)
        {
            return;
        }
        await _fileWork.PatternSaveAsync(PatternMapper.ToPattern(_patternView , _pattern), _seting.LastOpenedProject);
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
            await AutoLoadProjectFiles();
        }
    }

    public async void ProjectsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (Projects.SelectedIndex is int project)
        {
            await PatternViewSave();
            await ProjectFileDisplay(_projectFiles[project].FilePath);
            _seting.LastOpenedProject = _projectFiles[project].FilePath;
            _settingsManager.SaveSetting(_seting);
        }
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        _seting = await _settingsManager.LoadSetting();
        await AutoLoadProjectFiles();
        await ProjectFileDisplay(_seting.LastOpenedProject);
    }

    private async void MainWindow_Save(object? sender, RoutedEventArgs e)
    {
        await PatternViewSave();
    }

    private void DisplayPattern(string PathFile, int HSM, int WSM, int CountColor)
    {
        
        _pattern = _embroideryGenerator.ImageAlgoritm(PathFile, CountColor, WSM, HSM);
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
        _projectFiles = await _fileWork.GetProjectFiles(_seting.ProjectsFolder);
        Projects.ItemsSource = _projectFiles.Select(x => x.FileName);;
    }
}