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
    public partial class AboutNoteDialog : Form
    {
        private static AboutNoteDialog instance = new AboutNoteDialog();
        public static AboutNoteDialog Instance { get { return instance; } }
        private AboutNoteDialog()
        {
            InitializeComponent();
        }

    }
}
