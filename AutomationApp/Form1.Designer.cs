
namespace AutomationApp
{
    partial class Form1
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
            this.emailtxtbox = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.passtxtbox = new System.Windows.Forms.TextBox();
            this.lgnbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // emailtxtbox
            // 
            this.emailtxtbox.Location = new System.Drawing.Point(156, 6);
            this.emailtxtbox.Margin = new System.Windows.Forms.Padding(7);
            this.emailtxtbox.Name = "emailtxtbox";
            this.emailtxtbox.Size = new System.Drawing.Size(228, 35);
            this.emailtxtbox.TabIndex = 0;
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(62, 12);
            this.email.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(80, 29);
            this.email.TabIndex = 1;
            this.email.Text = "Email:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // passtxtbox
            // 
            this.passtxtbox.Location = new System.Drawing.Point(156, 55);
            this.passtxtbox.Margin = new System.Windows.Forms.Padding(7);
            this.passtxtbox.Name = "passtxtbox";
            this.passtxtbox.Size = new System.Drawing.Size(228, 35);
            this.passtxtbox.TabIndex = 3;
            // 
            // lgnbtn
            // 
            this.lgnbtn.Location = new System.Drawing.Point(156, 104);
            this.lgnbtn.Margin = new System.Windows.Forms.Padding(7);
            this.lgnbtn.Name = "lgnbtn";
            this.lgnbtn.Size = new System.Drawing.Size(228, 37);
            this.lgnbtn.TabIndex = 4;
            this.lgnbtn.Text = "Login";
            this.lgnbtn.UseVisualStyleBackColor = true;
            this.lgnbtn.Click += new System.EventHandler(this.lgnbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 148);
            this.Controls.Add(this.lgnbtn);
            this.Controls.Add(this.passtxtbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.email);
            this.Controls.Add(this.emailtxtbox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox emailtxtbox;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passtxtbox;
        private System.Windows.Forms.Button lgnbtn;
    }
}

