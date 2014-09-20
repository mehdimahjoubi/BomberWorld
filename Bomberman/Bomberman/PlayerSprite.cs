using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Bomberman
{
    public class PlayerSprite : Sprite
    {
        //proportion of a single frame from the playersSpriteSheet.
        const int PLAYER_HEIGHT = 40; //45
        const int PLAYER_WIDTH = 40; //30

        //used to store elapsed time.
        private float timer = 0;

        //fixes an interval for the animation's frame changing based on the current PlayerSpeed.
        private int animSpeedOffset = 40;

        public enum BombermanColor
        {
            WHITE,
            BLACK,
            RED,
            BLUE
        }

        #region Properties

        /// <summary>
        /// allow a unique identification of the players in the network
        /// </summary>
        public string PlayerID { get; set; }
        
        /// <summary>
        /// Controller handling the player movements.
        /// </summary>
        public Controller Controller { get; set; }

        /// <summary>
        /// next position to be reach by the player if controlled by a RemotePlayerController
        /// </summary>
        public Vector2 NextPosition { get; set; }

        /// <summary>
        /// Set/get the movement speed of the player.
        /// </summary>
        public float PlayerSpeed { get; set; }

        /// <summary>
        /// Vertical portion of the playersSpriteSheet containing the player assets for a given PlayerColor.
        /// </summary>
        public int PlayerIndex { get; set; }

        /// <summary>
        /// Frame to be displayed from the SpriteSheet according to the animation played.
        /// </summary>
        public int CurrentAnimFrame { get; private set; }

        /// <summary>
        /// Determine the color of display for the player.
        /// </summary>
        private BombermanColor playerColor;
        public BombermanColor PlayerColor
        {
            get
            {
                return playerColor;
            }
            set
            {
                playerColor = value;
                if (value == BombermanColor.WHITE)
                {
                    PlayerIndex = 0;
                }
                else if (value == BombermanColor.BLACK)
                {
                    PlayerIndex = 1;
                }
                else if (value == BombermanColor.RED)
                {
                    PlayerIndex = 2;
                }
                else
                {
                    PlayerIndex = 3;
                }
            }
        }

        public override float Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                scale = value;
                SpriteShape = new Rectangle(CurrentAnimFrame * PLAYER_WIDTH, PlayerIndex * PLAYER_HEIGHT,
                    (int)(SpriteTexture.Width * Scale), (int)(SpriteTexture.Height * Scale));
            }
        }

        #endregion

        /// <summary>
        /// Calls base class contructor to set the right AssetName.
        /// </summary>
        public PlayerSprite()
            : base("playersSpriteSheet3")
        {
            CurrentAnimFrame = 0;
            PlayerColor = BombermanColor.WHITE;
            PlayerSpeed = 2.75f;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            SpriteShape = new Rectangle((int)Position.X, (int)Position.Y,
                (int)(PLAYER_WIDTH * Scale), (int)(PLAYER_HEIGHT * Scale));
            theSpriteBatch.Draw(SpriteTexture, Position,
                new Rectangle(CurrentAnimFrame * PLAYER_WIDTH, PlayerIndex * PLAYER_HEIGHT, PLAYER_WIDTH, PLAYER_HEIGHT),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if(!GameContentManager.isGameEnded)
                Controller.UpdateMoves(gameTime);
        }

        /// <summary>
        /// determine wether the player is controlled by a remote controller or is local
        /// </summary>
        /// <returns>true if the player is remote controlled</returns>
        public bool HasRemotePlayerController()
        {
            try
            {
                RemotePlayerController RPC = (RemotePlayerController)Controller;
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public override void Destroy()
        {
            if (!HasRemotePlayerController())
            {
                KillPlayer();

                //network replication
                GameContentManager.Server.ReplicatePlayerDeath(GameContentManager.Game.GameID, PlayerID);
            }
        }

        /// <summary>
        /// kill the player and remove him from screen
        /// </summary>
        public void KillPlayer()
        {
            base.Destroy();
            DeathBomberSprite deadBomberMan = new DeathBomberSprite();
            deadBomberMan.LoadContent(GameContentManager.Game.Content);
            deadBomberMan.GameContentManager = GameContentManager;
            deadBomberMan.Position = Position;
            GameContentManager.Sprites.Add(deadBomberMan);
            SoundEffect explosion = GameContentManager.Game.Content.Load<SoundEffect>("death0");
            explosion.Play();

            //checking if the game is over (one player remaining)
            var remoteP = (from rp in GameContentManager.RemotePlayers
                           where GameContentManager.Sprites.Contains(rp)
                           select rp).ToList();
            if (remoteP.Count <= 1)
            {
                if (remoteP.Count == 0 && GameContentManager.Sprites.Contains(GameContentManager.LocalPlayer)) //the winner is the LocalPlayer
                {
                    GameContentManager.EndGame(GameContentManager.LocalPlayer);
                    //send gameWinner notification to server...
                }
                else if (remoteP.Count == 1 && !GameContentManager.Sprites.Contains(GameContentManager.LocalPlayer)) // ther winner is a RemotePlayer
                {
                    GameContentManager.EndGame(remoteP.ElementAt(0));
                }
            }
        }

        #region Animations

        public void PlayMoveLeftAnim(float deltaTime)
        {
            if (CurrentAnimFrame < 6 || CurrentAnimFrame > 8)
            {
                CurrentAnimFrame = 6;
            }
            timer += deltaTime;
            if (timer > PlayerSpeed * animSpeedOffset)
            {
                CurrentAnimFrame++;
                timer = 0;
            }
            if (CurrentAnimFrame == 9)
            {
                CurrentAnimFrame = 6;
            }
        }

        public void PlayMoveRightAnim(float deltaTime)
        {
            if (CurrentAnimFrame < 3 || CurrentAnimFrame > 5)
            {
                CurrentAnimFrame = 3;
            }
            timer += deltaTime;
            if (timer > PlayerSpeed * animSpeedOffset)
            {
                CurrentAnimFrame++;
                timer = 0;
            }
            if (CurrentAnimFrame == 6)
            {
                CurrentAnimFrame = 3;
            }
        }

        public void PlayMoveUpAnim(float deltaTime)
        {
            if (CurrentAnimFrame < 9 || CurrentAnimFrame > 11)
            {
                CurrentAnimFrame = 9;
            }
            timer += deltaTime;
            if (timer > PlayerSpeed * animSpeedOffset)
            {
                CurrentAnimFrame++;
                timer = 0;
            }
            if (CurrentAnimFrame == 12)
            {
                CurrentAnimFrame = 9;
            }
        }

        public void PlayMoveDownAnim(float deltaTime)
        {
            if (CurrentAnimFrame < 0 || CurrentAnimFrame > 2)
            {
                CurrentAnimFrame = 0;
            }
            timer += deltaTime;
            if (timer > PlayerSpeed * animSpeedOffset)
            {
                CurrentAnimFrame++;
                timer = 0;
            }
            if (CurrentAnimFrame == 3)
            {
                CurrentAnimFrame = 0;
            }
        }

        public void PauseAnim()
        {
            if (CurrentAnimFrame > 0 && CurrentAnimFrame < 3)
            {
                CurrentAnimFrame = 0;
                return;
            }
            if (CurrentAnimFrame > 3 && CurrentAnimFrame < 6)
            {
                CurrentAnimFrame = 3;
                return;
            }
            if (CurrentAnimFrame > 6 && CurrentAnimFrame < 9)
            {
                CurrentAnimFrame = 6;
                return;
            }
            if (CurrentAnimFrame > 9 && CurrentAnimFrame < 12)
            {
                CurrentAnimFrame = 9;
                return;
            }
        }

        #endregion
    }
}
