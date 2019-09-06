using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HSIS_lab_1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            Properties.Settings.Default.number_of_starts++;
            Properties.Settings.Default.Save();
        }

        private void RichTextBoxMain_TextChanged(object sender, EventArgs e)
        {

        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Clear();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            LabelTrialInfo.Text = Properties.Settings.Default.number_of_starts.ToString();

            if (Properties.Settings.Default.number_of_starts >= 5)
            {
                var res = MessageBox.Show(
                    "Evaluation is expired! Insert your HASP HL key",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (res == DialogResult.OK)
                {
                    Close();
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox1.Checked)
            {
                e.Cancel = true;
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                LabelTrialInfo.Text = "";
            }
            else
            {
                LabelTrialInfo.Text = "You are using trial version";
                LabelTrialInfo.ForeColor = Color.Red;
            }
        }
    }
}
