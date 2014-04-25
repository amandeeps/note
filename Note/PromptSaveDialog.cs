using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note
{
    public partial class PromptSaveDialog : Form
    {
        private static PromptSaveDialog instance = new PromptSaveDialog();
        public static PromptSaveDialog Instance
        {
            get { return instance; }
        }

        public PromptSaveDialog()
        {
            InitializeComponent();
        }

        private string msg = "Do you want to save changes to ";
        public string FileName
        {
            set
            {
                label1.Text = msg + value + "?";
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void dontButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
