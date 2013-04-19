using System;
using System.Windows.Forms;

namespace Snake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable nicer visuals for the game.
            Application.EnableVisualStyles();

            // Create a new game form.
            GameWindowForm gameForm = new GameWindowForm();
            // Start running the game form.
            Application.Run(gameForm);

            // Check that the results from the game form are yes.
            while (gameForm.DialogResult == DialogResult.Yes)
            {
                // If so, recreate the game form.
                gameForm = new GameWindowForm();
                // Start running the game form again.
                Application.Run(gameForm);
            }
        }
    }
}
