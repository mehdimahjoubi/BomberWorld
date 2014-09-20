using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Bomberman
{
    class BombSprite : Sprite
    {
        // initial scale applied to a new created bomb.
        private const float BOMB_INITIAL_SCALE = 0.4f;

        // Texture Width/height.
        private const int BOMB_TEXTURE_SIDE = 100;

        /// <summary>
        /// intial size of the bomb's SpriteShape Rectangle side.
        /// </summary>
        public const int BombRectangleSide = (int)(BOMB_TEXTURE_SIDE * BOMB_INITIAL_SCALE);
        
        /// <summary>
        /// Indicates the amount of time before the bomb explodes.
        /// </summary>
        public float BombTimer { get; private set; }

        //Stores the elapsed time since the bomb color has been changed.
        private float colorSwithTimer = 0;

        public override Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                SpriteShape = new Rectangle((int)Position.X, (int)Position.Y, BombRectangleSide, BombRectangleSide);
            }
        }

        public BombSprite()
            : base("bomb")
        {
            BombTimer = 3000;
            Scale = BOMB_INITIAL_SCALE;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            BombTimer -= deltaTime;
            colorSwithTimer += deltaTime;
            if (BombTimer <= 0)
            {
                Explode();
                return;
            }
            if (colorSwithTimer > 200)
            {
                if (DrawColor == Color.White)
                {
                    DrawColor = Color.Red;
                }
                else
                {
                    DrawColor = Color.White;
                }
                colorSwithTimer = 0;
            }

            bool startCollisions = true;
            if (GameContentManager.LocalPlayer.SpriteShape.Intersects(SpriteShape))
            {
                startCollisions = false;
            }
            foreach (PlayerSprite p in GameContentManager.RemotePlayers)
            {
                if (p.SpriteShape.Intersects(SpriteShape))
                {
                    startCollisions = false;
                    break;
                }
            }
            if (startCollisions)
            {
                CanCollide = false;
            }
        }

        private void Explode()
        {
            var e = new ExplosionSprite(Position, 3);
            e.GameContentManager = GameContentManager;
            e.LoadContent(GameContentManager.Game.Content);
            GameContentManager.Sprites.Add(e);
            GameContentManager.Sprites.Remove(this);
            SoundEffect explosion = GameContentManager.Game.Content.Load<SoundEffect>("explo");
            explosion.Play();
        }

        public override void Destroy()
        {
        }
    }
}
