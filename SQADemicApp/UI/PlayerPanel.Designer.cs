using System.Drawing;
namespace SQADemicApp
{
    partial class PlayerPanel
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.MoveButton = new System.Windows.Forms.Button();
            this.CureCityButton = new System.Windows.Forms.Button();
            this.RedCubes = new System.Windows.Forms.Label();
            this.BlueCubes = new System.Windows.Forms.Label();
            this.BlackCubes = new System.Windows.Forms.Label();
            this.YellowCubes = new System.Windows.Forms.Label();
            this.InfectionRate = new System.Windows.Forms.Label();
            this.OutbreakCount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AAButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.YellowCure = new System.Windows.Forms.Label();
            this.BlueCure = new System.Windows.Forms.Label();
            this.BlackCure = new System.Windows.Forms.Label();
            this.RedCure = new System.Windows.Forms.Label();
            this.DispatcherMove = new System.Windows.Forms.Button();
            this.EndSequenceBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(28, 541);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(232, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 567);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Move Count: 0/4";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(25, 401);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(235, 108);
            this.listBox1.TabIndex = 2;
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(25, 12);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(101, 63);
            this.MoveButton.TabIndex = 3;
            this.MoveButton.Text = "Move";
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // CureCityButton
            // 
            this.CureCityButton.Location = new System.Drawing.Point(159, 12);
            this.CureCityButton.Name = "CureCityButton";
            this.CureCityButton.Size = new System.Drawing.Size(101, 63);
            this.CureCityButton.TabIndex = 6;
            this.CureCityButton.Text = "Cure City";
            this.CureCityButton.UseVisualStyleBackColor = true;
            this.CureCityButton.Click += new System.EventHandler(this.CureCityButton_Click);
            // 
            // RedCubes
            // 
            this.RedCubes.AutoSize = true;
            this.RedCubes.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.RedCubes.Location = new System.Drawing.Point(10, 23);
            this.RedCubes.Name = "RedCubes";
            this.RedCubes.Size = new System.Drawing.Size(210, 14);
            this.RedCubes.TabIndex = 7;
            this.RedCubes.Text = "Red Cubes Remaining:    24/24";
            // 
            // BlueCubes
            // 
            this.BlueCubes.AutoSize = true;
            this.BlueCubes.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.BlueCubes.Location = new System.Drawing.Point(10, 48);
            this.BlueCubes.Name = "BlueCubes";
            this.BlueCubes.Size = new System.Drawing.Size(210, 14);
            this.BlueCubes.TabIndex = 8;
            this.BlueCubes.Text = "Blue Cubes Remaining:   24/24";
            // 
            // BlackCubes
            // 
            this.BlackCubes.AutoSize = true;
            this.BlackCubes.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.BlackCubes.Location = new System.Drawing.Point(10, 73);
            this.BlackCubes.Name = "BlackCubes";
            this.BlackCubes.Size = new System.Drawing.Size(210, 14);
            this.BlackCubes.TabIndex = 9;
            this.BlackCubes.Text = "Black Cubes Remaining:  24/24";
            this.BlackCubes.UseMnemonic = false;
            // 
            // YellowCubes
            // 
            this.YellowCubes.AutoSize = true;
            this.YellowCubes.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.YellowCubes.Location = new System.Drawing.Point(10, 98);
            this.YellowCubes.Name = "YellowCubes";
            this.YellowCubes.Size = new System.Drawing.Size(210, 14);
            this.YellowCubes.TabIndex = 10;
            this.YellowCubes.Text = "Yellow Cubes Remaining: 24/24";
            // 
            // InfectionRate
            // 
            this.InfectionRate.AutoSize = true;
            this.InfectionRate.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.InfectionRate.Location = new System.Drawing.Point(68, 160);
            this.InfectionRate.Name = "InfectionRate";
            this.InfectionRate.Size = new System.Drawing.Size(126, 14);
            this.InfectionRate.TabIndex = 11;
            this.InfectionRate.Text = "Infection Rate: 2";
            // 
            // OutbreakCount
            // 
            this.OutbreakCount.AutoSize = true;
            this.OutbreakCount.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.OutbreakCount.Location = new System.Drawing.Point(68, 177);
            this.OutbreakCount.Name = "OutbreakCount";
            this.OutbreakCount.Size = new System.Drawing.Size(126, 14);
            this.OutbreakCount.TabIndex = 12;
            this.OutbreakCount.Text = "Outbreak Level: 0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BlackCubes);
            this.groupBox1.Controls.Add(this.RedCubes);
            this.groupBox1.Controls.Add(this.BlueCubes);
            this.groupBox1.Controls.Add(this.YellowCubes);
            this.groupBox1.Location = new System.Drawing.Point(12, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 128);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Remaining Cubes";
            // 
            // AAButton
            // 
            this.AAButton.Location = new System.Drawing.Point(159, 82);
            this.AAButton.Name = "AAButton";
            this.AAButton.Size = new System.Drawing.Size(101, 63);
            this.AAButton.TabIndex = 14;
            this.AAButton.Text = "Advanced Actions";
            this.AAButton.UseVisualStyleBackColor = true;
            this.AAButton.Click += new System.EventHandler(this.AAButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.YellowCure);
            this.groupBox2.Controls.Add(this.BlueCure);
            this.groupBox2.Controls.Add(this.BlackCure);
            this.groupBox2.Controls.Add(this.RedCure);
            this.groupBox2.Location = new System.Drawing.Point(13, 338);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 57);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cure Status";
            // 
            // YellowCure
            // 
            this.YellowCure.AutoSize = true;
            this.YellowCure.Location = new System.Drawing.Point(144, 36);
            this.YellowCure.Name = "YellowCure";
            this.YellowCure.Size = new System.Drawing.Size(83, 13);
            this.YellowCure.TabIndex = 3;
            this.YellowCure.Text = "Yellow: No Cure";
            // 
            // BlueCure
            // 
            this.BlueCure.AutoSize = true;
            this.BlueCure.Location = new System.Drawing.Point(144, 18);
            this.BlueCure.Name = "BlueCure";
            this.BlueCure.Size = new System.Drawing.Size(73, 13);
            this.BlueCure.TabIndex = 2;
            this.BlueCure.Text = "Blue: No Cure";
            // 
            // BlackCure
            // 
            this.BlackCure.AutoSize = true;
            this.BlackCure.Location = new System.Drawing.Point(12, 36);
            this.BlackCure.Name = "BlackCure";
            this.BlackCure.Size = new System.Drawing.Size(79, 13);
            this.BlackCure.TabIndex = 1;
            this.BlackCure.Text = "Black: No Cure";
            // 
            // RedCure
            // 
            this.RedCure.AutoSize = true;
            this.RedCure.Location = new System.Drawing.Point(12, 18);
            this.RedCure.Name = "RedCure";
            this.RedCure.Size = new System.Drawing.Size(72, 13);
            this.RedCure.TabIndex = 0;
            this.RedCure.Text = "Red: No Cure";
            // 
            // DispatcherMove
            // 
            this.DispatcherMove.Location = new System.Drawing.Point(25, 82);
            this.DispatcherMove.Name = "DispatcherMove";
            this.DispatcherMove.Size = new System.Drawing.Size(102, 62);
            this.DispatcherMove.TabIndex = 16;
            this.DispatcherMove.Text = "Dispatch Other Player";
            this.DispatcherMove.UseVisualStyleBackColor = true;
            this.DispatcherMove.Click += new System.EventHandler(this.DispatcherMove_Click);
            // 
            // EndSequenceBtn
            // 
            this.EndSequenceBtn.Location = new System.Drawing.Point(25, 12);
            this.EndSequenceBtn.Name = "EndSequenceBtn";
            this.EndSequenceBtn.Size = new System.Drawing.Size(235, 132);
            this.EndSequenceBtn.TabIndex = 17;
            this.EndSequenceBtn.Text = "Draw Cards";
            this.EndSequenceBtn.UseVisualStyleBackColor = true;
            this.EndSequenceBtn.Click += new System.EventHandler(this.EndSequenceBtn_Click);
            // 
            // PlayerPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(286, 612);
            this.Controls.Add(this.EndSequenceBtn);
            this.Controls.Add(this.DispatcherMove);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.AAButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OutbreakCount);
            this.Controls.Add(this.InfectionRate);
            this.Controls.Add(this.CureCityButton);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "PlayerPanel";
            this.Text = "Player Turn";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Button CureCityButton;
        public System.Windows.Forms.Label RedCubes;
        public System.Windows.Forms.Label BlueCubes;
        public System.Windows.Forms.Label BlackCubes;
        public System.Windows.Forms.Label YellowCubes;
        public System.Windows.Forms.Label InfectionRate;
        public System.Windows.Forms.Label OutbreakCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Label BlackCure;
        public System.Windows.Forms.Label YellowCure;
        public System.Windows.Forms.Label BlueCure;
        public System.Windows.Forms.Label RedCure;
        public System.Windows.Forms.Button DispatcherMove;
        public System.Windows.Forms.Button AAButton;
        public System.Windows.Forms.Button EndSequenceBtn;
    }
}