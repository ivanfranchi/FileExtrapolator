using FormExtrapolate.Utils;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FormExtrapolate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string OutputFolder = "";

        private void btnInput_Click(object sender, EventArgs e)
        {
            listFiles.Items.Clear();

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    label1.Text = "input: " + fbd.SelectedPath;

                    foreach (var files in ReadFiles.ParseFolder(fbd))
                    {
                        listFiles.Items.AddRange(files);
                    }
                }
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    OutputFolder = fbd.SelectedPath;
                }
            }

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (listFiles.Items.Count == 0)
            {
                MessageBox.Show("Select files to copy first!");
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputFolder))
            {
                MessageBox.Show("Select an output first!");
                return;
            }

            foreach (var fileString in listFiles.Items)
            {
                var fullPath = fileString.ToString();
                var fileName = Path.GetFileName(fullPath);

                try
                {
                    File.Copy(fullPath, Path.Combine(OutputFolder, fileName));
                }
                catch (IOException ex)
                {
                    if (ex.Message.ToLower().Contains("already exists"))
                    {
                        byte[] firstHash = MD5.Create().ComputeHash(File.OpenRead(fullPath));
                        byte[] secondHash = MD5.Create().ComputeHash(File.OpenRead(Path.Combine(OutputFolder, fileName)));

                        if(firstHash.SequenceEqual(secondHash))
                        {
                            continue;
                        }
                        else
                        {
                            File.Copy(fullPath, Path.Combine(OutputFolder, $"{Guid.NewGuid()}-{fileName}"));
                        }
                    }
                }
            }

            MessageBox.Show($"Copied {listFiles.Items.Count} files!");
        }
    }
}
