using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Embroidery.Wpf.Settings;
using System.Text.Json;

namespace Embroidery.Wpf.Settings;

public class SettingsManager
{
    private readonly string _path = Path.Combine(AppContext.BaseDirectory, "Setting", "Setting.json");

    public SettingsManager() { }

    public async Task<Setting> LoadSetting()
    {
        Setting? setting;
        await using (FileStream fileStream = new FileStream(_path, FileMode.Open))
        {
            setting = await JsonSerializer.DeserializeAsync<Setting>(fileStream);
        }
        if (setting is null) throw new InvalidDataException("Не удалось загрузить настройки");
        return setting;
    }

    public async void SaveSetting(Setting setting)
    {
        await using (FileStream fileStream = new FileStream(_path, FileMode.Create))
        {
            await JsonSerializer.SerializeAsync<Setting>(fileStream, setting);
        }
    }
}