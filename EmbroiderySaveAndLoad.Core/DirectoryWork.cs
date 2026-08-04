using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace EmbroiderySaveAndLoad.Core;

public class DirectoryWork
{
    public async Task EnsureExists(string pathDirectory)
    {
        Directory.CreateDirectory(pathDirectory);
    }
}
