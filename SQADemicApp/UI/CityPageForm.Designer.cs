namespace SQADemicApp
{
    partial class CityPageForm
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
            this.CityName = new System.Windows.Forms.Label();
            this.CityColor = new System.Windows.Forms.Label();
            this.CubeCount = new System.Windows.Forms.GroupBox();
            this.YellowCubeCount = new System.Windows.Forms.Label();
            this.BlackCubeCount = new System.Windows.Forms.Label();
            this.BlueCubeCount = new System.Windows.Forms.Label();
            this.RedCubeCount = new System.Windows.Forms.Label();
            this.YellowCubesName = new System.Windows.Forms.Label();
            this.BlackCubesName = new System.Windows.Forms.Label();
            this.BlueCubesName = new System.Windows.Forms.Label();
            this.RedCubeName = new System.Windows.Forms.Label();
            this.HasResearchStation = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.CubeCount.SuspendLayout();
            this.SuspendLayout();
            // 
            // CityName
            // 
            this.CityName.AutoSize = true;
            this.CityName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CityName.Location = new System.Drawing.Point(19, 12);
            this.CityName.Name = "CityName";
            this.CityName.Size = new System.Drawing.Size(176, 39);
            this.CityName.TabIndex = 0;
            this.CityName.Text = "City Name";
            // 
            // CityColor
            // 
            this.CityColor.AutoSize = true;
            this.CityColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CityColor.Location = new System.Drawing.Point(38, 56);
            this.CityColor.Name = "CityColor";
            this.CityColor.Size = new System.Drawing.Size(49, 20);
            this.CityColor.TabIndex = 1;
            this.CityColor.Text = "Color";
            // 
            // CubeCount
            // 
            this.CubeCount.Controls.Add(this.YellowCubeCount);
            this.CubeCount.Controls.Add(this.BlackCubeCount);
            this.CubeCount.Controls.Add(this.BlueCubeCount);
            this.CubeCount.Controls.Add(this.RedCubeCount);
            this.CubeCount.Controls.Add(this.YellowCubesName);
            this.CubeCount.Controls.Add(this.BlackCubesName);
            this.CubeCount.Controls.Add(this.BlueCubesName);
            this.CubeCount.Controls.Add(this.RedCubeName);
            this.CubeCount.Location = new System.Drawing.Point(27, 79);
            this.CubeCount.Name = "CubeCount";
            this.CubeCount.Size = new System.Drawing.Size(186, 118);
            this.CubeCount.TabIndex = 2;
            this.CubeCount.TabStop = false;
            this.CubeCount.Text = "Cube Counts";
            // 
            // YellowCubeCount
            // 
            this.YellowCubeCount.AutoSize = true;
            this.YellowCubeCount.Location = new System.Drawing.Point(151, 86);
            this.YellowCubeCount.Name = "YellowCubeCount";
            this.YellowCubeCount.Size = new System.Drawing.Size(16, 17);
            this.YellowCubeCount.TabIndex = 7;
            this.YellowCubeCount.Text = "0";
            // 
            // BlackCubeCount
            // 
            this.BlackCubeCount.AutoSize = true;
            this.BlackCubeCount.Location = new System.Drawing.Point(151, 64);
            this.BlackCubeCount.Name = "BlackCubeCount";
            this.BlackCubeCount.Size = new System.Drawing.Size(16, 17);
            this.BlackCubeCount.TabIndex = 6;
            this.BlackCubeCount.Text = "0";
            // 
            // BlueCubeCount
            // 
            this.BlueCubeCount.AutoSize = true;
            this.BlueCubeCount.Location = new System.Drawing.Point(151, 44);
            this.BlueCubeCount.Name = "BlueCubeCount";
            this.BlueCubeCount.Size = new System.Drawing.Size(16, 17);
            this.BlueCubeCount.TabIndex = 5;
            this.BlueCubeCount.Text = "0";
            // 
            // RedCubeCount
            // 
            this.RedCubeCount.AutoSize = true;
            this.RedCubeCount.Location = new System.Drawing.Point(151, 23);
            this.RedCubeCount.Name = "RedCubeCount";
            this.RedCubeCount.Size = new System.Drawing.Size(16, 17);
            this.RedCubeCount.TabIndex = 4;
            this.RedCubeCount.Text = "0";
            // 
            // YellowCubesName
            // 
            this.YellowCubesName.AutoSize = true;
            this.YellowCubesName.Location = new System.Drawing.Point(12, 86);
            this.YellowCubesName.Name = "YellowCubesName";
            this.YellowCubesName.Size = new System.Drawing.Size(92, 17);
            this.YellowCubesName.TabIndex = 3;
            this.YellowCubesName.Text = "Yellow Cubes";
            // 
            // BlackCubesName
            // 
            this.BlackCubesName.AutoSize = true;
            this.BlackCubesName.Location = new System.Drawing.Point(12, 64);
            this.BlackCubesName.Name = "BlackCubesName";
            this.BlackCubesName.Size = new System.Drawing.Size(86, 17);
            this.BlackCubesName.TabIndex = 2;
            this.BlackCubesName.Text = "Black Cubes";
            // 
            // BlueCubesName
            // 
            this.BlueCubesName.AutoSize = true;
            this.BlueCubesName.Location = new System.Drawing.Point(12, 44);
            this.BlueCubesName.Name = "BlueCubesName";
            this.BlueCubesName.Size = new System.Drawing.Size(80, 17);
            this.BlueCubesName.TabIndex = 1;
            this.BlueCubesName.Text = "Blue Cubes";
            // 
            // RedCubeName
            // 
            this.RedCubeName.AutoSize = true;
            this.RedCubeName.Location = new System.Drawing.Point(12, 24);
            this.RedCubeName.Name = "RedCubeName";
            this.RedCubeName.Size = new System.Drawing.Size(78, 17);
            this.RedCubeName.TabIndex = 0;
            this.RedCubeName.Text = "Red Cubes";
            // 
            // HasResearchStation
            // 
            this.HasResearchStation.AutoSize = true;
            this.HasResearchStation.Enabled = false;
            this.HasResearchStation.Location = new System.Drawing.Point(277, 79);
            this.HasResearchStation.Name = "HasResearchStation";
            this.HasResearchStation.Size = new System.Drawing.Size(139, 21);
            this.HasResearchStation.TabIndex = 3;
            this.HasResearchStation.Text = "Research Station";
            this.HasResearchStation.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CityPageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 220);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.HasResearchStation);
            this.Controls.Add(this.CubeCount);
            this.Controls.Add(this.CityColor);
            this.Controls.Add(this.CityName);
            this.Name = "CityPageForm";
            this.Text = "CityPageForm";
            this.CubeCount.ResumeLayout(false);
            this.CubeCount.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label CityName;
        public System.Windows.Forms.Label CityColor;
        private System.Windows.Forms.GroupBox CubeCount;
        private System.Windows.Forms.Label YellowCubesName;
        private System.Windows.Forms.Label BlackCubesName;
        private System.Windows.Forms.Label BlueCubesName;
        private System.Windows.Forms.Label RedCubeName;
        public System.Windows.Forms.Label YellowCubeCount;
        public System.Windows.Forms.Label BlackCubeCount;
        public System.Windows.Forms.Label BlueCubeCount;
        public System.Windows.Forms.Label RedCubeCount;
        public System.Windows.Forms.CheckBox HasResearchStation;
        private System.Windows.Forms.Button button1;
    }
}