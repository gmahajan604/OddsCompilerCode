    namespace OddsCheckerCrawler
{
    partial class Coupons
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new CodersLab.Windows.Controls.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnxmlupdated = new System.Windows.Forms.Button();
            this.btnxmlall = new System.Windows.Forms.Button();
            this.btnaddmarket = new System.Windows.Forms.Button();
            this.btnupdate = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.lblmktavail = new System.Windows.Forms.Label();
            this.lblmktpriced = new System.Windows.Forms.Label();
            this.txtmatchdate = new System.Windows.Forms.TextBox();
            this.lblmatchnotset = new System.Windows.Forms.Label();
            this.tableLayoutNews = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutAttention = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutWarning = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.CouponMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutNews.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutAttention.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutWarning.SuspendLayout();
            this.panel1.SuspendLayout();
            this.CouponMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1276, 612);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.treeView1.SelectionMode = CodersLab.Windows.Controls.TreeViewSelectionMode.MultiSelectSameLevel;
            this.treeView1.Size = new System.Drawing.Size(231, 612);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick_1);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp_1);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutNews);
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutAttention);
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutWarning);
            this.splitContainer2.Size = new System.Drawing.Size(1041, 612);
            this.splitContainer2.SplitterDistance = 800;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 612);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Market Odds";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(794, 593);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 141);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(788, 440);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.SizeChanged += new System.EventHandler(this.flowLayoutPanel1_SizeChanged);
            // 
            // panel7
            // 
            this.panel7.AutoSize = true;
            this.panel7.Controls.Add(this.button1);
            this.panel7.Controls.Add(this.btnxmlupdated);
            this.panel7.Controls.Add(this.btnxmlall);
            this.panel7.Controls.Add(this.btnaddmarket);
            this.panel7.Controls.Add(this.btnupdate);
            this.panel7.Controls.Add(this.btnsave);
            this.panel7.Controls.Add(this.lblmktavail);
            this.panel7.Controls.Add(this.lblmktpriced);
            this.panel7.Controls.Add(this.txtmatchdate);
            this.panel7.Controls.Add(this.lblmatchnotset);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(788, 132);
            this.panel7.TabIndex = 1;
            // 
            // btnxmlupdated
            // 
            this.btnxmlupdated.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnxmlupdated.Location = new System.Drawing.Point(85, 88);
            this.btnxmlupdated.Name = "btnxmlupdated";
            this.btnxmlupdated.Size = new System.Drawing.Size(93, 23);
            this.btnxmlupdated.TabIndex = 7;
            this.btnxmlupdated.Text = "Updated XML";
            this.btnxmlupdated.UseVisualStyleBackColor = true;
            this.btnxmlupdated.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnxmlall
            // 
            this.btnxmlall.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnxmlall.Location = new System.Drawing.Point(18, 88);
            this.btnxmlall.Name = "btnxmlall";
            this.btnxmlall.Size = new System.Drawing.Size(51, 23);
            this.btnxmlall.TabIndex = 6;
            this.btnxmlall.Text = "XML";
            this.btnxmlall.UseVisualStyleBackColor = true;
            this.btnxmlall.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnaddmarket
            // 
            this.btnaddmarket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnaddmarket.AutoSize = true;
            this.btnaddmarket.Location = new System.Drawing.Point(603, 89);
            this.btnaddmarket.Name = "btnaddmarket";
            this.btnaddmarket.Size = new System.Drawing.Size(75, 23);
            this.btnaddmarket.TabIndex = 0;
            this.btnaddmarket.Text = "Add Market";
            this.btnaddmarket.UseVisualStyleBackColor = true;
            this.btnaddmarket.Click += new System.EventHandler(this.btnaddmarket_Click);
            // 
            // btnupdate
            // 
            this.btnupdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnupdate.AutoSize = true;
            this.btnupdate.Location = new System.Drawing.Point(696, 89);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 5;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // btnsave
            // 
            this.btnsave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsave.AutoSize = true;
            this.btnsave.Location = new System.Drawing.Point(516, 89);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 4;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // lblmktavail
            // 
            this.lblmktavail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblmktavail.AutoSize = true;
            this.lblmktavail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmktavail.Location = new System.Drawing.Point(513, 43);
            this.lblmktavail.Name = "lblmktavail";
            this.lblmktavail.Size = new System.Drawing.Size(119, 16);
            this.lblmktavail.TabIndex = 3;
            this.lblmktavail.Text = "Markets Available:";
            // 
            // lblmktpriced
            // 
            this.lblmktpriced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblmktpriced.AutoSize = true;
            this.lblmktpriced.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmktpriced.Location = new System.Drawing.Point(513, 18);
            this.lblmktpriced.Name = "lblmktpriced";
            this.lblmktpriced.Size = new System.Drawing.Size(101, 16);
            this.lblmktpriced.TabIndex = 2;
            this.lblmktpriced.Text = "Markets Priced:";
            // 
            // txtmatchdate
            // 
            this.txtmatchdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtmatchdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmatchdate.Location = new System.Drawing.Point(18, 37);
            this.txtmatchdate.Name = "txtmatchdate";
            this.txtmatchdate.Size = new System.Drawing.Size(324, 26);
            this.txtmatchdate.TabIndex = 1;
            // 
            // lblmatchnotset
            // 
            this.lblmatchnotset.AutoSize = true;
            this.lblmatchnotset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmatchnotset.Location = new System.Drawing.Point(14, 14);
            this.lblmatchnotset.Name = "lblmatchnotset";
            this.lblmatchnotset.Size = new System.Drawing.Size(58, 20);
            this.lblmatchnotset.TabIndex = 0;
            this.lblmatchnotset.Text = "Match";
            // 
            // tableLayoutNews
            // 
            this.tableLayoutNews.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutNews.ColumnCount = 1;
            this.tableLayoutNews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutNews.Controls.Add(this.panel6, 0, 1);
            this.tableLayoutNews.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutNews.Location = new System.Drawing.Point(16, 419);
            this.tableLayoutNews.Name = "tableLayoutNews";
            this.tableLayoutNews.RowCount = 2;
            this.tableLayoutNews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutNews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutNews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutNews.Size = new System.Drawing.Size(256, 192);
            this.tableLayoutNews.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Location = new System.Drawing.Point(4, 37);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(248, 151);
            this.panel6.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel5.Controls.Add(this.label3);
            this.panel5.Location = new System.Drawing.Point(4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(248, 26);
            this.panel5.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Maiandra GD", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(100, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "News";
            // 
            // tableLayoutAttention
            // 
            this.tableLayoutAttention.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutAttention.ColumnCount = 1;
            this.tableLayoutAttention.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAttention.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutAttention.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutAttention.Location = new System.Drawing.Point(15, 219);
            this.tableLayoutAttention.Name = "tableLayoutAttention";
            this.tableLayoutAttention.RowCount = 2;
            this.tableLayoutAttention.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAttention.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutAttention.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutAttention.Size = new System.Drawing.Size(260, 192);
            this.tableLayoutAttention.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Yellow;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(252, 26);
            this.panel3.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Maiandra GD", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(90, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Attention !";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.Location = new System.Drawing.Point(4, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(252, 151);
            this.panel4.TabIndex = 1;
            // 
            // tableLayoutWarning
            // 
            this.tableLayoutWarning.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutWarning.ColumnCount = 1;
            this.tableLayoutWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutWarning.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutWarning.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutWarning.Location = new System.Drawing.Point(13, 12);
            this.tableLayoutWarning.Name = "tableLayoutWarning";
            this.tableLayoutWarning.RowCount = 2;
            this.tableLayoutWarning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutWarning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutWarning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutWarning.Size = new System.Drawing.Size(260, 192);
            this.tableLayoutWarning.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 151);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.lblWarning);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 26);
            this.panel1.TabIndex = 0;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Maiandra GD", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(90, 4);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(78, 16);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = "Warnings !";
            // 
            // CouponMenuStrip
            // 
            this.CouponMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToToolStripMenuItem});
            this.CouponMenuStrip.Name = "CouponMenuStrip";
            this.CouponMenuStrip.Size = new System.Drawing.Size(152, 26);
            // 
            // addToToolStripMenuItem
            // 
            this.addToToolStripMenuItem.Name = "addToToolStripMenuItem";
            this.addToToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.addToToolStripMenuItem.Text = "Add to archive";
            this.addToToolStripMenuItem.Click += new System.EventHandler(this.addToToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.Location = new System.Drawing.Point(190, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Result";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Coupons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 612);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Coupons";
            this.Text = "Coupons";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Coupons_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tableLayoutNews.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutAttention.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutWarning.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.CouponMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutWarning;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.TableLayoutPanel tableLayoutAttention;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutNews;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private CodersLab.Windows.Controls.TreeView treeView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lblmktavail;
        private System.Windows.Forms.Label lblmktpriced;
        private System.Windows.Forms.TextBox txtmatchdate;
        private System.Windows.Forms.Label lblmatchnotset;
        private System.Windows.Forms.Button btnaddmarket;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnxmlall;
        private System.Windows.Forms.Button btnxmlupdated;
        private System.Windows.Forms.ContextMenuStrip CouponMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}