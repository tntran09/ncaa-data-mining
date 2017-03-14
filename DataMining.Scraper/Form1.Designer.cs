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
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSeed = new System.Windows.Forms.Label();
            this.seedTextBox = new System.Windows.Forms.TextBox();
            this.validationTextBox = new System.Windows.Forms.TextBox();
            this.teamDropdown = new System.Windows.Forms.ComboBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(180, 354);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(131, 42);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblEspn
            // 
            this.lblEspn.AutoSize = true;
            this.lblEspn.Location = new System.Drawing.Point(21, 232);
            this.lblEspn.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblEspn.Name = "lblEspn";
            this.lblEspn.Size = new System.Drawing.Size(108, 29);
            this.lblEspn.TabIndex = 1;
            this.lblEspn.Text = "ESPN ID";
            // 
            // idTextBox
            // 
            this.idTextBox.Enabled = false;
            this.idTextBox.Location = new System.Drawing.Point(180, 232);
            this.idTextBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(294, 35);
            this.idTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 181);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Team Name";
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(21, 291);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(71, 29);
            this.lblSeed.TabIndex = 4;
            this.lblSeed.Text = "Seed";
            // 
            // seedTextBox
            // 
            this.seedTextBox.Location = new System.Drawing.Point(180, 291);
            this.seedTextBox.Name = "seedTextBox";
            this.seedTextBox.Size = new System.Drawing.Size(294, 35);
            this.seedTextBox.TabIndex = 2;
            // 
            // validationTextBox
            // 
            this.validationTextBox.Enabled = false;
            this.validationTextBox.Location = new System.Drawing.Point(482, 12);
            this.validationTextBox.Multiline = true;
            this.validationTextBox.Name = "validationTextBox";
            this.validationTextBox.Size = new System.Drawing.Size(478, 350);
            this.validationTextBox.TabIndex = 0;
            this.validationTextBox.TabStop = false;
            // 
            // teamDropdown
            // 
            this.teamDropdown.DisplayMember = "Name";
            this.teamDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.teamDropdown.FormattingEnabled = true;
            this.teamDropdown.Location = new System.Drawing.Point(182, 181);
            this.teamDropdown.MaxDropDownItems = 12;
            this.teamDropdown.Name = "teamDropdown";
            this.teamDropdown.Size = new System.Drawing.Size(294, 37);
            this.teamDropdown.TabIndex = 7;
            this.teamDropdown.ValueMember = "Id";
            this.teamDropdown.SelectedIndexChanged += new System.EventHandler(this.teamDropdown_SelectedIndexChanged);
            // 
            // outputButton
            // 
            this.outputButton.Enabled = false;
            this.outputButton.Location = new System.Drawing.Point(761, 369);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(198, 42);
            this.outputButton.TabIndex = 8;
            this.outputButton.Text = "Save";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 484);
            this.Controls.Add(this.outputButton);
            this.Controls.Add(this.teamDropdown);
            this.Controls.Add(this.validationTextBox);
            this.Controls.Add(this.seedTextBox);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.lblEspn);
            this.Controls.Add(this.btnSubmit);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblEspn;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox seedTextBox;
        private System.Windows.Forms.TextBox validationTextBox;
        private System.Windows.Forms.ComboBox teamDropdown;
        private System.Windows.Forms.Button outputButton;
    }
}

