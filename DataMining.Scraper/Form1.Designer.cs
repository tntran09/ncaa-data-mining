namespace DataMining.Scraper
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblEspn = new System.Windows.Forms.Label();
            this.txtEspnId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTeam = new System.Windows.Forms.TextBox();
            this.lblSeed = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(206, 378);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(6);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(150, 45);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblEspn
            // 
            this.lblEspn.AutoSize = true;
            this.lblEspn.Location = new System.Drawing.Point(24, 248);
            this.lblEspn.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEspn.Name = "lblEspn";
            this.lblEspn.Size = new System.Drawing.Size(126, 32);
            this.lblEspn.TabIndex = 1;
            this.lblEspn.Text = "ESPN ID";
            // 
            // txtEspnId
            // 
            this.txtEspnId.Location = new System.Drawing.Point(206, 248);
            this.txtEspnId.Margin = new System.Windows.Forms.Padding(6);
            this.txtEspnId.Name = "txtEspnId";
            this.txtEspnId.Size = new System.Drawing.Size(336, 38);
            this.txtEspnId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 194);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Team Name";
            // 
            // txtTeam
            // 
            this.txtTeam.Location = new System.Drawing.Point(206, 184);
            this.txtTeam.Margin = new System.Windows.Forms.Padding(6);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(336, 38);
            this.txtTeam.TabIndex = 0;
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(24, 311);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(82, 32);
            this.lblSeed.TabIndex = 4;
            this.lblSeed.Text = "Seed";
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(206, 311);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(336, 38);
            this.txtSeed.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 517);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.txtTeam);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEspnId);
            this.Controls.Add(this.lblEspn);
            this.Controls.Add(this.btnSubmit);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblEspn;
        private System.Windows.Forms.TextBox txtEspnId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTeam;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox txtSeed;
    }
}

