using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FormExtrapolate.Utils
{
    public static class ReadFiles
    {
        public static IEnumerable<string[]> ParseFolder(FolderBrowserDialog fbd)
        {
            var path = fbd.SelectedPath;
            var newFiles = Directory.GetFiles(path);
            yield return newFiles;

            var directories = Directory.GetDirectories(path).ToList();
            if (directories.Any())
            {
                for (var i = directories.Count - 1; i >= 0; i--)
                {
                    newFiles = Directory.GetFiles(directories[i]);
                    var newDirs = Directory.GetDirectories(directories[i]);
                    directories.InsertRange(0, newDirs);
                    i += newDirs.Length;
                    yield return newFiles;
                }
            }
        }
    }
}
