using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Media;
using BomberContracts_WCF;

namespace Bomberman
{
    public class GameContentManager
    {
        //public PlayerSprite testRemotePlayer = new PlayerSprite();

        public GameContentManager(string localPlayer, List<string> allPlayers, BomberGame game)
        {
            Game = game;
            isGameEnded = false;
            Sprites = new List<Sprite>();
            RemotePlayers = new List<PlayerSprite>();
            LocalPlayer = new PlayerSprite();
            LocalPlayer.GameContentManager = this;
            LocalPlayer.PlayerID = localPlayer;
            int localPlayerNumber = allPlayers.IndexOf(localPlayer);
            LocalPlayer.PlayerIndex = localPlayerNumber;
            LocalPlayer.Controller = new PlayerController();
            LocalPlayer.Controller.Player = LocalPlayer;
            Sprites.Add(LocalPlayer);
            foreach (string s in allPlayers)
            {
                if (s != localPlayer)
                {
                    PlayerSprite player = new PlayerSprite();
                    player.GameContentManager = this;
                    player.PlayerID = s;
                    int playerNumber = allPlayers.IndexOf(s);
                    player.PlayerIndex = playerNumber;
                    Sprites.Add(player);
                    RemotePlayers.Add(player);
                }
            }
        }

        /// <summary>
        /// channel to server
        /// </summary>
        public IBomberService Server { get; set; }

        /// <summary>
        /// Reference to the managed Game instance.
        /// </summary>
        public BomberGame Game { get; set; }

        /// <summary>
        /// List of the Sprites currently present in the map.
        /// </summary>
        public List<Sprite> Sprites { get; set; }

        /// <summary>
        /// remote players of this game.
        /// </summary>
        public List<PlayerSprite> RemotePlayers { get; set; }

        /// <summary>
        /// Local player.
        /// </summary>
        public PlayerSprite LocalPlayer { get; set; }

        public bool isGameEnded { get; private set; }
        bool isEndMusicLaunched = false;
        PlayerSprite winner;

        SpriteFont bomberFont;
        Vector2 FontPos;
        float FontRotation;

        /// <summary>
        /// Initializes the Sprites.
        /// </summary>
        public void Initialize()
        {
            var screenWidth = Game.graphics.PreferredBackBufferWidth;
            var screenHeight = Game.graphics.PreferredBackBufferHeight;
            int unitDimension = 40;
            Song mainTheme = Game.Content.Load<Song>("mainTheme");
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume /= 3;
            //SoundEffect.MasterVolume /= 6;
            MediaPlayer.Play(mainTheme);
            GenerateBorders();
            if (!GenerateMapFromFile("Map.txt"))
            {
                GenerateMap("22222222222222222222222222222222222222222222222222001");
            }
            if (LocalPlayer.PlayerIndex == 0)
            {
                LocalPlayer.Position = new Vector2(unitDimension, unitDimension);
            }
            else if (LocalPlayer.PlayerIndex == 1)
            {
                LocalPlayer.Position = new Vector2(screenWidth - 2 * unitDimension, unitDimension);
            }
            else if (LocalPlayer.PlayerIndex == 2)
            {
                LocalPlayer.Position = new Vector2(screenWidth - 2 * unitDimension, screenHeight - 2 * unitDimension);
            }
            else
            {
                LocalPlayer.Position = new Vector2(unitDimension, screenHeight - 2 * unitDimension);
            }
            for (int i = 0; i < RemotePlayers.Count; i++)
            {
                PlayerSprite player = RemotePlayers.ElementAt(i);
                if (player.PlayerIndex == 0)
                {
                    player.Position = new Vector2(unitDimension, unitDimension);
                    player.NextPosition = new Vector2(unitDimension, unitDimension);
                }
                else if (player.PlayerIndex == 1)
                {
                    player.Position = new Vector2(screenWidth - 2 * unitDimension, unitDimension);
                    player.NextPosition = new Vector2(screenWidth - 2 * unitDimension, unitDimension);
                }
                else if (player.PlayerIndex == 2)
                {
                    player.Position = new Vector2(screenWidth - 2 * unitDimension, screenHeight - 2 * unitDimension);
                    player.NextPosition = new Vector2(screenWidth - 2 * unitDimension, screenHeight - 2 * unitDimension);
                }
                else
                {
                    player.Position = new Vector2(unitDimension, screenHeight - 2 * unitDimension);
                    player.NextPosition = new Vector2(unitDimension, screenHeight - 2 * unitDimension);
                }
                player.Controller = new RemotePlayerController();
                player.Controller.Player = player;
            }
        }

