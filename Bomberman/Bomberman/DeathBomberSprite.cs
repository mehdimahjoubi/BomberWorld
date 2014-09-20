using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    class DeathBomberSprite : Sprite
    {
        // frame dimensions
        private const int FRAME_WIDTH = 40;
        private const int FRAME_HEIGHT = 40;

        // frame from sprite sheet currently displayed
        private int CurrentAnimFrame = 0;

        // interval for frame switching
        private int interval = 300;

        private float Timer = 0;
        
        public DeathBomberSprite()
            : base("bomberDeath")
        {
            SpriteShape = new Rectangle();
        }

        public override void Destroy()
        {
            // Do not destroy! The sprite will be automaticaly removed once the animation is over.
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(SpriteTexture, Position,
                new Rectangle(CurrentAnimFrame * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > interval)
            {
                CurrentAnimFrame++;
                Timer = 0;
            }
            if (CurrentAnimFrame > 4)
            {
                base.Destroy();
            }
        }
    }
}
