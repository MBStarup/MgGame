using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    [Serializable]
    public class PokeMan
    {
        public int Id;
        public ElementEnum Element;
        public Move[] Moves;
        public int Exp;
        public int Lvl;
        public int Hp;
        public string Nickname;

        public void Attack(PokeMan enemy, Move move)
        {
            move.DoMove(this, enemy);
        }

        public void TakeDmg(int dmg)
        {
            Hp = Math.Clamp(Hp - dmg, 0, Int32.MaxValue);

            if (Hp == 0)
                this.Die();
        }

        private void Die()
        {
            throw new NotImplementedException("Your PokeMan died!");
        }
    }
}