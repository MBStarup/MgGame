using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
   
    class Leaflutter : PokeMan
    {

        public Leaflutter(string nickname, int level)
        {

            LoadSprite("Leaflutter");
            rnd = new Random();
            base.nickname = nickname;
            lvl = level;



            //baseHp=50;
            //baseAttack = 10;
            //baseDefence = 10;
            //baseSpeed = 10;

            baseStats = new int[4] { 50, 10, 10, 10 };
            DetermineNature();
            CalculateStats();





        }

    }
}
