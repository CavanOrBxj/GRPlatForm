namespace GRPlatForm
{
    partial class ServerSetForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtYXPlat = new System.Windows.Forms.TextBox();
            this.txtZJPlat = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "永新平台：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "总局平台：";
            // 
            // txtYXPlat
            // 
            this.txtYXPlat.Location = new System.Drawing.Point(146, 99);
            this.txtYXPlat.Name = "txtYXPlat";
            this.txtYXPlat.Size = new System.Drawing.Size(303, 21);
            this.txtYXPlat.TabIndex = 2;
            // 
            // txtZJPlat
            // 
            this.txtZJPlat.Location = new System.Drawing.Point(146, 174);
            this.txtZJPlat.Name = "txtZJPlat";
            this.txtZJPlat.Size = new System.Drawing.Size(303, 21);
            this.txtZJPlat.TabIndex = 3;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(462, 319);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(94, 33);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "设置";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // ServerSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 400);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtZJPlat);
            this.Controls.Add(this.txtYXPlat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ServerSetForm";
            this.Text = "服务反馈地址地址设置";
            this.Load += new System.EventHandler(this.ServerSetForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtYXPlat;
        private System.Windows.Forms.TextBox txtZJPlat;
        private System.Windows.Forms.Button btnSet;
    }
}