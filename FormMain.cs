using System;
using System.IO;
using System.Windows.Forms;
using Aladdin.HASP;

namespace Rakhmax
{
    public partial class FormMain : Form
    {
        private static string _openedFile = null;
        private static readonly string _vendorCode =
            "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsV" +
            "vIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrB" +
            "rh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6" +
            "AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf" +
            "8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1" +
            "e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBh" +
            "aJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SK" +
            "unFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bL" +
            "Cx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwL" +
            "zaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9s" +
            "JN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaT" +
            "tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y";

        public FormMain()
        {
            InitializeComponent();

            Properties.Settings.Default.number_of_starts++;
            Properties.Settings.Default.Save();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Clear();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            HaspFeature feature = HaspFeature.Default;

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(_vendorCode);

            var haspStatus = getHaspStatusMessage(status);

            if (Properties.Settings.Default.number_of_starts < 5 && haspStatus == "OK")
            {
                Properties.Settings.Default.hasp_plugged_in = true;
            }

            if (haspStatus != "OK" && Properties.Settings.Default.hasp_plugged_in)
            {
                var res = MessageBox.Show(
                    "The key was plugged in once. Plug it in again",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                if (res == DialogResult.OK)
                {
                    Close();
                }
            }

            if (Properties.Settings.Default.number_of_starts >= 5 && haspStatus != "OK")
            {
                var res = MessageBox.Show(
                    haspStatus,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                if (res == DialogResult.OK)
                {
                    Close();
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            HaspFeature feature = HaspFeature.Default;

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Logout();

            getHaspStatusMessage(status);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            HaspFeature feature = HaspFeature.Default;

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(_vendorCode);

            var haspStatus = getHaspStatusMessage(status);

            if (haspStatus != "OK")
            {
                var res = MessageBox.Show(
                    "HASP HL key has been removed. If you need to save the file, click Cancel and save it",
                    "Error",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error
                );

                if (res == DialogResult.OK)
                {
                    Close();
                } 
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _openedFile = openFileDialog.FileName;
                try
                {
                    richTextBoxMain.Text = File.ReadAllText(_openedFile);
                } catch (Exception err)
                {
                    throw err;
                }
            }
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_openedFile))
            {
                SaveFile();
            }
            else
            {
                File.WriteAllText(_openedFile, richTextBoxMain.Text);
            }
        }

        private void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            HaspFeature feature = HaspFeature.Default;

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(_vendorCode);

            var haspStatus = getHaspStatusMessage(status);

            if (haspStatus != "OK")
            {
                var res = MessageBox.Show(
                    "HASP HL key has been removed. If you need to save the file, click Cancel and save it",
                    "Error",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error
                );

                if (res == DialogResult.OK)
                {
                    Close();
                }
            }
            SaveFile();
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_openedFile))
            {
                var res = MessageBox.Show(
                    "Would you like to save the file?",
                    "Warning",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                switch (res)
                {
                    case DialogResult.Yes:
                        File.WriteAllText(_openedFile, richTextBoxMain.Text);
                        richTextBoxMain.Text = "";
                        _openedFile = null;
                        break;
                    case DialogResult.No:
                        richTextBoxMain.Text = "";
                        _openedFile = null;
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }

        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "*.txt",
                Filter = "Text files|*.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, true))
                {
                    sw.WriteLine(richTextBoxMain.Text);
                    sw.Close();
                }
            }
        }

        private static string getHaspStatusMessage(HaspStatus status)
        {
            string statusText;

            switch (status)
            {
                case HaspStatus.ContainerNotFound:
                    statusText = "Evaluation is expired! Plug in your HASP HL key and restart the application.";
                    break;
                case HaspStatus.DriverNotFound:
                    statusText = "Please install HASP HL driver and try again";
                    break;
                case HaspStatus.StatusOk:
                    statusText = "OK";
                    break;
                default:
                    statusText = status.ToString();
                    break;
            }

            return statusText;
        }
    }
}
