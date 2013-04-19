using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Snake
{
    /// <summary>
    /// This is the form where the main gameplay happens for Snake.
    /// Everything from the game controls to updating the game screen
    /// component is done in this class.
    /// </summary>
    public partial class GameWindowForm : Form
    {
        /// <summary>
        /// The tile's width.
        /// </summary>
        private const int TILE_WIDTH = 32;
        /// <summary>
        /// The tile's height.
        /// </summary>
        private const int TILE_HEIGHT = 32;

        /// <summary>
        /// The timers for updating graphics.
        /// </summary>
        private Timer gfxTimer = new Timer();
        /// <summary>
        /// The timers for updating gameplay.
        /// </summary>
        private Timer gameTimer = new Timer();

        /// <summary>
        /// The velocity of the player's character.
        /// </summary>
        private Point velocity = new Point(1, 0);
        
        /// <summary>
        /// The player's score.
        /// </summary>
        private int score = 0;

        /// <summary>
        /// A variable to add to the score to prevent it from being messed with.
        /// </summary>
        private int scoreSubtractor = 0;

        /// <summary>
        /// The scored after it has been hashed.
        /// </summary>
        private string hashedScore = "";

        /// <summary>
        /// The constructor for the main game window form.
        /// </summary>
        public GameWindowForm()
        {
            // Initialize the main parts of the compnent.
            InitializeComponent();

            // Set the score and score subtractor to the same value.
            // Only the score changes.
            score = scoreSubtractor = new Random().Next();

            // Rehash the score.  First the score is bitshifted to the right by 8.  This is done
            // to obfuscate the score even more before it is encrypted.  Then it is hashed
            // to add one more layer of obfuscation.
            hashedScore = Encoding.ASCII.GetString((new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes((score << 8).ToString())));

            // Add the first background layer (the bottom one).
            gameScreen.addBackgroundLayer(Snake.Properties.Resources.background0);
            // Add the second background layer (the top one).
            gameScreen.addBackgroundLayer(Snake.Properties.Resources.background1);

            // Create the food sprite (index 0).
            gameScreen.addSprite(0, Snake.Properties.Resources.foodDot, 32, 32);
            // Create the main sprite (index 1).
            gameScreen.addSprite(1, Snake.Properties.Resources.mainDot, 0, 0);

#if DEBUG
            // If the game is being debugged, set the food to a specific spot each time
            // to aid in testing.
            gameScreen.setSpritePosition(0, 64, 64);
#else
            // If not, set the sprite to a random position within the screen's limits.
            gameScreen.setSpritePosition(0, (new Random().Next((gameScreen.Width - 64) / TILE_WIDTH)) * TILE_WIDTH, (new Random().Next((gameScreen.Height - 64) / TILE_HEIGHT)) * TILE_HEIGHT);
            // Then loop through all of the objects.
            for (int i = 1; i < (score - scoreSubtractor) + 2; i += 1)
            {
                // Check if there is a collision between the food
                // object and the sprite.
                if (gameScreen.checkCollision(0, i))
                {
                    // If there is a collision, restart the loop
                    // through all of the other sprites and
                    // set the food to a new random position.
                    // Note that it is set to 0 because it will be incremented during
                    // the next iteration.
                    gameScreen.setSpritePosition(0, (new Random().Next((gameScreen.Width - 64) / TILE_WIDTH)) * TILE_WIDTH, (new Random().Next((gameScreen.Height - 64) / TILE_HEIGHT)) * TILE_HEIGHT);
                    i = 0;
                }
            }
#endif

            // Set the event handler for when a key is pressed.
            gameScreen.KeyDown += new KeyEventHandler(gameScreenForm_KeyDown);

            // Set the game timer to be updated ever 750 milliseconds.
            gameTimer.Interval = 750;
            // Set the tick event handler for the game timer.
            gameTimer.Tick += new EventHandler(gameTimer_Tick);

            // Set the tick event handler for the graphics timer.
            gfxTimer.Tick += new EventHandler(gfxTimer_Tick);

            // Start the graphics timer first so the graphics
            // are in place before gameplay starts (from the game timer).
            gfxTimer.Start();
            // Start the game timer second now that the user
            // can see the gameplay.
            gameTimer.Start();
        }

        /// <summary>
        /// The graphics timer's tick event.  This is where all of the
        /// game's graphics are updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gfxTimer_Tick(object sender, EventArgs e)
        {
            // Refresh the game screen, which forces the component
            // to call its paint method.
            gameScreen.Refresh();
        }

        /// <summary>
        /// The game timer's tick event.  This is where all of the
        /// main gameplay happens, such as updating the movement of
        /// objects, collisions, point increasing, as well as a constant
        /// checking of the hashed score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gameTimer_Tick(object sender, EventArgs e)
        {
            // Check whether the currently stored hashed score is equal to the score being
            // hashed again.  This helps to make sure that the score cannot be edited in memory, by making
            // it harder to find, but is not a full proof method unfortunately, but it does a very good
            // job.
            if (!hashedScore.Equals(Encoding.ASCII.GetString((new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes((score << 8).ToString())))))
            {
                // If the score is not equal to it, the game is stopped, a message
                // is displayed, and then the game ends.

                // So first, the game timer is stopped so that the gameplay stops immediately.
                gameTimer.Stop();
                // Then the graphics timer is stopped since the gameplay has
                // halted.
                gfxTimer.Stop();
                // A message box to let them know that they have been found is displayed.
                // The message box will halt the program until the "ok" button on it is
                // clicked.
                MessageBox.Show("Hacker!!!!");
                // Finally the game is closed.
                this.Close();
            }

            // Then we get the location of the main character's sprite.
            Point previousLocation = gameScreen.getSpritePosition(1);
            // Next, the main character's sprite position is updated.
            gameScreen.setSpritePosition(1, gameScreen.getSpritePosition(1).X + velocity.X * TILE_WIDTH, gameScreen.getSpritePosition(1).Y + velocity.Y * TILE_HEIGHT);

            // Next, the tail sprites are looped for (which is simply the actual score + 1).
            // Note that we still must subtract the score subtractor.
            for (int i = 1; i < (score - scoreSubtractor) + 1; i += 1)
            {
                // While looping through, each tail sprite must be updated based on the
                // previous tail sprite, with the head being the initial location.
                // (aka: The tail follows the head around the game).

                // So first, a temporary position of the tail sprite is grabbed.
                Point tempPreviousLocation = gameScreen.getSpritePosition(1 + i);
                // Then the tail sprite's location is set to the previously checked sprite's
                // location.
                gameScreen.setSpritePosition(1 + i, previousLocation.X, previousLocation.Y);
                // Finally, the previous sprite location is set to the temporary one.
                previousLocation = tempPreviousLocation;

                // Next, we check if the head of the snake is
                // colliding with this piece of it's tail.
                if (gameScreen.checkCollision(1, i + 1))
                {
                    // If the head is touching a tail piece, we must exit the game.

                    // To start, the game timer is stopped to make sure that gameplay is
                    // definitely halted.
                    gameTimer.Stop();
                    // Then, the graphics timer is stopped so the graphics don't keep updating.
                    gfxTimer.Stop();
                    // Next, a game over message is shown with the player's final score.
                    // The message box will halt the program until an option is selected, and
                    // then store the results.
                    DialogResult results = MessageBox.Show("Game Over! Final Score: " + (score - scoreSubtractor) + " Play again?", "Game Over", MessageBoxButtons.YesNo);

                    // Check whether the currently stored hashed score is equal to the score being
                    // hashed again.  This helps to make sure that the score cannot be edited in memory, by making
                    // it harder to find, but is not a full proof method unfortunately, but it does a very good
                    // job.
                    if (hashedScore.Equals(Encoding.ASCII.GetString((new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes((score << 8).ToString())))))
                    {
                        new HighScores((score - scoreSubtractor)).ShowDialog();
                    }

                    // After the message box has been clicked, the game closes.
                    this.Close();

                    if (results == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    }
                    else
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.No;
                    }
                }
            }

            // Next, we check for a collision between the snake's head and a piece of food.
            if (gameScreen.checkCollision(0, 1))
            {
                // If the snake is touching a piece of food, then the score must
                // be incremented.  If it is incremented, then there is some obfuscation
                // done to prevent it from being tampered with.  The new score is stored to
                // a temporary value, and then the score is set to the temporary value while
                // being rehashed, so that it can't be traced as easily.

                // First, the score is incremented and stored in temporary score value.
                int tempScore = score + 1;
                // Immediatly afterwords, the score is incremented, with the
                // temporary score value being stored in the real score value, all
                // while being hashed.
                hashedScore = Encoding.ASCII.GetString((new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes(((score = tempScore) << 8).ToString())));

                // Next, the score's text label is set to the real score.
                scoreLabel.Text = "Score: " + (score - scoreSubtractor);

                // Add the new tail sprite to the screen.
                gameScreen.addSprite((score - scoreSubtractor) + 1, Snake.Properties.Resources.tailDot, previousLocation.X, previousLocation.Y);

#if DEBUG
                // For debug mode, check if the game timer's interval is greater than 50 milliseconds.
                if (gameTimer.Interval > 50)
                {
                    // If so, decrement the game timer's interval by 50 milliseconds to
                    // increase the difficulty.
                    gameTimer.Interval -= 50;
                }
                // Then set the sprite's position to a standard position for easier testing.
                gameScreen.setSpritePosition(0, 64, 64);
#else
                // For release mode, check both the game's timer interval, and if the score
                // is divisable by 4.
                if ((score - scoreSubtractor) % 4 == 0 && gameTimer.Interval > 50)
                {
                    // If so, decrement the game timer's interval by 50 milliseconds to
                    // increase the difficulty.
                    gameTimer.Interval -= 50;
                }
                // Then set the sprite to a random position within the screen limits.
                gameScreen.setSpritePosition(0, (new Random().Next((gameScreen.Width - 64) / TILE_WIDTH)) * TILE_WIDTH, (new Random().Next((gameScreen.Height - 64) / TILE_HEIGHT)) * TILE_HEIGHT);
                // Then loop through all of the objects.
                for (int i = 1; i < (score - scoreSubtractor) + 2; i += 1)
                {
                    // Check if there is a collision between the food
                    // object and the sprite.
                    if (gameScreen.checkCollision(0, i))
                    {
                        // If there is a collision, restart the loop
                        // through all of the other sprites and
                        // set the food to a new random position.
                        // Note that it is set to 0 because it will be incremented during
                        // the next iteration.
                        gameScreen.setSpritePosition(0, (new Random().Next((gameScreen.Width - 64) / TILE_WIDTH)) * TILE_WIDTH, (new Random().Next((gameScreen.Height - 64) / TILE_HEIGHT)) * TILE_HEIGHT);
                        i = 0;
                    }
                }
#endif
            }
        }

        /// <summary>
        /// The event for when a key is pressed on the keyboard. This is
        /// for updating the player's movements, as well as the pause button.
        /// The controls are AWSD, with Enter to pause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gameScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Switch through the various key codes.
            switch (e.KeyCode)
            {
                case Keys.A:
                    // If the key is the A key, the player moves left.
                    velocity.X = -1;
                    velocity.Y = 0;
                    break;
                case Keys.D:
                    // If the key is the D key, the player moves right.
                    velocity.X = 1;
                    velocity.Y = 0;
                    break;
                case Keys.W:
                    // If the key is the W key, the player moves up.
                    velocity.X = 0;
                    velocity.Y = -1;
                    break;
                case Keys.S:
                    // If the key is the S key, the player moves down.
                    velocity.X = 0;
                    velocity.Y = 1;
                    break;
                case Keys.Enter:
                    // If the key is the Enter key, the game is paused.
                    // First the game timer is stopped so that gameplay won't
                    // continue unseen.
                    gameTimer.Stop();
                    // Then, the graphics timer is stopped since the gameplay
                    // has stopped.
                    gfxTimer.Stop();
                    // Next, a message box saying pause is shown.  This will halt
                    // the program until the "ok" button is clicked.
                    MessageBox.Show("Paused!");
                    // Next, the graphics are started again so the player can see what
                    // happens.
                    gfxTimer.Start();
                    // Then, the game timer is started again since the player can
                    // see the graphics updating.
                    gameTimer.Start();
                    break;
            }
        }
    }
}
