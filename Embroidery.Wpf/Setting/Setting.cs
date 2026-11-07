using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Embroidery.Wpf.Settings;

public class Setting
{
    [JsonInclude]
    public string ProjectsFolder { get; private set; }
    public string LastOpenedProject { get; set; }
    [JsonInclude]
    public string Backups {get; private set;}

    public void SetProjectsFolder(string folder)
    {
        if (!string.IsNullOrWhiteSpace(folder))
        {
            ProjectsFolder = folder;
        }
    }

    public void SetBackups(string folder)
    {
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Backups = folder;
        }
    }
}