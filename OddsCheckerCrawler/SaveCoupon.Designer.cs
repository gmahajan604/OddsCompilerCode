namespace OddsCheckerCrawler
{
    partial class SaveCoupon
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
            this.txtcouponname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnname = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtcouponname
            // 
            this.txtcouponname.Location = new System.Drawing.Point(94, 32);
            this.txtcouponname.Name = "txtcouponname";
            this.txtcouponname.Size = new System.Drawing.Size(201, 20);
            this.txtcouponname.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Coupon Name";
            // 
            // btnname
            // 
            this.btnname.Location = new System.Drawing.Point(16, 79);
            this.btnname.Name = "btnname";
            this.btnname.Size = new System.Drawing.Size(75, 23);
            this.btnname.TabIndex = 2;
            this.btnname.Text = "Save";
            this.btnname.UseVisualStyleBackColor = true;
            this.btnname.Click += new System.EventHandler(this.btnname_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(108, 79);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 3;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // SaveCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 114);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtcouponname);
            this.Name = "SaveCoupon";
            this.Text = "SaveCoupon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtcouponname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnname;
        private System.Windows.Forms.Button btncancel;
    }
}