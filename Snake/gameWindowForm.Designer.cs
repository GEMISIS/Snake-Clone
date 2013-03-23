namespace Snake
{
    partial class gameWindowForm
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
            this.scoreLabel = new System.Windows.Forms.Label();
            this.gameScreen = new Snake.GameScreen();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(12, 9);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(47, 13);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "Score: 0";
            // 
            // gameScreen
            // 
            this.gameScreen.BackColor = System.Drawing.Color.White;
            this.gameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gameScreen.Location = new System.Drawing.Point(13, 25);
            this.gameScreen.Name = "gameScreen";
            this.gameScreen.Size = new System.Drawing.Size(599, 404);
            this.gameScreen.TabIndex = 0;
            this.gameScreen.Text = "Game!";
            // 
            // gameWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.gameScreen);
            this.DoubleBuffered = true;
            this.Name = "gameWindowForm";
            this.Text = "Snake";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GameScreen gameScreen;
        private System.Windows.Forms.Label scoreLabel;














    }
}

