namespace TTBoardReader
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.btnImagePathBrowse = new System.Windows.Forms.Button();
            this.btnMetaBrowse = new System.Windows.Forms.Button();
            this.txtMetaImageFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "TT Board Image Path:";
            // 
            // txtImagePath
            // 
            this.txtImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImagePath.Location = new System.Drawing.Point(182, 18);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(493, 26);
            this.txtImagePath.TabIndex = 1;
            // 
            // btnImagePathBrowse
            // 
            this.btnImagePathBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImagePathBrowse.Location = new System.Drawing.Point(704, 20);
            this.btnImagePathBrowse.Name = "btnImagePathBrowse";
            this.btnImagePathBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnImagePathBrowse.TabIndex = 2;
            this.btnImagePathBrowse.Text = "&Browse...";
            this.btnImagePathBrowse.UseVisualStyleBackColor = true;
            this.btnImagePathBrowse.Click += new System.EventHandler(this.btnImagePathBrowse_Click);
            // 
            // btnMetaBrowse
            // 
            this.btnMetaBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMetaBrowse.Location = new System.Drawing.Point(704, 54);
            this.btnMetaBrowse.Name = "btnMetaBrowse";
            this.btnMetaBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnMetaBrowse.TabIndex = 5;
            this.btnMetaBrowse.Text = "B&rowse...";
            this.btnMetaBrowse.UseVisualStyleBackColor = true;
            this.btnMetaBrowse.Click += new System.EventHandler(this.btnMetaBrowse_Click);
            // 
            // txtMetaImageFolder
            // 
            this.txtMetaImageFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMetaImageFolder.Location = new System.Drawing.Point(182, 52);
            this.txtMetaImageFolder.Name = "txtMetaImageFolder";
            this.txtMetaImageFolder.Size = new System.Drawing.Size(493, 26);
            this.txtMetaImageFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Save Meta Images To:";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 84);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(108, 23);
            this.btnProcess.TabIndex = 6;
            this.btnProcess.Text = "&Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtResults
            // 
            this.txtResults.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResults.Location = new System.Drawing.Point(16, 113);
            this.txtResults.MaxLength = 262144;
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(763, 325);
            this.txtResults.TabIndex = 7;
            this.txtResults.WordWrap = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnMetaBrowse);
            this.Controls.Add(this.txtMetaImageFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnImagePathBrowse);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "TT Board Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button btnImagePathBrowse;
        private System.Windows.Forms.Button btnMetaBrowse;
        private System.Windows.Forms.TextBox txtMetaImageFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtResults;
    }
}

