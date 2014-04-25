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
            updateTitle();
            mainTextBox.TextChanged += mainTextBox_TextChanged;
        }

        private bool dirty = false;
        private string openFilePath = string.Empty;
        private string fileName = "Untitled";

        private OpenFileDialog openFileDialog = null;
        private SaveFileDialog saveFileDialog = null;

        void mainTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!dirty)
            {
                dirty = true;
                this.Text += "*";
            }
        }

        private void updateTitle()
        {
            this.Text = fileName + " - Note";
        }

        private bool promptSaveContinue()
        {
            if (dirty)
            {
                PromptSaveDialog.Instance.FileName = fileName;
                switch (PromptSaveDialog.Instance.ShowDialog())
                {
                    case DialogResult.OK:
                        saveToFile();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return false;
                }
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (promptSaveContinue())
            {
                openFilePath = string.Empty;
                mainTextBox.Text = "";

                dirty = false;
                fileName = "Untitled";
                updateTitle();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (promptSaveContinue())
            {
                if (openFileDialog == null)
                {
                    openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                }
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openFilePath = openFileDialog.FileName;
                    using (StreamReader reader = new StreamReader(openFilePath))
                        mainTextBox.Text = reader.ReadToEnd();

                    fileName = System.Text.RegularExpressions.Regex.Match(
                        openFilePath, @"([^\\]*)(?=\.)").Value;
                    updateTitle();
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
                fileName = System.Text.RegularExpressions.Regex.Match(
                    openFilePath, @"([^\\]*)(?=\.)").Value;
                updateTitle();
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
            AboutNoteDialog.Instance.Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if(fd.ShowDialog()== DialogResult.OK)
            {
                mainTextBox.Font = fd.Font;
            }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (!promptSaveContinue())
                e.Cancel = true;
        }

    }
}
