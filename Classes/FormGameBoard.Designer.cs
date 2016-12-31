namespace B16_Ex05
{
    public partial class FormGameBoard
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
            this.labelPlayerOneScore = new System.Windows.Forms.Label();
            this.labelPlayerTwoScore = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlayerOneScore
            // 
            this.labelPlayerOneScore.AutoSize = true;
            this.labelPlayerOneScore.BackColor = System.Drawing.SystemColors.Control;
            this.labelPlayerOneScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPlayerOneScore.Location = new System.Drawing.Point(63, 230);
            this.labelPlayerOneScore.Name = "labelPlayerOneScore";
            this.labelPlayerOneScore.Size = new System.Drawing.Size(35, 13);
            this.labelPlayerOneScore.TabIndex = 0;
            this.labelPlayerOneScore.Text = "label1";
            // 
            // labelPlayerTwoScore
            // 
            this.labelPlayerTwoScore.AutoSize = true;
            this.labelPlayerTwoScore.BackColor = System.Drawing.SystemColors.Control;
            this.labelPlayerTwoScore.Location = new System.Drawing.Point(156, 230);
            this.labelPlayerTwoScore.Name = "labelPlayerTwoScore";
            this.labelPlayerTwoScore.Size = new System.Drawing.Size(35, 13);
            this.labelPlayerTwoScore.TabIndex = 1;
            this.labelPlayerTwoScore.Text = "label2";
            // 
            // FormGameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.labelPlayerTwoScore);
            this.Controls.Add(this.labelPlayerOneScore);
            this.Name = "FormGameBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "4 in a Raw !!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPlayerOneScore;
        private System.Windows.Forms.Label labelPlayerTwoScore;
    }
}