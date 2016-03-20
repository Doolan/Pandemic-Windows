namespace SQADemicApp
{
    partial class ShareCardForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.P1T = new System.Windows.Forms.Button();
            this.P2T = new System.Windows.Forms.Button();
            this.P3T = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StealCardButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // P1T
            // 
            this.P1T.Location = new System.Drawing.Point(189, 175);
            this.P1T.Name = "P1T";
            this.P1T.Size = new System.Drawing.Size(75, 23);
            this.P1T.TabIndex = 1;
            this.P1T.Text = "button1";
            this.P1T.UseVisualStyleBackColor = true;
            this.P1T.Click += new System.EventHandler(this.P1T_Click);
            // 
            // P2T
            // 
            this.P2T.Location = new System.Drawing.Point(189, 205);
            this.P2T.Name = "P2T";
            this.P2T.Size = new System.Drawing.Size(75, 23);
            this.P2T.TabIndex = 2;
            this.P2T.Text = "button2";
            this.P2T.UseVisualStyleBackColor = true;
            this.P2T.Click += new System.EventHandler(this.P2T_Click);
            // 
            // P3T
            // 
            this.P3T.Location = new System.Drawing.Point(189, 235);
            this.P3T.Name = "P3T";
            this.P3T.Size = new System.Drawing.Size(75, 23);
            this.P3T.TabIndex = 3;
            this.P3T.Text = "button3";
            this.P3T.UseVisualStyleBackColor = true;
            this.P3T.Click += new System.EventHandler(this.P3T_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.P1T);
            this.groupBox1.Controls.Add(this.P3T);
            this.groupBox1.Controls.Add(this.P2T);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 267);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Give A Card";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.StealCardButton);
            this.groupBox2.Location = new System.Drawing.Point(294, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 267);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Take A Card";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(6, 21);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(258, 148);
            this.listBox2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "From: ";
            // 
            // StealCardButton
            // 
            this.StealCardButton.Location = new System.Drawing.Point(189, 205);
            this.StealCardButton.Name = "StealCardButton";
            this.StealCardButton.Size = new System.Drawing.Size(75, 23);
            this.StealCardButton.TabIndex = 2;
            this.StealCardButton.Text = "Player x";
            this.StealCardButton.UseVisualStyleBackColor = true;
            this.StealCardButton.Click += new System.EventHandler(this.StealCardButton_Click);
            // 
            // ShareCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 287);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ShareCardForm";
            this.Text = "ShareCardForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button P1T;
        private System.Windows.Forms.Button P2T;
        private System.Windows.Forms.Button P3T;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button StealCardButton;
    }
}