        /// <summary>
        /// Loads content for each Sprite contained in the GameContentManager.
        /// </summary>
        public void LoadContent()
        {
            foreach (Sprite s in Sprites)
            {
                s.LoadContent(Game.Content);
            }
            bomberFont = Game.Content.Load<SpriteFont>("BomberFont");
            FontPos = new Vector2(Game.graphics.GraphicsDevice.Viewport.Width / 2,
            Game.graphics.GraphicsDevice.Viewport.Height / 2);
            FontRotation = 0;
        }

        /// <summary>
        /// Performs updates to each sprite contained in the GameContentManager.
        /// </summary>
        /// <param name="gameTime">Reference to the game time.</param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                Sprites.ElementAt(i).Update(gameTime);
            }
        }

        public void EndGame(PlayerSprite winner)
        {
            this.winner = winner;
            isGameEnded = true;
        }

        /// <summary>
        /// Draws the sprites contained in the GameContentManager on the screen.
        /// </summary>
        /// <param name="theSpriteBatch">SpriteBatch object used to draw the graphics.</param>
        public void Draw(SpriteBatch theSpriteBatch)
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                Sprites.ElementAt(i).Draw(theSpriteBatch);
            }
            if (isGameEnded)
            {
                //draw winner's name...
                string output = "Winner : " + winner.PlayerID;
                // Find the center of the string
                Vector2 FontOrigin = bomberFont.MeasureString(output) / 2;
                // Draw the string with the same color as the winner
                Color color = Color.White;
                if (winner.PlayerIndex == 1)
                    color = Color.Black;
                else if (winner.PlayerIndex == 3)
                    color = Color.Blue;
                else if (winner.PlayerIndex == 2)
                    color = Color.Red;
                theSpriteBatch.DrawString(bomberFont, output, FontPos, color,
                    FontRotation, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                //switching to end game music
                if (!isEndMusicLaunched)
                {
                    Song endTheme = Game.Content.Load<Song>("endGame");
                    MediaPlayer.IsRepeating = false;
                    //MediaPlayer.Volume /= 3;
                    //SoundEffect.MasterVolume /= 6;
                    MediaPlayer.Play(endTheme);
                    isEndMusicLaunched = true;
                }
            }
        }

        /// <summary>
        /// places a bomb in the game map
        /// </summary>
        /// <param name="Position">position where the bomb will be placed</param>
        public void SpawnBomb(Vector2 Position)
        {
            BombSprite bomb = new BombSprite();
            bomb.LoadContent(Game.Content);
            bomb.Position = Position;
            Sprites.Insert(0, bomb);
            bomb.GameContentManager = this;
            bomb.CanCollide = true;
            SoundEffect dropBomb = Game.Content.Load<SoundEffect>("dropBomb");
            dropBomb.CreateInstance().Play();
        }

        /// <summary>
        /// generates borders to limit the map
        /// </summary>
        private void GenerateBorders()
        {
            var screenWidth = Game.graphics.PreferredBackBufferWidth;
            var screenHeight = Game.graphics.PreferredBackBufferHeight;
            int unitDimension = 40;
            int horizontolBorderSpritesNumber = (int)(screenWidth / unitDimension);
            int verticalBorderSpritesNumber = (int)(screenHeight / unitDimension);
            Vector2 newPosition = Vector2.Zero;
            for (int i = 0; i < horizontolBorderSpritesNumber; i++)
            {
                //top border building
                Sprite wall = new Sprite("wall");
                wall.Scale = 0.078125f;
                wall.IsDestroyable = false;
                wall.Position = newPosition;
                Sprites.Add(wall);
                wall.GameContentManager = this;
                wall.DrawColor = Color.DarkGray;
                //bottom border building
                wall = new Sprite("wall");
                wall.Scale = 0.078125f;
                wall.IsDestroyable = false;
                wall.Position = newPosition;
                wall.Position += new Vector2(0, screenHeight - unitDimension);
                Sprites.Add(wall);
                wall.GameContentManager = this;
                wall.DrawColor = Color.DarkGray;
                //updating build position
                newPosition.X += unitDimension;
            }
            newPosition = Vector2.Zero;
            for (int i = 0; i < verticalBorderSpritesNumber - 2; i++)
            {
                newPosition.Y += unitDimension;
                //left border
                Sprite wall = new Sprite("wall");
                wall.Scale = wall.Scale = 0.078125f;
                wall.IsDestroyable = false;
                wall.Position = newPosition;
                Sprites.Add(wall);
                wall.GameContentManager = this;
                wall.DrawColor = Color.DarkGray;
                //right border
                wall = new Sprite("wall");
                wall.Scale = wall.Scale = 0.078125f;
                wall.IsDestroyable = false;
                wall.Position = newPosition;
                wall.Position += new Vector2(screenWidth - unitDimension, 0);
                Sprites.Add(wall);
                wall.GameContentManager = this;
                wall.DrawColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// Generates a map using a coded description:
        /// 0: the next avaible space in the map will contain a destroyable object
        /// 1: the next avaible space will contain an undestroyable object
        /// Anything else: the next avaible space will be empty.
        /// The map will be filled from left to right and form top to bottom.
        /// </summary>
        /// <param name="mapCode">contains the generation code (e.g: "100221ff5... etc")</param>
        private void GenerateMap(string mapCode)
        {
            var screenWidth = Game.graphics.PreferredBackBufferWidth;
            var screenHeight = Game.graphics.PreferredBackBufferHeight;
            int unitDimension = 40;
            Vector2 nextPosition = new Vector2(unitDimension, unitDimension);
            char[] codes = new char[mapCode.Length];
            int currentCodePos = 0;
            using (StringReader sr = new StringReader(mapCode))
            {
                sr.Read(codes, 0, mapCode.Length);
            }
            while (nextPosition.Y <= screenHeight - 2 * unitDimension && currentCodePos < mapCode.Length)
            {
                int nextCode = (int)char.GetNumericValue(codes[currentCodePos]);
                if (nextCode == 0)
                {
                    Sprite wall = new Sprite("wall");
                    wall.Scale = wall.Scale = 0.078125f;
                    wall.Position = nextPosition;
                    Sprites.Add(wall);
                    wall.GameContentManager = this;
                }
                else if (nextCode == 1)
                {
                    Sprite wall = new Sprite("wall");
                    wall.Scale = wall.Scale = 0.078125f;
                    wall.Position = nextPosition;
                    wall.IsDestroyable = false;
                    Sprites.Add(wall);
                    wall.GameContentManager = this;
                    wall.DrawColor = Color.DarkGray;
                }
                nextPosition += new Vector2(unitDimension, 0);
                if (nextPosition.X > screenWidth - 2 * unitDimension)
                {
                    nextPosition += new Vector2(-nextPosition.X + unitDimension, unitDimension);
                }
                currentCodePos++;
            }
        }

        /// <summary>
        /// Generates the map from the code contained in a file using GenerateMap(string mapCode) method.
        /// Returns false if the file couldn't be read.
        /// </summary>
        /// <param name="fileName">file relative path</param>
        private bool GenerateMapFromFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line = sr.ReadToEnd();
                    GenerateMap(line);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        #region Network Callback operations

        public void UpdatePlayerPosition(string PlayerID, int PlayerPositionX, int PlayerPositionY)
        {
            foreach (PlayerSprite p in RemotePlayers)
            {
                if (p.PlayerID == PlayerID)
                {
                    p.NextPosition = new Vector2(PlayerPositionX, PlayerPositionY);
                }
            }
        }

        public void SpawnBomb(int BombPositionX, int BombPositionY)
        {
            SpawnBomb(new Vector2(BombPositionX, BombPositionY));
        }

        public void KillPlayer(string PlayerID)
        {
            foreach (PlayerSprite p in RemotePlayers)
            {
                if (p.PlayerID == PlayerID)
                {
                    p.KillPlayer();
                }
            }
        }

        #endregion
    }
}
