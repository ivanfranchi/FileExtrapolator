using System.IO;
using System.Diagnostics;

namespace ExtrapolateFiles
{
	public class ReadFolder
	{
		public ReadFolder(FolderBrowserDialog fbd)
		{
			Debug.WriteLine("Reading folder: " + fbd.SelectedPath);
		}
	}
}