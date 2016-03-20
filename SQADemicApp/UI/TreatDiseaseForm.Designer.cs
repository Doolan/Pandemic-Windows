namespace SQADemicApp
{
    partial class TreatDiseaseForm
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
            this.BlueButton = new System.Windows.Forms.Button();
            this.BlackButton = new System.Windows.Forms.Button();
            this.RedButton = new System.Windows.Forms.Button();
            this.YellowButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BlueButton
            // 
            this.BlueButton.Location = new System.Drawing.Point(37, 57);
            this.BlueButton.Name = "BlueButton";
            this.BlueButton.Size = new System.Drawing.Size(110, 51);
            this.BlueButton.TabIndex = 0;
            this.BlueButton.Text = "Blue";
            this.BlueButton.UseVisualStyleBackColor = true;
            this.BlueButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // BlackButton
            // 
            this.BlackButton.Location = new System.Drawing.Point(37, 116);
            this.BlackButton.Name = "BlackButton";
            this.BlackButton.Size = new System.Drawing.Size(110, 51);
            this.BlackButton.TabIndex = 1;
            this.BlackButton.Text = "Black";
            this.BlackButton.UseVisualStyleBackColor = true;
            this.BlackButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // RedButton
            // 
            this.RedButton.Location = new System.Drawing.Point(37, 173);
            this.RedButton.Name = "RedButton";
            this.RedButton.Size = new System.Drawing.Size(110, 51);
            this.RedButton.TabIndex = 2;
            this.RedButton.Text = "Red";
            this.RedButton.UseVisualStyleBackColor = true;
            this.RedButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // YellowButton
            // 
            this.YellowButton.Location = new System.Drawing.Point(37, 230);
            this.YellowButton.Name = "YellowButton";
            this.YellowButton.Size = new System.Drawing.Size(110, 51);
            this.YellowButton.TabIndex = 3;
            this.YellowButton.Text = "Yellow";
            this.YellowButton.UseVisualStyleBackColor = true;
            this.YellowButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Disease to Cure";
            // 
            // TreatDiseaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 287);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.YellowButton);
            this.Controls.Add(this.RedButton);
            this.Controls.Add(this.BlackButton);
            this.Controls.Add(this.BlueButton);
            this.Name = "TreatDiseaseForm";
            this.Text = "TreatDiseaseForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BlueButton;
        private System.Windows.Forms.Button BlackButton;
        private System.Windows.Forms.Button RedButton;
        private System.Windows.Forms.Button YellowButton;
        private System.Windows.Forms.Label label1;
    }
}