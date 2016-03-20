namespace SQADemicApp
{
    partial class DispatcherMove
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
            this.Player1 = new System.Windows.Forms.Button();
            this.Player2 = new System.Windows.Forms.Button();
            this.Player3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Player1
            // 
            this.Player1.Location = new System.Drawing.Point(13, 13);
            this.Player1.Name = "Player1";
            this.Player1.Size = new System.Drawing.Size(143, 125);
            this.Player1.TabIndex = 0;
            this.Player1.Text = "Player 1";
            this.Player1.UseVisualStyleBackColor = true;
            // 
            // Player2
            // 
            this.Player2.Location = new System.Drawing.Point(162, 13);
            this.Player2.Name = "Player2";
            this.Player2.Size = new System.Drawing.Size(143, 125);
            this.Player2.TabIndex = 1;
            this.Player2.Text = "Player 2";
            this.Player2.UseVisualStyleBackColor = true;
            // 
            // Player3
            // 
            this.Player3.Location = new System.Drawing.Point(311, 13);
            this.Player3.Name = "Player3";
            this.Player3.Size = new System.Drawing.Size(143, 125);
            this.Player3.TabIndex = 2;
            this.Player3.Text = "Player 3";
            this.Player3.UseVisualStyleBackColor = true;
            // 
            // DispatcherMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 150);
            this.Controls.Add(this.Player3);
            this.Controls.Add(this.Player2);
            this.Controls.Add(this.Player1);
            this.Name = "DispatcherMove";
            this.Text = "DispatcherMove";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Player1;
        private System.Windows.Forms.Button Player2;
        private System.Windows.Forms.Button Player3;
    }
}