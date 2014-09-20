using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class PlayerController : Controller
    {
        //Saves the last change of position after each movement to be able to remove it in case of collision.
        private Vector2 LastMove = Vector2.Zero;

        //last position replicated on the network
        private Vector2 lastReplictadPosition = Vector2.Zero;
        
        public override void UpdateMoves(GameTime gameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            List<Sprite> Map = Player.GameContentManager.Sprites;

            //bomb command called.
            if (aCurrentKeyboardState.IsKeyDown(Keys.E) == true)
            {
                DropBomb(); //dropping a new bomb
            }
            
            bool allowMove = true;
            Vector2 direction = Vector2.Zero;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
            {
                direction.X = MOVE_LEFT;
                Player.PlayMoveLeftAnim(deltaTime);
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                direction.X = MOVE_RIGHT;
                Player.PlayMoveRightAnim(deltaTime);
            }

            else if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
            {
                direction.Y = MOVE_UP;
                Player.PlayMoveUpAnim(deltaTime);
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
            {
                direction.Y = MOVE_DOWN;
                Player.PlayMoveDownAnim(deltaTime);
            }
            else
            {
                Player.PauseAnim();
            }

            //handle collisions:
            for (int i = 0; i < Map.Count ; i++)
            {
                Sprite s = Map.ElementAt(i);
                if (!s.CanCollide && s.SpriteShape.Intersects(Player.SpriteShape) && s != Player)
                {
                    allowMove = false;
                    Player.Position -= LastMove;
                    break;
                }
            }

            if (allowMove)
            {
                LastMove = direction * Player.PlayerSpeed;
                Player.Position += LastMove;

                // network replication
                Vector2 positionError = Player.Position - lastReplictadPosition;
                if (Math.Abs(positionError.X) > 20 || Math.Abs(positionError.Y) > 20)
                {
                    Player.GameContentManager.Server.ReplicatePlayerPosition(Player.GameContentManager.Game.GameID,
                        Player.PlayerID, (int)Player.Position.X, (int)Player.Position.Y);
                    lastReplictadPosition = Player.Position;
                }
            }
        }

        private void DropBomb()
        {
            Vector2 bombPosition = Player.Position;

            //Bomb spawned behind the player
            ////player is looking at his left
            //if (Player.CurrentAnimFrame >= 6 && Player.CurrentAnimFrame <= 8)
            //{
            //    bombPosition.X += Player.SpriteShape.Width + 1;
            //}
            ////player is looking at his right
            //else if (Player.CurrentAnimFrame >= 3 && Player.CurrentAnimFrame <= 5)
            //{
            //    bombPosition.X -= BombSprite.BombRectangleSide + 1;
            //}
            ////player is looking up
            //else if (Player.CurrentAnimFrame >= 9 && Player.CurrentAnimFrame <= 11)
            //{
            //    bombPosition.Y += Player.SpriteShape.Height + 1;
            //}
            ////player is looking down
            //else
            //{
            //    bombPosition.Y -= BombSprite.BombRectangleSide + 1;
            //}

            //Bomb spawned in player's position
            Rectangle bombShape = new Rectangle((int)bombPosition.X, (int)bombPosition.Y,
                BombSprite.BombRectangleSide, BombSprite.BombRectangleSide);
            List<Sprite> Sprites = Player.GameContentManager.Sprites;

            for (int i = 0; i < Sprites.Count; i++)
            {
                if(Sprites.ElementAt(i).GetType() == typeof(PlayerSprite))
                    continue;
                if (Sprites.ElementAt(i).SpriteShape.Intersects(bombShape))
                    return;
            }
            Player.GameContentManager.SpawnBomb(bombPosition);

            //network replication
            Player.GameContentManager.Server.ReplicateSpawnedBombPosition(Player.GameContentManager.Game.GameID,
                (int)bombPosition.X, (int)bombPosition.Y);
        }
    }
}
