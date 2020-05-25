namespace Do
{
    partial class TEXT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TEXT));
            this.btn_global = new System.Windows.Forms.Button();
            this.btn_tr = new System.Windows.Forms.Button();
            this.btn_en = new System.Windows.Forms.Button();
            this.btn_isp = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_global
            // 
            this.btn_global.Location = new System.Drawing.Point(26, 202);
            this.btn_global.Name = "btn_global";
            this.btn_global.Size = new System.Drawing.Size(351, 33);
            this.btn_global.TabIndex = 0;
            this.btn_global.Text = "Global\'e Gonder";
            this.btn_global.UseVisualStyleBackColor = true;
            this.btn_global.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_tr
            // 
            this.btn_tr.Location = new System.Drawing.Point(26, 241);
            this.btn_tr.Name = "btn_tr";
            this.btn_tr.Size = new System.Drawing.Size(113, 33);
            this.btn_tr.TabIndex = 1;
            this.btn_tr.Text = "TR\'ye Gonder";
            this.btn_tr.UseVisualStyleBackColor = true;
            this.btn_tr.Click += new System.EventHandler(this.btn_tr_Click);
            // 
            // btn_en
            // 
            this.btn_en.Location = new System.Drawing.Point(145, 241);
            this.btn_en.Name = "btn_en";
            this.btn_en.Size = new System.Drawing.Size(113, 33);
            this.btn_en.TabIndex = 2;
            this.btn_en.Text = "Ingılız\'e Gonder";
            this.btn_en.UseVisualStyleBackColor = true;
            this.btn_en.Click += new System.EventHandler(this.btn_en_Click);
            // 
            // btn_isp
            // 
            this.btn_isp.Location = new System.Drawing.Point(264, 241);
            this.btn_isp.Name = "btn_isp";
            this.btn_isp.Size = new System.Drawing.Size(113, 33);
            this.btn_isp.TabIndex = 3;
            this.btn_isp.Text = "Ispanyol\'a Gonder";
            this.btn_isp.UseVisualStyleBackColor = true;
            this.btn_isp.Click += new System.EventHandler(this.btn_isp_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(145, 280);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(113, 31);
            this.button5.TabIndex = 4;
            this.button5.Text = "BAN SISTEMI";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(26, 278);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(113, 33);
            this.button6.TabIndex = 5;
            this.button6.Text = "KICK SISTEMI";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(264, 280);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(113, 29);
            this.button7.TabIndex = 6;
            this.button7.Text = "CHAT LOG";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Buxton Sketch", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(131, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 79);
            this.label1.TabIndex = 7;
            this.label1.Text = "Z3K";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 105);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(324, 79);
            this.textBox1.TabIndex = 8;
            // 
            // TEXT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 324);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btn_isp);
            this.Controls.Add(this.btn_en);
            this.Controls.Add(this.btn_tr);
            this.Controls.Add(this.btn_global);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TEXT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Panel";
            this.Load += new System.EventHandler(this.TEXT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_global;
        private System.Windows.Forms.Button btn_tr;
        private System.Windows.Forms.Button btn_en;
        private System.Windows.Forms.Button btn_isp;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}