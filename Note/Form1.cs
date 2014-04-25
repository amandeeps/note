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

namespace Note
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            updateTitle("Untitled");
            mainTextBox.TextChanged += mainTextBox_TextChanged;
        }

        private bool dirty = false;
        private string openFilePath = string.Empty;

        private OpenFileDialog openFileDialog = null;
        private SaveFileDialog saveFileDialog = null;

        void mainTextBox_TextChanged(object sender, EventArgs e)
        {
            setDirty();
        }

        private void updateTitle(string file)
        {
            this.Text = file + " - Note";
        }

        private void setDirty()
        {
            if (!dirty)
            {
                dirty = true;
                this.Text += "*";
            }
        }

        private bool promptSaveContinue()
        {
            //TODO Show Dialog
            saveToFile();
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (promptSaveContinue())
            {
                openFilePath = string.Empty;
                mainTextBox.Text = "";

                dirty = false;
                updateTitle("Untitled");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog == null)
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            }
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (promptSaveContinue())
                {
                    openFilePath = openFileDialog.FileName;
                    using (StreamReader reader = new StreamReader(openFilePath))
                        mainTextBox.Text = reader.ReadToEnd();

                    string safeName = openFileDialog.SafeFileName;
                    updateTitle(safeName.Remove(safeName.IndexOf('.')));
                    dirty = false;
                }
            }
        }

        private void saveHelper()
        {
            using (StreamWriter writer = new StreamWriter(openFilePath, false))
                writer.WriteAsync(mainTextBox.Text);
            dirty = false;
        }

        private void saveToFile()
        {
            if (dirty)
            {
                if (string.IsNullOrEmpty(openFilePath))
                {
                    saveToNewFile();
                }
                else
                {
                    saveHelper();
                    this.Text = this.Text.Remove(this.Text.Length - 1);
                }
            }
        }

        private void saveToNewFile()
        {
            if (saveFileDialog == null)
            {
                saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFilePath = saveFileDialog.FileName;
                saveHelper();
                string safeName = openFilePath;
                safeName = System.Text.RegularExpressions.Regex.Match(
                    safeName, @"([^\\]*)(?=\.)").Value;
                updateTitle(safeName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToNewFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(promptSaveContinue())
            {
                Application.Exit();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Paste(Clipboard.GetText());
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO Show About Dialog
        }

    }
}
