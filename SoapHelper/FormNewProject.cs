using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SoapHelper.Data;
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
    public partial class FormNewProject : Form
    {
        public FormNewProject()
        {
            InitializeComponent();
        }

        public FormNewProject(WebServices webserv)
        {
            InitializeComponent();
            project = webserv;
        }

        WebServices project;

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool error = false;
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                errorProvider1.SetError(txtUrl, "Web servis adresi girilmedi!");
                error = true;
            }

            if (txtUrl.Text.IndexOf("http") == -1 && txtUrl.Text.IndexOf("asmx") == -1)
            {
                errorProvider1.SetError(txtUrl, "Web servis adresi hatalı!");
                error = true;
            }

            if (string.IsNullOrWhiteSpace(txtNamespace.Text))
            {
                errorProvider1.SetError(txtNamespace, "Proje namespace girilmedi!");
                error = true;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Proje adı girilmedi!");
                error = true;
            }

            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                errorProvider1.SetError(txtPath, "Proje yolu girilmedi!");
                error = true;
            }

            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                errorProvider1.SetError(comboBox1, "Proje kategorisi girilmedi!");
                error = true;
            }

            if (error) return;

            if(project == null)
            {
                project = new WebServices(XpoDefault.Session);
            }

            Categories category = XpoDefault.Session.FindObject<Categories>(CriteriaOperator.Parse("Name = ?", comboBox1.Text.ToUpper()));
            if (category == null)
            {
                category = new Categories(XpoDefault.Session);
                category.Name = comboBox1.Text.ToUpper(); ;
                category.Save();
            }

            project.Category = category;
            project.Name = txtName.Text;
            project.PackageName = txtNamespace.Text;
            project.Path = txtPath.Text;
            project.Url = txtUrl.Text;
            project.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }

        private void FormNewProject_Load(object sender, EventArgs e)
        {
            try
            {
                var categories = (from q in new XPQuery<Categories>(XpoDefault.Session)
                                  orderby q.Name descending
                                  select q.Name).ToList();

                if (categories != null && categories.Count > 0)
                {
                    foreach (string s in categories)
                    {
                        comboBox1.Items.Add(s);
                    }
                    comboBox1.SelectedIndex = 0;
                }

                if (project != null)
                {
                    txtName.Text = project.Name;
                    txtNamespace.Text = project.PackageName;
                    txtPath.Text = project.Path;
                    txtUrl.Text = project.Url;
                    if (project.Category != null)
                        comboBox1.Text = project.Category.Name;
                }
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
        }

        private void btnfolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fl = new FolderBrowserDialog();
            fl.Description = "Proje dizini seçin";
            fl.ShowNewFolderButton = true;
            if (fl.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fl.SelectedPath;
            }
        }
    }
}
