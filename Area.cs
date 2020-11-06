//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;

//namespace PokeMan
//{
//    internal class Area : Scene
//    {
//        public byte[,,] Tiles = new byte[100, 100, 10];
//        private Texture2D[] Textures;
//        public int SpriteSize { get; private set; }
//        private SpriteFont font;

//        public Area(string xmlPath)
//        {
//            Content.RootDirectory = "Content";
//            font = Content.Load<SpriteFont>("Assets/FontTextBox");
//            LoadContent(xmlPath);
//        }

//        public async void LoadContent(string xmlPath)
//        {
//            Loads tile sprites based off xml doc
//            XmlDocument doc = new XmlDocument();
//            doc.Load("../../../Content/Xml/Areas/" + xmlPath);
//            var node = doc.DocumentElement.SelectSingleNode("/Tiles");
//            var rootPath = node.Attributes["rootPath"].Value;
//            SpriteSize = int.Parse(node.Attributes["size"].Value);

//            int count = node.ChildNodes.Count;
//            Textures = new Texture2D[count + 1];
//            string[] paths = new string[count];

//            int i = 0;
//            foreach (XmlNode tile in node)
//            {
//                paths[i++] = ($"{rootPath}{tile.InnerText}");
//            }
//            IEnumerable<Texture2D> a = await LoadAssets<Texture2D>(paths);
//            Textures = a.ToArray();
//        }

//        public override void Draw(SpriteBatch spriteBatch, Camera camera)
//        {
//            if (this.LoadAmount < 1)
//            {
//                spriteBatch.DrawString(font, "Loading Assets: " + LoadAmount + "%", Vector2.Zero, Color.Black);
//            }
//            else
//            {
//                for (int y = 0; y < Tiles.GetLength(1); y++)
//                {
//                    for (int x = 0; x < Tiles.GetLength(0); x++)
//                    {
//                        for (int z = 0; z < Tiles.GetLength(2); z++)
//                        {
//                            if (Textures[Tiles[x, y, z]] != null)
//                                spriteBatch.Draw(Textures[Tiles[x, y, z]], camera.WorldToScreen(GridToWorld((x, y))), Color.White);
//                        }
//                    }
//                }
//            }

//            base.Draw(spriteBatch, camera);
//        }

//        public (int x, int y) WorldToGrid(Vector2 worldCoords)
//        {
//            return ((int)Math.Round(worldCoords.X / SpriteSize), (int)Math.Round(worldCoords.Y / SpriteSize));
//        }

//        public Vector2 GridToWorld((int x, int y) gridCoords)
//        {
//            return new Vector2(gridCoords.x * SpriteSize - SpriteSize / 2, gridCoords.y * SpriteSize - SpriteSize / 2);
//        }
//    }
//}