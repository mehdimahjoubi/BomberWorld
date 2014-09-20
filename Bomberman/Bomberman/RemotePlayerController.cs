using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    class RemotePlayerController : Controller
    {
        public override void UpdateMoves(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 direction = Vector2.Zero;
            int allowedError = 2;
            if (Player.NextPosition != Player.Position) // new position informations received
            {
                if (Player.NextPosition.X > Player.Position.X + allowedError)
                {
                    Player.PlayMoveRightAnim(deltaTime);
                    direction.X = MOVE_RIGHT;
                }
                else if (Player.NextPosition.X < Player.Position.X - allowedError)
                {
                    Player.PlayMoveLeftAnim(deltaTime);
                    direction.X = MOVE_LEFT;
                }
                else if (Player.NextPosition.Y > Player.Position.Y + allowedError)
                {
                    Player.PlayMoveDownAnim(deltaTime);
                    direction.Y = MOVE_DOWN;
                }
                else if (Player.NextPosition.Y < Player.Position.Y - allowedError)
                {
                    Player.PlayMoveUpAnim(deltaTime);
                    direction.Y = MOVE_UP;
                }
                if (direction == Vector2.Zero)
                {
                    Player.PauseAnim();
                }
                else 
                {
                    Player.Position += direction * Player.PlayerSpeed;
                }
            }
            else
            {
                Player.PauseAnim();
            }
        }
    }
}
