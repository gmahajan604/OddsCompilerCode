namespace OddsCheckerCrawler
{
    partial class frmResultView
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
            this.lblScortName = new System.Windows.Forms.Label();
            this.lblMatchNmae = new System.Windows.Forms.Label();
            this.lblMatchID = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblsportid = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblScortName
            // 
            this.lblScortName.AutoSize = true;
            this.lblScortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScortName.Location = new System.Drawing.Point(12, 18);
            this.lblScortName.Name = "lblScortName";
            this.lblScortName.Size = new System.Drawing.Size(57, 20);
            this.lblScortName.TabIndex = 0;
            this.lblScortName.Text = "label1";
            // 
            // lblMatchNmae
            // 
            this.lblMatchNmae.AutoSize = true;
            this.lblMatchNmae.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchNmae.Location = new System.Drawing.Point(11, 38);
            this.lblMatchNmae.Name = "lblMatchNmae";
            this.lblMatchNmae.Size = new System.Drawing.Size(70, 25);
            this.lblMatchNmae.TabIndex = 1;
            this.lblMatchNmae.Text = "label2";
            // 
            // lblMatchID
            // 
            this.lblMatchID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMatchID.AutoSize = true;
            this.lblMatchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchID.Location = new System.Drawing.Point(344, 18);
            this.lblMatchID.Name = "lblMatchID";
            this.lblMatchID.Size = new System.Drawing.Size(57, 20);
            this.lblMatchID.TabIndex = 2;
            this.lblMatchID.Text = "label1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(413, 489);
            this.dataGridView1.TabIndex = 3;
            // 
            // lblsportid
            // 
            this.lblsportid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblsportid.AutoSize = true;
            this.lblsportid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsportid.Location = new System.Drawing.Point(344, 38);
            this.lblsportid.Name = "lblsportid";
            this.lblsportid.Size = new System.Drawing.Size(57, 20);
            this.lblsportid.TabIndex = 4;
            this.lblsportid.Text = "label1";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(12, 76);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(51, 20);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "label1";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(283, 76);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(51, 20);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "label1";
            // 
            // frmResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 588);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblsportid);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblMatchID);
            this.Controls.Add(this.lblMatchNmae);
            this.Controls.Add(this.lblScortName);
            this.Name = "frmResultView";
            this.Text = "Market Result";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScortName;
        private System.Windows.Forms.Label lblMatchNmae;
        private System.Windows.Forms.Label lblMatchID;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblsportid;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
    }
}