using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TTImageProcessor;

namespace TTBoardReader
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnImagePathBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                CheckFileExists = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = "jpg",
                DereferenceLinks = true,
                Filter = string.Concat("All Supported Image Types "
                    + "(*.bmp;*.gif;*.jpg;*.png;*.tif)"
                    + "|*.pmg;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
                    + "|Bitmap Files (*.bmp)"
                    + "|*.bmp"
                    + "|GIF Files (*.gif)"
                    + "|*.gif"
                    + "|JPG Files (*.jpg)"
                    + "|*.jpg;*.jpeg"
                    + "|Portable Network Graphic Files (*.png)"
                    + "|*.png"
                    + "|Tagged Image Files (*.tiff)"
                    + "|*.tif;*.tiff"
                    + "|All Files (*.*)"
                    + "|*.*"),
                FilterIndex = 1,
                Multiselect = false,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
                Title = "Select camera image of TT board",
                ValidateNames = true,
                SupportMultiDottedExtensions = true,
                RestoreDirectory = true
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            txtImagePath.Text = dlg.FileName;

            if (string.IsNullOrWhiteSpace(txtMetaImageFolder.Text))
            {
                try
                {
                    txtMetaImageFolder.Text = Path.GetDirectoryName(dlg.FileName);
                }
                catch (Exception ex)
                {
                    Logger.Info(1807162029, $"Could not get directory name from selected filename:\r\n{ex}");
                }
            }
        }

        private void btnMetaBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                Description = "Meta images will go here",
                ShowNewFolderButton = true
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            txtMetaImageFolder.Text = dlg.SelectedPath;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            txtResults.Text = "";
            AddMessage("Starting.");
            var original = ImageUtil.LoadBitmap(txtImagePath.Text);
            var baseFilename = Path.GetFileNameWithoutExtension(txtImagePath.Text);
            AddMessage("Image loaded.");

            var decimate3bit = ImageUtil.DecimateColors(original, 0x80, 0x80, 0x80);
            var decimate6bit = ImageUtil.DecimateColors(original, 0xC0, 0xC0, 0xC0);
            var decimate9bit = ImageUtil.DecimateColors(original, 0xE0, 0xE0, 0xE0);
            AddMessage("Created decimated images.");

            var baseDir = string.IsNullOrWhiteSpace(txtMetaImageFolder.Text) ? Environment.CurrentDirectory : txtMetaImageFolder.Text;
            var d3filename = Path.Combine(baseDir, $"{baseFilename}-decimate-3bit.bmp");
            var d6filename = Path.Combine(baseDir, $"{baseFilename}-decimate-6bit.bmp");
            var d9filename = Path.Combine(baseDir, $"{baseFilename}-decimate-9bit.bmp");

            decimate3bit?.Save(d3filename);
            decimate6bit?.Save(d6filename);
            decimate9bit?.Save(d9filename);
            AddMessage("Saved decimated images.");


            AddMessage("Done (for now . . .)");
        }

        private void AddMessage(string message)
        {
            txtResults.Text += $"{DateTime.Now.ToString("HH:mm:ss")} {message}\r\n";
            Application.DoEvents(); //this is super lazy -- need to use async/await
        }
    }
}
