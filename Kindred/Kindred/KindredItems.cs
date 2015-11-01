using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindred
{
    class KindredItems
    {
        public static Spell.Targeted ignite;
        public static void loadSpells()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner1, 580);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner2, 580);
        }
    }
}

//In the program.cs we have to add this on GameUpdate function
/*public static void gameUpdate(EventArgs args)
        {
            if (KindredItems.ignite != null)
            k.ignite();
        }*/
//In the program.cs we have to add also this 
  
//In the Kindredmenu.cs we have to add this to the killsteal 



      
