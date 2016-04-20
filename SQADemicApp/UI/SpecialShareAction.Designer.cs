namespace SQADemicApp.UI
{
    partial class SpecialShareAction
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.P1T = new System.Windows.Forms.Button();
            this.P3T = new System.Windows.Forms.Button();
            this.P2T = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.P1T);
            this.groupBox1.Controls.Add(this.P3T);
            this.groupBox1.Controls.Add(this.P2T);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 231);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Give A Card";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(6, 21);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(258, 148);
            this.listBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Send to:";
            // 
            // P1T
            // 
            this.P1T.Location = new System.Drawing.Point(189, 175);
            this.P1T.Name = "P1T";
            this.P1T.Size = new System.Drawing.Size(75, 23);
            this.P1T.TabIndex = 1;
            this.P1T.Text = "button1";
            this.P1T.UseVisualStyleBackColor = true;
            // 
            // P3T
            // 
            this.P3T.Location = new System.Drawing.Point(189, 235);
            this.P3T.Name = "P3T";
            this.P3T.Size = new System.Drawing.Size(75, 23);
            this.P3T.TabIndex = 3;
            this.P3T.Text = "button3";
            this.P3T.UseVisualStyleBackColor = true;
            // 
            // P2T
            // 
            this.P2T.Location = new System.Drawing.Point(189, 205);
            this.P2T.Name = "P2T";
            this.P2T.Size = new System.Drawing.Size(75, 23);
            this.P2T.TabIndex = 2;
            this.P2T.Text = "button2";
            this.P2T.UseVisualStyleBackColor = true;
            // 
            // SpecialShareAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.groupBox1);
            this.Name = "SpecialShareAction";
            this.Text = "SpecialShareAction";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button P1T;
        private System.Windows.Forms.Button P3T;
        private System.Windows.Forms.Button P2T;
    }
}