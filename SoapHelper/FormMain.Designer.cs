namespace SoapHelper
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.güncelleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnolustur = new System.Windows.Forms.ToolStripMenuItem();
            this.düzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.silToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnnew = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnclass = new System.Windows.Forms.ToolStripMenuItem();
            this.btnadapter = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 906);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1503, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(18, 20);
            this.lblInfo.Text = "...";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 906);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.treeView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 27);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(415, 581);
            this.panel3.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 1;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(415, 581);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.güncelleToolStripMenuItem,
            this.btnolustur,
            this.düzenleToolStripMenuItem,
            this.silToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 108);
            // 
            // güncelleToolStripMenuItem
            // 
            this.güncelleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("güncelleToolStripMenuItem.Image")));
            this.güncelleToolStripMenuItem.Name = "güncelleToolStripMenuItem";
            this.güncelleToolStripMenuItem.Size = new System.Drawing.Size(139, 26);
            this.güncelleToolStripMenuItem.Text = "Güncelle";
            this.güncelleToolStripMenuItem.Click += new System.EventHandler(this.güncelleToolStripMenuItem_Click);
            // 
            // btnolustur
            // 
            this.btnolustur.Image = ((System.Drawing.Image)(resources.GetObject("btnolustur.Image")));
            this.btnolustur.Name = "btnolustur";
            this.btnolustur.Size = new System.Drawing.Size(139, 26);
            this.btnolustur.Text = "Oluştur";
            this.btnolustur.Click += new System.EventHandler(this.btnolustur_Click);
            // 
            // düzenleToolStripMenuItem
            // 
            this.düzenleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("düzenleToolStripMenuItem.Image")));
            this.düzenleToolStripMenuItem.Name = "düzenleToolStripMenuItem";
            this.düzenleToolStripMenuItem.Size = new System.Drawing.Size(139, 26);
            this.düzenleToolStripMenuItem.Text = "Düzenle";
            this.düzenleToolStripMenuItem.Click += new System.EventHandler(this.düzenleToolStripMenuItem_Click);
            // 
            // silToolStripMenuItem
            // 
            this.silToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("silToolStripMenuItem.Image")));
            this.silToolStripMenuItem.Name = "silToolStripMenuItem";
            this.silToolStripMenuItem.Size = new System.Drawing.Size(139, 26);
            this.silToolStripMenuItem.Text = "Sil";
            this.silToolStripMenuItem.Click += new System.EventHandler(this.silToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "iconfinder_File_Explorer_272535.png");
            this.imageList1.Images.SetKeyName(1, "iconfinder_folder_299060.png");
            this.imageList1.Images.SetKeyName(2, "iconfinder_folder-blue_285658.png");
            this.imageList1.Images.SetKeyName(3, "iconfinder_Internet_Line-02_1587499.png");
            this.imageList1.Images.SetKeyName(4, "iconfinder_internet-web-browser_118810.png");
            this.imageList1.Images.SetKeyName(5, "iconfinder_stock_view-function-selection_21861.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnnew});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(415, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnnew
            // 
            this.btnnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnnew.Image = ((System.Drawing.Image)(resources.GetObject("btnnew.Image")));
            this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(24, 24);
            this.btnnew.Text = "toolStripButton1";
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 608);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(415, 8);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 616);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(415, 290);
            this.panel2.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(415, 290);
            this.propertyGrid1.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(415, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(8, 906);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.richTextBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(423, 616);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1080, 290);
            this.panel4.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1080, 290);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Location = new System.Drawing.Point(423, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1080, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(423, 608);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(1080, 8);
            this.splitter3.TabIndex = 5;
            this.splitter3.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.listView1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(423, 25);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1080, 583);
            this.panel5.TabIndex = 6;
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.ContextMenuStrip = this.contextMenuStrip2;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1080, 583);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sıra";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 140;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Array";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Enum";
            this.columnHeader4.Width = 50;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Base";
            this.columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Element";
            this.columnHeader6.Width = 200;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Properties";
            this.columnHeader7.Width = 400;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnclass,
            this.btnadapter});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(199, 84);
            // 
            // btnclass
            // 
            this.btnclass.Image = ((System.Drawing.Image)(resources.GetObject("btnclass.Image")));
            this.btnclass.Name = "btnclass";
            this.btnclass.Size = new System.Drawing.Size(198, 26);
            this.btnclass.Text = "Java Class Oluştur";
            this.btnclass.Click += new System.EventHandler(this.btnclass_Click);
            // 
            // btnadapter
            // 
            this.btnadapter.Image = ((System.Drawing.Image)(resources.GetObject("btnadapter.Image")));
            this.btnadapter.Name = "btnadapter";
            this.btnadapter.Size = new System.Drawing.Size(198, 26);
            this.btnadapter.Text = "Adapter Oluştur";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1503, 931);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FormMain";
            this.Text = "Android Tool";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStripButton btnnew;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem güncelleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem düzenleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem silToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        private System.Windows.Forms.ToolStripMenuItem btnolustur;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem btnclass;
        private System.Windows.Forms.ToolStripMenuItem btnadapter;
    }
}

