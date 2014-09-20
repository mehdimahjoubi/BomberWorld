using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public abstract class Controller
    {
        protected const int MOVE_UP = -1;
        protected const int MOVE_DOWN = 1;
        protected const int MOVE_LEFT = -1;
        protected const int MOVE_RIGHT = 1;

        /// <summary>
        /// Player controlled by this instance of Controller.
        /// </summary>
        public PlayerSprite Player { get; set; }

        /// <summary>
        /// Executes the player's moves.
        /// </summary>
        /// <param name="aCurrentKeyboardState">current state of the keyboard</param>
        public abstract void UpdateMoves(GameTime gameTime);
    }
}
