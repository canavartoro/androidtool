using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapHelper
{
    public partial class JavaClassForm : Form
    {
        public JavaClassForm()
        {
            InitializeComponent();
        }

        public string ClassText { get; set; }

        public JavaClassForm(string classtext)
        {
            InitializeComponent();
            this.ClassText = classtext;
        }

        private void JavaClassForm_Load(object sender, EventArgs e)
        {
            this.fastColoredTextBox1.Text = this.ClassText;
            this.fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.CSharp;
        }
    }
}
