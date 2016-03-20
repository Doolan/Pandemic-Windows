namespace SQADemicApp
{
    partial class AdvancedActions
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
            this.ShareKnowledge = new System.Windows.Forms.Button();
            this.CreateCure = new System.Windows.Forms.Button();
            this.BuildResearchStation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ShareKnowledge
            // 
            this.ShareKnowledge.Location = new System.Drawing.Point(12, 12);
            this.ShareKnowledge.Name = "ShareKnowledge";
            this.ShareKnowledge.Size = new System.Drawing.Size(101, 63);
            this.ShareKnowledge.TabIndex = 4;
            this.ShareKnowledge.Text = "Share Knowledge";
            this.ShareKnowledge.UseVisualStyleBackColor = true;
            this.ShareKnowledge.Click += new System.EventHandler(this.ShareKnowledge_Click);
            // 
            // CreateCure
            // 
            this.CreateCure.Location = new System.Drawing.Point(262, 12);
            this.CreateCure.Name = "CreateCure";
            this.CreateCure.Size = new System.Drawing.Size(101, 63);
            this.CreateCure.TabIndex = 5;
            this.CreateCure.Text = "Create Cure";
            this.CreateCure.UseVisualStyleBackColor = true;
            this.CreateCure.Click += new System.EventHandler(this.CreateCure_Click);
            // 
            // BuildResearchStation
            // 
            this.BuildResearchStation.Location = new System.Drawing.Point(136, 12);
            this.BuildResearchStation.Name = "BuildResearchStation";
            this.BuildResearchStation.Size = new System.Drawing.Size(101, 63);
            this.BuildResearchStation.TabIndex = 6;
            this.BuildResearchStation.Text = "Build Research Station";
            this.BuildResearchStation.UseVisualStyleBackColor = true;
            this.BuildResearchStation.Click += new System.EventHandler(this.BuildResearchStation_Click);
            // 
            // AdvancedActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 99);
            this.Controls.Add(this.BuildResearchStation);
            this.Controls.Add(this.CreateCure);
            this.Controls.Add(this.ShareKnowledge);
            this.Name = "AdvancedActions";
            this.Text = "AdvancedActions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ShareKnowledge;
        private System.Windows.Forms.Button CreateCure;
        private System.Windows.Forms.Button BuildResearchStation;
    }
}