using Embroidery.Core.Models;
using EmbroiderySaveAndLoad.Core.Models;
using SixLabors.ImageSharp.ColorSpaces;
using System.Diagnostics;
using System.Text.Json;

namespace EmbroiderySaveAndLoad.Core.FileWork;

public class FileWork
{
    public async Task PatternSaveAsync(Pattern pattern)
    {
        Cell[][] cells = ToJagged(pattern.Cells);

        List<ModelColorSave> modelColorSaves = new List<ModelColorSave>();
        foreach (var item in pattern.Threads)
        {
            CieLabSave cieLabSave = new CieLabSave(item.CieLabColor.L, item.CieLabColor.A, item.CieLabColor.B);
            RgbSave rgbSave = new RgbSave(item.RgbColor.R, item.RgbColor.G, item.RgbColor.B);
            modelColorSaves.Add(new ModelColorSave(cieLabSave, rgbSave, item.IdThread, ToJagged(item.Symbol)));
        }

        PatternSave patternSave = new PatternSave(){Threads = modelColorSaves, Cells = cells}; 

        await using (FileStream fileStream = new FileStream("F:\\code\\ProjectImage\\Embroidery\\Saves\\Embroidery.json", FileMode.OpenOrCreate))
        {
            await JsonSerializer.SerializeAsync(fileStream, patternSave);
        }
    }

    public async Task<Pattern> PatternLoadAsync()
    {
        PatternSave? patternSave;
        await using (FileStream fileStream = new FileStream("F:\\code\\ProjectImage\\Embroidery\\Saves\\Embroidery.json", FileMode.Open))
        {
            patternSave = await JsonSerializer.DeserializeAsync<PatternSave>(fileStream);
        }

        if (patternSave is null) throw new InvalidDataException("Не удалось загрузить Pattern.");

        Cell[,] cells = To2D(patternSave.Cells);
        List<ModelColor> modelColors = new List<ModelColor>();
        foreach (var item in patternSave.Threads)
        {
            CieLab cieLab = new CieLab(item.CieLabColor.L, item.CieLabColor.A, item.CieLabColor.B);
            Rgb rgb = new Rgb(item.RgbColor.R, item.RgbColor.G, item.RgbColor.B);
            modelColors.Add(new ModelColor(cieLab, rgb, item.IdThread, To2D(item.Symbol)));
        }

        Dictionary<byte, int> threadsQuantity = new Dictionary<byte, int>();
        foreach (var cell in cells)
        {
            if (cell.ThreadIndex is null)
            {
                continue;
            }

            if (threadsQuantity.ContainsKey((byte)cell.ThreadIndex))
            {
                threadsQuantity[(byte)cell.ThreadIndex] += 1;
            }
            else
            {
                threadsQuantity[(byte)cell.ThreadIndex] = 1;
            }
        }
        
        Pattern pattern = new Pattern(){Threads = modelColors, ThreadsQuantity = threadsQuantity, Cells = cells};
        return pattern;
    }

    private T[][] ToJagged<T>(T[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        T[][] result = new T[width][];

        for (int x = 0; x < width; x++)
        {
            result[x] = new T[height];

            for (int y = 0; y < height; y++)
            {
                result[x][y] = values[x, y];
            }
        }

        return result;
    }

    private T[,] To2D<T>(T[][] values)
    {
        int width = values.Length;
        int height = values[0].Length;

        T[,] result = new T[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = values[x][y];
            }
        }

        return result;
    }
}
