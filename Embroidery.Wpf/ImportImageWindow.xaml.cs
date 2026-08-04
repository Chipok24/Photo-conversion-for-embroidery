using System.Windows;
using System.Windows.Automation;
using Microsoft.Win32;

namespace Embroidery.Wpf;

public partial class ImportImageWindow : Window
{
    public int HeightSM { get; private set; }
    public int WidthSM { get; private set; }
    public int ColorCount { get; private set; }

    public string FilePath { get; private set;}
    public ImportImageWindow()
    {
        InitializeComponent();
    }
    
    public void Accept_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(HeightSMBox.Text, out int HSM) || !int.TryParse(WidthSMBox.Text, out int WSM) || !int.TryParse(ColorCountBox.Text, out int CC) || FilePath is null )
        {
            MessageBox.Show("Введите число / Выберите изображение");
            return;
        }
        HeightSM = Math.Abs(HSM);
        WidthSM = Math.Abs(WSM);
        ColorCount = Math.Abs(CC);
        this.DialogResult = true;
    }

    public void ChooseImage_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        bool? result = openFileDialog.ShowDialog();
        if (result == true)
        {
            string selectPathFile = openFileDialog.FileName;
            FilePath = selectPathFile;
        }
    }
}