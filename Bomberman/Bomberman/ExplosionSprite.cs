using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    class ExplosionSprite : Sprite
    {
        /// <summary>
        /// Set/get the interval of time between increases of explosion surface.
        /// </summary>
        public float ExplosionExpandingInterval { get; set; }

        // timer for explosion expanding
        private float explosionExpandTimer = 0;

        // time before explosion ends
        private float explosionEndTimer = 1500;

        // Number of explosion blocks shown on each side of the central one.
        private int explosionRadius;

        //Contains instances of ExplosionSprite according the radius.
        private List<ExplosionSprite> explosionEffects = new List<ExplosionSprite>();

        //Contains the top explosion effects
        private List<ExplosionSprite> explosionTopEffects = new List<ExplosionSprite>();

        //Contains the bottom explosion effects
        private List<ExplosionSprite> explosionBottomEffects = new List<ExplosionSprite>();

        //contains the right side explosion effects
        private List<ExplosionSprite> explosionRightEffects = new List<ExplosionSprite>();

        //contains the left explosion effects
        private List<ExplosionSprite> explosionLeftEffects = new List<ExplosionSprite>();

        // determine whether the explosion can expand in the given direction or not
        private bool topExpansionAllowed = true;
        private bool bottomExpansionAllowed = true;
        private bool rightExpansionAllowed = true;
        private bool leftExpansionAllowed = true;

        /// <summary>
        /// Creates an explosion effect according to the parameters.
        /// </summary>
        /// <param name="center">Left top position of the central block of the explosion.</param>
        /// <param name="radius">Number of explosion blocks shown on each side of the central one.</param>
        public ExplosionSprite(Vector2 center, int radius = 0)
            : base("explosion")
        {
            Position = center;
            explosionRadius = radius;
            ExplosionExpandingInterval = 100;
            Scale = 0.4f;
            explosionEffects.Add(this);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            int collisionDimunition = 7;
            for (int i = 0; i < explosionEffects.Count; i++)
            {
                ExplosionSprite e = explosionEffects.ElementAt(i);
                e.SpriteShape = new Rectangle((int)e.Position.X + collisionDimunition, (int)e.Position.Y + collisionDimunition,
                    (int)(e.SpriteTexture.Width * e.Scale) - 2*collisionDimunition, (int)(e.SpriteTexture.Height * e.Scale) - 2*collisionDimunition);
                theSpriteBatch.Draw(e.SpriteTexture, e.Position - new Vector2((int)(e.SpriteTexture.Width * 0.1), (int)(e.SpriteTexture.Height * 0.1)),
                    new Rectangle(0, 0, e.SpriteTexture.Width, e.SpriteTexture.Height),
                    e.DrawColor, 0.0f, Vector2.Zero, e.Scale + 0.2f, SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            explosionExpandTimer += deltaTime;
            explosionEndTimer -= deltaTime;
            if (explosionEndTimer <= 0)
            {
                explosionEffects.Clear();
                GameContentManager.Sprites.Remove(this);
            }
            if (explosionExpandTimer >= ExplosionExpandingInterval && explosionRadius > 0) // explosion has to expand
            {
                // explosion hasn't started expanding yet
                if (explosionTopEffects.Count == 0)
                {
                    ExplosionSprite center = explosionEffects.ElementAt(0);
                    Vector2 offsetPos = Vector2.Zero;
                    ExplosionSprite e;

                    // top expand
                    if (topExpansionAllowed)
                    {
                        offsetPos = new Vector2(0, center.SpriteTexture.Height * center.Scale);
                        e = new ExplosionSprite(center.Position - offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionTopEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // bottom expand
                    if (bottomExpansionAllowed)
                    {
                        offsetPos = new Vector2(0, center.SpriteTexture.Height * center.Scale);
                        e = new ExplosionSprite(center.Position + offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionBottomEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // right expand
                    if (rightExpansionAllowed)
                    {
                        offsetPos = new Vector2(center.SpriteTexture.Width * center.Scale, 0);
                        e = new ExplosionSprite(center.Position + offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionRightEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // left expand
                    if (leftExpansionAllowed)
                    {
                        offsetPos = new Vector2(center.SpriteTexture.Width * center.Scale, 0);
                        e = new ExplosionSprite(center.Position - offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionLeftEffects.Add(e);
                        explosionEffects.Add(e);
                    }
                }
                else if (explosionRadius > 0) //in case this isn't the first expanding
                {
                    ExplosionSprite lastEffect;
                    Vector2 offsetPos = Vector2.Zero;
                    ExplosionSprite e;
                    
                    // top expand
                    if (topExpansionAllowed)
                    {
                        lastEffect = explosionTopEffects.ElementAt(explosionTopEffects.Count - 1);
                        offsetPos = new Vector2(0, lastEffect.SpriteTexture.Height * lastEffect.Scale);
                        e = new ExplosionSprite(lastEffect.Position - offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionTopEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // bottom expand
                    if (bottomExpansionAllowed)
                    {
                        lastEffect = explosionBottomEffects.ElementAt(explosionBottomEffects.Count - 1);
                        offsetPos = new Vector2(0, lastEffect.SpriteTexture.Height * lastEffect.Scale);
                        e = new ExplosionSprite(lastEffect.Position + offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionBottomEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // right expand
                    if (rightExpansionAllowed)
                    {
                        lastEffect = explosionRightEffects.ElementAt(explosionRightEffects.Count - 1);
                        offsetPos = new Vector2(lastEffect.SpriteTexture.Width * lastEffect.Scale, 0);
                        e = new ExplosionSprite(lastEffect.Position + offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionRightEffects.Add(e);
                        explosionEffects.Add(e);
                    }

                    // left expand
                    if (leftExpansionAllowed)
                    {
                        lastEffect = explosionLeftEffects.ElementAt(explosionLeftEffects.Count - 1);
                        offsetPos = new Vector2(lastEffect.SpriteTexture.Width * lastEffect.Scale, 0);
                        e = new ExplosionSprite(lastEffect.Position - offsetPos, 0);
                        e.LoadContent(GameContentManager.Game.Content);
                        e.GameContentManager = GameContentManager;
                        explosionLeftEffects.Add(e);
                        explosionEffects.Add(e);
                    }
                }
                explosionExpandTimer = 0;
                explosionRadius--;
            }
            handleCollisions();
        }

        // destory every destroyable sprite colliding with the explosion effect
        private void handleCollisions()
        {
            for (int j = 0; j < explosionEffects.Count; j++)
            {
                ExplosionSprite effect = explosionEffects.ElementAt(j);
                for (int i = 0; i < GameContentManager.Sprites.Count; i++)
                {
                    Sprite s = GameContentManager.Sprites.ElementAt(i);
                    if (s.SpriteShape.Intersects(effect.SpriteShape) && s != this)
                    {
                        s.Destroy();
                        if (!s.IsDestroyable)
                        {
                            if (explosionTopEffects.Contains(effect))
                            {
                                explosionEffects.Remove(effect);
                                topExpansionAllowed = false;
                            }
                            else if (explosionBottomEffects.Contains(effect))
                            {
                                explosionEffects.Remove(effect);
                                bottomExpansionAllowed = false;
                            }
                            else if (explosionRightEffects.Contains(effect))
                            {
                                explosionEffects.Remove(effect);
                                rightExpansionAllowed = false;
                            }
                            else if (explosionLeftEffects.Contains(effect))
                            {
                                explosionEffects.Remove(effect);
                                leftExpansionAllowed = false;
                            }
                        }
                    }
                }
            }
        }

        public override void Destroy()
        {
        }
    }
}
