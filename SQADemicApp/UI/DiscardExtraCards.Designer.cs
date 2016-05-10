namespace SQADemicApp.UI
{
    partial class DiscardExtraCards
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
            this.CurrentHandBox = new System.Windows.Forms.ListBox();
            this.DiscardBox = new System.Windows.Forms.ListBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.currentCards = new System.Windows.Forms.Label();
            this.discarded = new System.Windows.Forms.Label();
            this.MaxHandSizeLabel = new System.Windows.Forms.Label();
            this.CurrentHandSizeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurrentHandBox
            // 
            this.CurrentHandBox.FormattingEnabled = true;
            this.CurrentHandBox.ItemHeight = 16;
            this.CurrentHandBox.Location = new System.Drawing.Point(12, 23);
            this.CurrentHandBox.Name = "CurrentHandBox";
            this.CurrentHandBox.Size = new System.Drawing.Size(142, 196);
            this.CurrentHandBox.TabIndex = 1;
            // 
            // DiscardBox
            // 
            this.DiscardBox.FormattingEnabled = true;
            this.DiscardBox.ItemHeight = 16;
            this.DiscardBox.Location = new System.Drawing.Point(309, 23);
            this.DiscardBox.Name = "DiscardBox";
            this.DiscardBox.Size = new System.Drawing.Size(142, 196);
            this.DiscardBox.TabIndex = 2;
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Enabled = false;
            this.ConfirmButton.Location = new System.Drawing.Point(12, 225);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(439, 42);
            this.ConfirmButton.TabIndex = 3;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(160, 125);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(143, 37);
            this.Add.TabIndex = 4;
            this.Add.Text = ">>";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(160, 182);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(143, 37);
            this.Remove.TabIndex = 5;
            this.Remove.Text = "<<";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // currentCards
            // 
            this.currentCards.AutoSize = true;
            this.currentCards.Location = new System.Drawing.Point(12, 3);
            this.currentCards.Name = "currentCards";
            this.currentCards.Size = new System.Drawing.Size(96, 17);
            this.currentCards.TabIndex = 6;
            this.currentCards.Text = "Current Cards";
            // 
            // discarded
            // 
            this.discarded.AutoSize = true;
            this.discarded.Location = new System.Drawing.Point(306, 3);
            this.discarded.Name = "discarded";
            this.discarded.Size = new System.Drawing.Size(77, 17);
            this.discarded.TabIndex = 7;
            this.discarded.Text = "To Discard";
            // 
            // MaxHandSizeLabel
            // 
            this.MaxHandSizeLabel.AutoSize = true;
            this.MaxHandSizeLabel.Location = new System.Drawing.Point(163, 42);
            this.MaxHandSizeLabel.Name = "MaxHandSizeLabel";
            this.MaxHandSizeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MaxHandSizeLabel.Size = new System.Drawing.Size(118, 17);
            this.MaxHandSizeLabel.TabIndex = 8;
            this.MaxHandSizeLabel.Text = "Max Hand Size: #";
            // 
            // CurrentHandSizeLabel
            // 
            this.CurrentHandSizeLabel.AutoSize = true;
            this.CurrentHandSizeLabel.Location = new System.Drawing.Point(163, 83);
            this.CurrentHandSizeLabel.Name = "CurrentHandSizeLabel";
            this.CurrentHandSizeLabel.Size = new System.Drawing.Size(140, 17);
            this.CurrentHandSizeLabel.TabIndex = 9;
            this.CurrentHandSizeLabel.Text = "Current Hand Size: #";
            // 
            // DiscardExtraCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 279);
            this.Controls.Add(this.CurrentHandSizeLabel);
            this.Controls.Add(this.MaxHandSizeLabel);
            this.Controls.Add(this.discarded);
            this.Controls.Add(this.currentCards);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.DiscardBox);
            this.Controls.Add(this.CurrentHandBox);
            this.Name = "DiscardExtraCards";
            this.Text = "DiscardExtraCards";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox CurrentHandBox;
        private System.Windows.Forms.ListBox DiscardBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Label currentCards;
        private System.Windows.Forms.Label discarded;
        private System.Windows.Forms.Label MaxHandSizeLabel;
        private System.Windows.Forms.Label CurrentHandSizeLabel;
    }
}