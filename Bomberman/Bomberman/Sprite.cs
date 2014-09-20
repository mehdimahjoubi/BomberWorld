using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    public class Sprite
    {
        #region Properties

        /// <summary>
        /// if true, the sprite will be destroyed if caught in an explosion
        /// </summary>
        public bool IsDestroyable { get; set; }

        /// <summary>
        /// Reference to the GameContentManager containing this Sprite.
        /// </summary>
        public GameContentManager GameContentManager { get; set; }

        /// <summary>
        /// The current position of the sprite.
        /// </summary>
        protected Vector2 position;
        public virtual Vector2 Position {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// The texture object used when drawing the sprite.
        /// </summary>
        public Texture2D SpriteTexture { get; protected set; }

        /// <summary>
        /// The asset name for the Sprite's Texture.
        /// </summary>
        public string AssetName { get; protected set; }

        /// <summary>
        /// Scaled shape of the sprite.
        /// </summary>
        public Rectangle SpriteShape { get; protected set; }

        /// <summary>
        /// Color used by the SpriteBatch to draw the sprite.
        /// </summary>
        public Color DrawColor { get; set; }

        /// <summary>
        /// The scale of the sprite. Allows to increase/decrease its size.
        /// </summary>
        protected float scale;
        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                if (SpriteTexture != null)
                {
                    SpriteShape = new Rectangle((int)Position.X, (int)Position.Y, (int)(SpriteTexture.Width * Scale),
                    (int)(SpriteTexture.Height * Scale));
                }
            }
        }

        protected bool canCollide;
        public bool CanCollide
        {
            get
            {
                return canCollide;
            }
            set
            {
                canCollide = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assertName">The asset name for the Sprite's Texture.</param>
        public Sprite(string assertName)
        {
            AssetName = assertName;
            Position = Vector2.Zero;
            scale = 1;
            DrawColor = Color.White;
            IsDestroyable = true;
            CanCollide = false;
        }

        /// <summary>
        /// Load the texture for the sprite using the Content Pipeline.
        /// </summary>
        /// <param name="theContentManager">ContentManager of the game.</param>
        /// <param name="theAssetName">The asset name for the Sprite's Texture.</param>
        public void LoadContent(ContentManager theContentManager)
        {
            SpriteTexture = theContentManager.Load<Texture2D>(AssetName);
        }

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="theSpriteBatch">Reference to the SpriteBatch used to draw the graphics.</param>
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            int collisionDiminution = 4;
            SpriteShape = new Rectangle((int)Position.X + collisionDiminution, (int)Position.Y + collisionDiminution,
                (int)(SpriteTexture.Width * Scale) - 2 * collisionDiminution, (int)(SpriteTexture.Height * Scale) - 2 * collisionDiminution);
            theSpriteBatch.Draw(SpriteTexture, Position,
                new Rectangle(0, 0, SpriteTexture.Width, SpriteTexture.Height),
                DrawColor, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Destroy()
        {
            if (IsDestroyable)
            {
                GameContentManager.Sprites.Remove(this);
            }
        }
    }
}
