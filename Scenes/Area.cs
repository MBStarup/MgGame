using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using EnumBuilder;

namespace PokeMan
{
    internal class Area : Scene
    {
        new public float LoadAmount { get => (float)completedLoadTasks / loadTasks + base.LoadAmount / loadTasks; }

        private int completedLoadTasks;
        private int loadTasks = 1; //loading the player, the rest is added later

        public byte[,,] Tiles;
        private Texture2D[][] Palettes;
        public int SpriteSize { get; private set; }
        private SpriteFont font;

        private Player p;
        private Camera cam;
        private KeyboardState lastState;

        public Area(PokeManGame game, string xmlPath) : base(game)
        {
            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            cam = new Camera(game.Graphics);
            LoadContent(xmlPath);
        }

        public override void Update()
        {
            var playerPos = p.Position;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right))
            {
                playerPos += new Vector2(SpriteSize, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left))
            {
                playerPos -= new Vector2(SpriteSize, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
            {
                playerPos -= new Vector2(0, SpriteSize);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
            {
                playerPos += new Vector2(0, SpriteSize);
            }
            lastState = Keyboard.GetState();

            var i = WorldToGrid(playerPos);

            try
            {
                if (Tiles[i.x, i.y, 0] != 1)
                {
                    p.Position = playerPos;
                }
            }
            catch (IndexOutOfRangeException) { } //Don't move the player if we outside the array

            cam.position = p.Position;

            base.Update();
        }

        public async void LoadContent(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/Areas/" + xmlPath);
            var node = doc.DocumentElement.SelectSingleNode("/Tiles");
            SpriteSize = int.Parse(node.Attributes["size"].Value);

            //loads map from file
            FileStream fileStream = File.OpenRead("../../../Content/Maps/" + node.Attributes["file"].Value);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data);
            fileStream.Close();
            Tiles = data.Deserialize<Byte[,,]>();

            p = new Player(SpriteSize);

            void PlacePlayer() //local function so we can use retrun to break out of both loops
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x < Tiles.GetLength(0); x++)
                    {
                        if (Tiles[x, y, 0] == 2) //The first occurence of the value 2 in the effect layer of the map is where we spawn the player, not good with areas where you'd come from both sides but it's prototype stuff so it's fiiiiinneeeee
                        {
                            p.Position = GridToWorld((x, y));
                            return;
                        }
                    }
                }
            }

            PlacePlayer();

            p.LoadAssets(this.Content, "MainChar.xml");

            Palettes = new Texture2D[node.ChildNodes.Count][];
            var nodes = node.SelectNodes("Palette");
            loadTasks += nodes.Count;

            int i = 0;
            foreach (XmlNode n in nodes)
            {
                var rootPath = n.Attributes["path"].Value;
                nodes = n.SelectNodes("Tile");
                int count = n.ChildNodes.Count;
                string[] paths = new string[count];

                int j = 0;
                foreach (XmlNode tile in nodes)
                {
                    paths[j++] = ($"{rootPath}{tile.InnerText}");
                }

                IEnumerable<Texture2D> a = await LoadAssets<Texture2D>(paths);
                a = a.Prepend(null); //I use null for no texture, this should correspond to the byte value 0 in the bytearray, so i place it at index 0 of all palettes

                Palettes[i++] = a.ToArray();
                completedLoadTasks += 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + LoadAmount + "%", Vector2.Zero, Color.Black);
            }
            else
            {
                for (int z = 1; z < Tiles.GetLength(2); z++)
                {
                    for (int y = 0; y < Tiles.GetLength(1); y++)
                    {
                        for (int x = 0; x < Tiles.GetLength(0); x++)
                        {
                            if (Palettes[z][Tiles[x, y, z]] != null)
                                spriteBatch.Draw(Palettes[z][Tiles[x, y, z]], cam.WorldToScreen(GridToWorld((x, y))), Color.White);

                            if (x == 0 && y == 0 && z == Tiles.GetLength(2) - 1) //Before the last layer we draw the player, so that player can stand behind some foreground sprites
                                p.Draw(spriteBatch);
                        }
                    }
                }
            }
        }

        public (int x, int y) WorldToGrid(Vector2 worldCoords)
        {
            return ((int)Math.Round(worldCoords.X / SpriteSize, MidpointRounding.AwayFromZero), (int)Math.Round(worldCoords.Y / SpriteSize, MidpointRounding.AwayFromZero));
        }

        public Vector2 GridToWorld((int x, int y) gridCoords)
        {
            return new Vector2((gridCoords.x) * SpriteSize - SpriteSize / 2, (gridCoords.y) * SpriteSize - SpriteSize / 2);
        }

        public bool WillColide(int x, int y)
        {
            return Tiles[x, y, 0] == 1;
        }
    }
}