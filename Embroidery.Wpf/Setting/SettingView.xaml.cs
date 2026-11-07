using System.Diagnostics;
using System.Windows;
using Embroidery.Wpf.Settings;
using Microsoft.Win32;

namespace Embroidery.Wpf;

public partial class SettingView : Window
{
    private Setting _setting;
    public SettingView(Setting setting)
    {
        _setting = setting;
        InitializeComponent();
    }

    public void ChooseProjectSaves_Click(object sender, RoutedEventArgs e)
    {
        string? selectFolder = ChooseFolder();
        if (selectFolder is null)
        {
            return;
        }
        SelectFolderSaves.Text += selectFolder;
        _setting.SetProjectsFolder(selectFolder);
    }

    public void ChooseBackup_Click(object sender, RoutedEventArgs e)
    {
        string? selectFolder = ChooseFolder();
        if (selectFolder is null)
        {
            return;
        }
        SelectFolderBackup.Text += selectFolder;
        _setting.SetBackups(selectFolder);
    }

    public async void Save_Click(object sender, RoutedEventArgs e)
    {
        SettingsManager sM = new SettingsManager();
        await sM.SaveSetting(_setting);
        this.DialogResult = true;
    }

    private string? ChooseFolder()
    {
        OpenFolderDialog oFD = new OpenFolderDialog();
        bool? result = oFD.ShowDialog();
        if (result is true)
        {
            return oFD.FolderName;
        }
        return null;
    }
}