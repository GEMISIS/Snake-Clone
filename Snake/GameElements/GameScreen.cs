using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// A screen where the actual game is displayed.  Can
    /// have sprites and backgrounds (layered on top of each other) drawn
    /// on it.
    /// </summary>
    public partial class GameScreen : Control
    {
        /// <summary>
        /// The list of sprites that are displayed.
        /// Stored as a picture box because then we have
        /// coordinates and boundaries for it with an integer as its key
        /// (ie: the index).  This particular list is also sorted by the keys,
        /// which allows for layering of the sprites.
        /// </summary>
        private SortedDictionary<int, PictureBox> sprites;

        /// <summary>
        /// The constructor for a game screen.
        /// Sets the component to be double buffered
        /// to prevent flickering, and intializes
        /// the sprite list.
        /// </summary>
        public GameScreen()
        {
            // Initialize the main component.
            InitializeComponent();

            // Set the component to be double buffered.
            this.DoubleBuffered = true;

            // Initialize the sprite list.
            sprites = new SortedDictionary<int, PictureBox>();
        }

        /// <summary>
        /// Adds a sprite to the component.
        /// </summary>
        /// <param name="index">The of the sprite.</param>
        /// <param name="spriteImage">The image to use for the sprite.</param>
        /// <param name="x">The X position of the sprite.</param>
        /// <param name="y">The Y position of the sprite.</param>
        public void addSprite(int index, Image spriteImage, int x, int y)
        {
            // A temporary picture box is created, and then
            // it's various properties (such as the image, the size, etc.)
            // are set, and then it is added to the list of picture boxes.
            // Future implementation will include a way to set the various sprites
            // to specific indexes, to make retrieving them easier.

            // A temporary PictureBox to add.
            PictureBox pic = new PictureBox();

            // Set the image.
            pic.Image = spriteImage;
            // Set the width of the image.
            pic.Width = spriteImage.Width;
            // Set the height of the image.
            pic.Height = spriteImage.Height;
            // Set the location.
            pic.Location = new Point(x, y);

            // Add the PictureBox to the list of sprites.
            if (sprites.ContainsKey(index))
            {
                sprites[index] = pic;
            }
            else
            {
                sprites.Add(index, pic);
            }

            // Dipose of the PictureBox.
            pic.Dispose();
        }

        /// <summary>
        /// Adds a background layer.  These are drawn on top of each other
        /// in the order that they are added (the first being the bottom layer and last
        /// being the top layer).
        /// </summary>
        /// <param name="backgroundImage">The background image to add.</param>
        public void addBackgroundLayer(Image backgroundImage)
        {
            // If there is no background image already, then
            // it is set to the chosen image, otherwise it needs
            // to be drawn on top of the old background image.

            // Check if there is an image already set as a background.
            if (this.BackgroundImage != null)
            {
                // If so, create a graphics object from the image.
                Graphics g = Graphics.FromImage(this.BackgroundImage);
                // Then draw the new background image on top of the old background image.
                g.DrawImage(backgroundImage, 0, 0, this.BackgroundImage.Width, this.BackgroundImage.Height);
                // Dipose of the graphics object to free up resources.
                g.Dispose();
            }
            else
            {
                // If there is no image, set the default background image.
                this.BackgroundImage = backgroundImage;
            }
        }

        /// <summary>
        /// Checks whether two sprites have collided.
        /// </summary>
        /// <param name="index1">The index of the first sprite.</param>
        /// <param name="index2">The index of the second sprite.</param>
        /// <returns>Returns true if there is a collision, false otherwise.</returns>
        public bool checkCollision(int index1, int index2)
        {
            // Check that the index is in the correct range.
            if (index1 < sprites.Count && index2 < sprites.Count)
            {
                // Returns whether the two rectangles around the sprites are intersecting.
                return (sprites[index1].Bounds.IntersectsWith(sprites[index2].Bounds));
            }
            // Return false otherwise.
            return false;
        }

        /// <summary>
        /// Set the sprite at the desired index's position.
        /// </summary>
        /// <param name="index">The index of the sprite to set the position of.</param>
        /// <param name="x">The new X position of the sprite.</param>
        /// <param name="y">The new Y position of the sprite.</param>
        public void setSpritePosition(int index, int x, int y)
        {
            // Check that the index is in the correct range.
            if (index < sprites.Count)
            {
                // Set the sprite position by creating it as a new point.
                sprites[index].Location = new Point(x, y);
            }
        }

        /// <summary>
        /// Get the sprite position.
        /// </summary>
        /// <param name="index">The index of the sprite to get the position of.</param>
        /// <returns>Returns the sprite position as a point.</returns>
        public Point getSpritePosition(int index)
        {
            // Check that the index is in the correct range.
            if (index < sprites.Count)
            {
                // Get the sprite position as a point.
                return sprites[index].Location;
            }
            // Return an empty point if the sprite index is invalid.
            return Point.Empty;
        }

        /// <summary>
        /// The new paint method.
        /// </summary>
        /// <param name="pe">The arguments for the OnPaint method, mainly
        /// used to retrive the grahpics object.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Loop through the list of sprites.
            foreach (KeyValuePair<int, PictureBox> sprite in sprites)
            {
                // Check that the sprite's image isn't null.
                if (sprite.Value.Image != null)
                {
                    // Draw the sprite to the screen.
                    pe.Graphics.DrawImage(sprite.Value.Image, sprite.Value.Location);
                }
            }
            // Call the base's OnPaint method with the same paint event arguments.
            base.OnPaint(pe);
        }
    }
}
