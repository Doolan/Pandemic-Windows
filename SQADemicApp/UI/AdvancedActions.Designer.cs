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
            this.BuildResearchStation = new System.Windows.Forms.Button();
            this.CreateCure = new System.Windows.Forms.Button();
            this.ShareKnowledge = new System.Windows.Forms.Button();
            this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BuildResearchStation
            // 
            this.BuildResearchStation.Location = new System.Drawing.Point(82, 2);
            this.BuildResearchStation.Margin = new System.Windows.Forms.Padding(2);
            this.BuildResearchStation.Name = "BuildResearchStation";
            this.BuildResearchStation.Size = new System.Drawing.Size(76, 51);
            this.BuildResearchStation.TabIndex = 6;
            this.BuildResearchStation.Text = "Build Research Station";
            this.BuildResearchStation.UseVisualStyleBackColor = true;
            this.BuildResearchStation.Click += new System.EventHandler(this.BuildResearchStation_Click);
            // 
            // CreateCure
            // 
            this.CreateCure.Location = new System.Drawing.Point(162, 2);
            this.CreateCure.Margin = new System.Windows.Forms.Padding(2);
            this.CreateCure.Name = "CreateCure";
            this.CreateCure.Size = new System.Drawing.Size(76, 51);
            this.CreateCure.TabIndex = 5;
            this.CreateCure.Text = "Create Cure";
            this.CreateCure.UseVisualStyleBackColor = true;
            this.CreateCure.Click += new System.EventHandler(this.CreateCure_Click);
            // 
            // ShareKnowledge
            // 
            this.ShareKnowledge.Location = new System.Drawing.Point(2, 2);
            this.ShareKnowledge.Margin = new System.Windows.Forms.Padding(2);
            this.ShareKnowledge.Name = "ShareKnowledge";
            this.ShareKnowledge.Size = new System.Drawing.Size(76, 51);
            this.ShareKnowledge.TabIndex = 4;
            this.ShareKnowledge.Text = "Share Knowledge";
            this.ShareKnowledge.UseVisualStyleBackColor = true;
            this.ShareKnowledge.Click += new System.EventHandler(this.ShareKnowledge_Click);
            // 
            // flowLayoutPanel1
            // 
            this.ButtonPanel.Controls.Add(this.ShareKnowledge);
            this.ButtonPanel.Controls.Add(this.BuildResearchStation);
            this.ButtonPanel.Controls.Add(this.CreateCure);
            this.ButtonPanel.Location = new System.Drawing.Point(9, 12);
            this.ButtonPanel.Name = "flowLayoutPanel1";
            this.ButtonPanel.Size = new System.Drawing.Size(260, 151);
            this.ButtonPanel.TabIndex = 7;
            // 
            // AdvancedActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 175);
            this.Controls.Add(this.ButtonPanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AdvancedActions";
            this.Text = "AdvancedActions";
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BuildResearchStation;
        private System.Windows.Forms.Button CreateCure;
        private System.Windows.Forms.Button ShareKnowledge;
        public System.Windows.Forms.FlowLayoutPanel ButtonPanel;
    }
}