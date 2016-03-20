namespace SQADemicApp
{
    partial class CharacterPane
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
            this.Player4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Player1
            // 
            this.Player1.BackColor = System.Drawing.Color.Aqua;
            this.Player1.Location = new System.Drawing.Point(12, 12);
            this.Player1.Name = "Player1";
            this.Player1.Size = new System.Drawing.Size(180, 93);
            this.Player1.TabIndex = 0;
            this.Player1.Text = "Player 1\nAtlanta";
            this.Player1.UseVisualStyleBackColor = false;
            this.Player1.Click += new System.EventHandler(this.Player_Click);
            // 
            // Player2
            // 
            this.Player2.BackColor = System.Drawing.Color.Aqua;
            this.Player2.Location = new System.Drawing.Point(209, 12);
            this.Player2.Name = "Player2";
            this.Player2.Size = new System.Drawing.Size(180, 93);
            this.Player2.TabIndex = 1;
            this.Player2.Text = "Player 2\nAtlanta";
            this.Player2.UseVisualStyleBackColor = true;
            this.Player2.Click += new System.EventHandler(this.Player_Click);
            // 
            // Player3
            // 
            this.Player3.BackColor = System.Drawing.Color.Aqua;
            this.Player3.Location = new System.Drawing.Point(12, 127);
            this.Player3.Name = "Player3";
            this.Player3.Size = new System.Drawing.Size(180, 93);
            this.Player3.TabIndex = 2;
            this.Player3.Text = "Player 3\nAtlanta";
            this.Player3.UseVisualStyleBackColor = true;
            this.Player3.Click += new System.EventHandler(this.Player_Click);
            // 
            // Player4
            // 
            this.Player4.BackColor = System.Drawing.Color.Aqua;
            this.Player4.Location = new System.Drawing.Point(209, 127);
            this.Player4.Name = "Player4";
            this.Player4.Size = new System.Drawing.Size(180, 93);
            this.Player4.TabIndex = 3;
            this.Player4.Text = "Player 4\nAtlanta";
            this.Player4.UseVisualStyleBackColor = true;
            this.Player4.Click += new System.EventHandler(this.Player_Click);
            // 
            // CharacterPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 231);
            this.Controls.Add(this.Player4);
            this.Controls.Add(this.Player3);
            this.Controls.Add(this.Player2);
            this.Controls.Add(this.Player1);
            this.Name = "CharacterPane";
            this.Text = "Player Pane";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button Player1;
        public System.Windows.Forms.Button Player2;
        public System.Windows.Forms.Button Player3;
        public System.Windows.Forms.Button Player4;
    }
}