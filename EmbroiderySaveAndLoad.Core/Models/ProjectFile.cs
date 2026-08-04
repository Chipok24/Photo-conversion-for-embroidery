using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbroiderySaveAndLoad.Core.Models;

public record class ProjectFile
{
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string FullFileName { get; set; }
    public string ImagePath { get; set; } = null;

    public ProjectFile(string FilePath, string FileName, string FullFileName)
    {
        this.FilePath = FilePath;
        this.FileName = FileName;
        this.FullFileName = FullFileName;
    }
}
