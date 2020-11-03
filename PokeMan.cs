using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    [Serializable]
    public abstract class PokeMan
    {
        public int id;
        protected Random rnd;




        private ElementEnum element;
        public Move[] moves;
        public int exp;
        public int lvl;
        public int hp;
        public string nickname;

        private int maxHp;
        private int attack;
        private int defence;
        private int speed;

        private Texture2D spriteFront;
        private Texture2D spriteBack;

        //protected int baseHp;
        //protected int baseAttack;
        //protected int baseDefence;
        //protected int baseSpeed;

        protected int[] baseStats; // Hp, Attack, Defence, Speed
        protected float[][] nature;

        public ElementEnum Element { get => element; protected set => element = value; }


        public int MaxHpStat { get => maxHp; protected set => maxHp = value; }
        public int AttackStat { get => attack; protected set => attack = value; }
        public int DefenceStat { get => defence; protected set => defence = value; }
        public int SpeedStat { get => speed; protected set => speed = value; }
        public Texture2D SpriteFront { get => spriteFront; protected set => spriteFront = value; }
        public Texture2D SpriteBack { get => spriteBack; protected set => spriteBack = value; }

        public void Attack(PokeMan enemy, Move move)
        {
            move.DoMove(this, enemy);
        }


        public void TakeDmg(int dmg)
        {
            hp = Math.Clamp(hp - dmg, 0, Int32.MaxValue);

            if (hp == 0)
                this.Die();
        }

        private void Die()
        {
            throw new NotImplementedException("Your PokeMan died!");
        }

        public void LevelUp()
        {
            lvl++;
            CalculateStats();

        }


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
        protected void CalculateStats()
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
        protected void LoadSprite(ContentManager content,string pokeman)
        {
            SpriteFront = content.Load<Texture2D>("Assets/Characters/Pokeman/" + pokeman + "/"+pokeman + ".png");
            SpriteBack = content.Load<Texture2D>("Assets/Characters/Pokeman/" + pokeman + "/" + pokeman + "Back.png");
        }



    }
}