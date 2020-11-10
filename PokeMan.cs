using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PokeMan
{
    [Serializable]
    public class PokeMan
    {
        public int id;
        protected Random rnd = new Random();

        public SpriteAnimation Sprite;

        private ElementEnum element;
        public Move[] moves;
        public int exp;
        public int lvl;
        public int hp;
        public string nickname;
        public int tookdmg;

        private int maxHp;
        private int attack;
        private int defence;
        private int speed;

        private Texture2D spriteFront;
        private Texture2D spriteBack;

        public static List<PokeMan> playerPokemen = new List<PokeMan>(4);

        protected int[] baseStats; // Hp, Attack, Defence, Speed
        protected float[][] nature;

        public ElementEnum Element { get => element; protected set => element = value; }

        public int MaxHpStat { get => maxHp; protected set => maxHp = value; }
        public int AttackStat { get => attack; protected set => attack = value; }
        public int DefenceStat { get => defence; protected set => defence = value; }
        public int SpeedStat { get => speed; protected set => speed = value; }
        public Texture2D SpriteFront { get => spriteFront; protected set => spriteFront = value; }
        public Texture2D SpriteBack { get => spriteBack; protected set => spriteBack = value; }

        private XmlDocument doc;
        private XmlDocument movepoolDoc;

        public PokeMan()
        {
        }

        public PokeMan(int id, int level)
        {
            this.id = id;
            lvl = level;
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            var node = doc.DocumentElement.SelectSingleNode("/PokeMans");
            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == this.id);

            nickname = node.Attributes["name"].Value;
            element = (ElementEnum)Enum.Parse(typeof(ElementEnum), node.Attributes["element"].Value);

            baseStats = new int[4];

            baseStats[0] = int.Parse(node.Attributes["baseHp"].Value);
            baseStats[1] = int.Parse(node.Attributes["baseAttack"].Value);
            baseStats[2] = int.Parse(node.Attributes["baseDefense"].Value);
            baseStats[3] = int.Parse(node.Attributes["baseSpeed"].Value);

            moves = new Move[4];

            movepoolDoc = new XmlDocument();
            movepoolDoc.Load("../../../Content/Xml/PocketMan.xml");
            



            GenerateStats();
        }

        public void Load()
        {
           
        }

        public void Attack(PokeMan enemy, Move move)
        {
            move.DoMove(this, enemy);
        }
        public void TakeDmg(int dmg)
        {
            tookdmg = Math.Clamp(dmg, 0, Int32.MaxValue);
            hp = Math.Clamp(hp - dmg, 0, Int32.MaxValue);
            if (hp == 0)
                this.Die();
        }

        private void Die()
        {
            //throw new NotImplementedException("Your PokeMan died!");
            PokeManGame.Scenes.Pop();
        }

        public void LevelUp()
        {
            lvl++;
            UpdateStats();
            UpdateMoveset();
        }

        /// <summary>
        /// Generates a Random Nature, that will effect statgrowth
        /// if both the positive and negative are the same stat it should cancel eachother out leving us with a neutral nature
        /// </summary>
        protected void DetermineNature()
        {
            nature = new float[4][];
            for (int i = 0; i < 4; i++)
            {
                nature[i] = new float[2];
                nature[i][0] = 0;
                nature[i][1] = 1;
            }

            // Determine positive nature
            int pos = rnd.Next(0, 4);
            nature[pos][0] += 1;
            nature[pos][1] *= 2;

            // Determine negative nature
            int neg = rnd.Next(0, 4);
            nature[neg][0] -= 1;
            nature[neg][1] *= 0.5f;
        }

        /// <summary>
        /// Updates the Pokemans Current Stats Based on Base stats, Level and Nature
        /// </summary>
        public void UpdateStats()
        {
            float[] stats = new float[4];
            for (int i = 0; i < 4; i++)
            {
                stats[i] = (baseStats[i] + nature[i][0]) + (nature[i][1] * lvl);
            }

            MaxHpStat = (int)Math.Floor(stats[0]);
            AttackStat = (int)Math.Floor(stats[1]);
            DefenceStat = (int)Math.Floor(stats[2]);
            SpeedStat = (int)Math.Floor(stats[3]);
        }
        public void UpdateMoveset()
        {
            var movepoolNode = movepoolDoc.DocumentElement.SelectSingleNode("/PokeMans");
            movepoolNode = movepoolNode.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == this.id).SelectSingleNode("Moves");

            int slot = 0; // To place the move the correct place

            foreach(XmlNode m in movepoolNode)
            {
                int moveLevel = int.Parse(m.Attributes["lvl"].Value);
                if (moveLevel <= lvl)
                {
                    moves[slot] = new Move(int.Parse(m.Attributes["id"].Value));
                    slot++;
                }
                else// if it is not the previous level it will not me the next levels either
                    break;
                
            }


        }

        /// <summary>
        /// Generates Nature and updates stats, and Udates Moveset
        /// Only for use when first generating the pokemon
        /// </summary>
        protected void GenerateStats()
        {
            DetermineNature();
            UpdateStats();
            UpdateMoveset();

            hp = MaxHpStat;
        }
    }
}