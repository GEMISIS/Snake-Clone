using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Snake
{
    public partial class HighScores : Form
    {
        public HighScores(int score)
        {
            InitializeComponent();

            this.loadHighScores(score);
            this.saveHighScore();
        }

        private void saveHighScore()
        {
            FileStream fs = File.Open("scores.txt", FileMode.Create);
            foreach (ListViewItem item in this.highScoreList.Items)
            {
                if (item.Text.Length > 3)
                {
                    fs.Write(Encoding.ASCII.GetBytes(item.Text.Substring(3) + "\n"), 0, Encoding.ASCII.GetBytes(item.Text.Substring(3) + "\n").Length);
                }
            }
            fs.Close();
        }

        private void loadHighScores(int newScore)
        {
            if (File.Exists("scores.txt"))
            {
                StreamReader reader = File.OpenText("scores.txt");
                string score = string.Empty;
                int scoreLabel = 0;
                while ((score = reader.ReadLine()) != null && scoreLabel < this.highScoreList.Items.Count)
                {
                    if (newScore >= Int32.Parse(score))
                    {
                        this.highScoreList.Items[scoreLabel].Text = (scoreLabel + 1) + ") " + newScore;
                        newScore = -1;
                        scoreLabel += 1;
                    }
                    if (scoreLabel < this.highScoreList.Items.Count)
                    {
                        this.highScoreList.Items[scoreLabel].Text = (scoreLabel + 1) + ") " + score;
                        scoreLabel += 1;
                    }
                }
                reader.Close();
                if (scoreLabel < this.highScoreList.Items.Count && newScore != -1)
                {
                    this.highScoreList.Items[scoreLabel].Text = (scoreLabel + 1) + ") " + newScore;
                }
            }
            else
            {
                this.highScoreList.Items[0].Text = "1) " + newScore;
            }
        }
    }
}
