using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan.Scenes
{
    class Item
    {

        public Item healthPotion;
        public Item pokeball;
        public Item attackPotion;

        public PokeMan FriendlyPokeMan { get; }

        public Item (Item item)
        {
            FriendlyPokeMan = Area.p.party[0];
        }

        protected void healthPotionEffect()
        {
            // TODO Heal da pokeman
            FriendlyPokeMan.hp += 10;


        }

        protected void pokeballEffect()
        {
            // TODO Catch da other pokeman
        }

        protected void attackPotionEffect()
        {
            // TODO Increase attack power of da pokeman
        }
    }
}
