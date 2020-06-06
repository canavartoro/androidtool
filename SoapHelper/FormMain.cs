using DevExpress.Xpo;
using SoapHelper.Data;
using SoapHelper.Wsdl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapHelper
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        WebServices webserv = null;

        private void btnnew_Click(object sender, EventArgs e)
        {
            new FormNewProject().ShowDialog();
            LoadProjects();
        }

        private void LoadProjects()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();

                var projects = (from q in new XPQuery<WebServices>(XpoDefault.Session)
                                orderby q.Category descending
                                select q).Take(100).ToList();

                var categories = (from q in projects group q by q.Category.Name into g select g.Key).ToList();

                foreach (string s in categories)
                {
                    TreeNode n = new TreeNode();
                    n.Text = s;
                    n.ImageIndex = 1;
                    n.SelectedImageIndex = 0;

                    var project = (from q in projects where q.Category.Name == s select q).ToList();
                    if (project != null && project.Count > 0)
                    {
                        foreach (var p in project)
                        {
                            TreeNode node = new TreeNode();
                            node.Text = p.Name;
                            node.Tag = p;
                            node.ImageIndex = 3;
                            node.SelectedImageIndex = 4;
                            n.Nodes.Add(node);
                        }
                        Application.DoEvents();
                    }

                    treeView1.Nodes.Add(n);
                }


            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
            finally
            {
                treeView1.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadFunctions()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                webserv = treeView1.SelectedNode.Tag as WebServices;
                if (webserv != null)
                {
                    TreeNode node = treeView1.SelectedNode;
                    node.Nodes.Clear();

                    Utility.WriteTrace(string.Format("Seçilen Proje: {0}, Web Servis:{1}, Metod: {2}", webserv.Name, webserv.ServiceName, webserv.Functions.Count));
                    treeView1.BeginUpdate();

                    var funcwithorder = webserv.Functions.OrderBy(x => x.Name).ToList();

                    for (int i = 0; i < funcwithorder.Count; i++)
                    {
                        TreeNode subnode = new TreeNode();
                        subnode.Tag = funcwithorder[i];
                        subnode.Text = string.Concat(funcwithorder[i].Name, "(", funcwithorder[i].InputType, ")", " -> ", funcwithorder[i].OutputType);
                        subnode.ToolTipText = funcwithorder[i].RegisterClasses;
                        subnode.ImageIndex = 5;
                        subnode.SelectedImageIndex = 5;
                        subnode.Checked = funcwithorder[i].Output;
                        node.Nodes.Add(subnode);
                    }
                }
                else
                {
                    Utility.WriteTrace(string.Format("Seçilen Kategori: {0}", treeView1.SelectedNode.Text));
                }
                propertyGrid1.SelectedObject = webserv;
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
            finally
            {
                Cursor = Cursors.Default;
                treeView1.EndUpdate();
            }
        }

        private void LoadClasses()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (webserv != null)
                {
                    Utility.WriteTrace(string.Format("Seçilen Proje: {0}, Web Servis:{1}, Metod: {2}, Class: {3}", webserv.Name, webserv.ServiceName, webserv.Functions.Count, webserv.SoapClasses.Count));
                    listView1.Tag = 1;
                    listView1.BeginUpdate();
                    listView1.Items.Clear();

                    var classwithorder = webserv.SoapClasses.OrderBy(x => x.Name).ToList();

                    for (int i = 0; i < classwithorder.Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = Convert.ToString(i + 1);
                        item.Tag = classwithorder[i];
                        item.SubItems.Add(classwithorder[i].Name);
                        item.SubItems.Add(classwithorder[i].IsArray ? "√" : "");
                        item.SubItems.Add(classwithorder[i].IsEnum ? "√" : "");
                        item.SubItems.Add(classwithorder[i].SuperClassType);
                        item.SubItems.Add(classwithorder[i].ElementType);
                        //item.SubItems.Add(classwithorder[i].PropertyText);
                        item.Checked = classwithorder[i].Output;
                        listView1.Items.Add(item);
                    }
                }
                else
                {
                    Utility.WriteTrace(string.Format("Seçilen Kategori: {0}", treeView1.SelectedNode.Text));
                }
                propertyGrid1.SelectedObject = webserv;
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
            finally
            {
                Cursor = Cursors.Default;
                listView1.EndUpdate();
                listView1.Tag = null;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Trace.Listeners.Add((TraceListener)new TextTraceListener(this.richTextBox1));
            LoadProjects();


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is WebServices)
                {
                    LoadFunctions();
                    LoadClasses();
                }
                else if (treeView1.SelectedNode.Tag is WebFunctions)
                {
                    WebFunctions webfunc = treeView1.SelectedNode.Tag as WebFunctions;
                    Utility.WriteTrace(string.Format("Seçilen Fonksiyon: {0}", webfunc.Name));
                    propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;
                }
                else
                {
                    Utility.WriteTrace(string.Format("Seçilen Kategori: {0}", treeView1.SelectedNode.Text));
                }
            }
            else
            {
                webserv = null;
                propertyGrid1.SelectedObject = null;
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                new FormNewProject(treeView1.SelectedNode.Tag as WebServices).ShowDialog();
                LoadProjects();
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                WebServices proj = treeView1.SelectedNode.Tag as WebServices;
                if (Utility.Sor("Proje silinsin mi?"))
                {
                    proj.Delete();
                    LoadProjects();
                }

            }
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                WebServices proj = treeView1.SelectedNode.Tag as WebServices;
                if (Utility.Sor("Proje güncellenecek onaylıyor musunuz?"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    WSDLParser parser = new WSDLParser(proj);

                    parser.Parse();

                    this.Cursor = Cursors.Default;
                }

            }
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.Index >= 0 && listView1.Tag == null)
            //{
            //    WebFunctions webfunc = listView1.Items[e.Index].Tag as WebFunctions;
            //    if (webfunc != null)
            //    {
            //        propertyGrid1.SelectedObject = webfunc;
            //        webfunc.Output = e.NewValue == CheckState.Checked;
            //        webfunc.Save();
            //        LoadFunctions();
            //    }
            //}
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                SoapClasses soapClass = listView1.Items[listView1.SelectedIndices[0]].Tag as SoapClasses;
                if (soapClass != null)
                {
                    propertyGrid1.SelectedObject = soapClass;
                    Utility.WriteTrace(string.Format("Seçilen Class: {0}, Property:{1}", soapClass.Name, soapClass.Properties.Count));
                }
            }
        }

        private void btnolustur_Click(object sender, EventArgs e)
        {
            try
            {
                if (webserv != null)
                {
                    Cursor = Cursors.WaitCursor;
                    SoapCreator creator = new SoapCreator(webserv);
                    creator.Create();
                }
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                WebFunctions webfunc = e.Node.Tag as WebFunctions;
                if (webfunc != null)
                {
                    propertyGrid1.SelectedObject = webfunc;
                    webfunc.Output = e.Node.Checked;
                    webfunc.Save();
                    //LoadFunctions();
                }
            }
        }

        private void btnclass_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                SoapClasses soapClass = listView1.Items[listView1.SelectedIndices[0]].Tag as SoapClasses;
                if (soapClass != null && webserv != null)
                {
                    JavaClassCreator jvcreator = new JavaClassCreator();
                    JavaClassForm frm = new JavaClassForm(jvcreator.Create(soapClass, webserv));
                    frm.Show();
                }
            }
        }
    }
}
