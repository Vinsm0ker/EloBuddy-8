using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace L.U.L.U
{
    class Program
    {

        public const string ChampionName = "Lulu";

        public static Menu Menu, Combo, Harass, LaneClear, KS, Rm, AntiGapCloser;

        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        public static Spell.Targeted Ignite;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static void OnLoad(EventArgs args)
        {
            if (_Player.ChampionName != ChampionName) return;

            Q = new Spell.Skillshot(SpellSlot.Q, (uint)_Player.Spellbook.GetSpell(SpellSlot.Q).SData.CastRangeDisplayOverride, SkillShotType.Linear);
            W = new Spell.Targeted(SpellSlot.W, (uint)_Player.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
            E = new Spell.Targeted(SpellSlot.E, (uint)_Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Targeted(SpellSlot.R, (uint)_Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);
            Ignite = new Spell.Targeted(_Player.GetSpellSlotFromName("summonerdot"), 600);

            Menu = MainMenu.AddMenu("L.U.L.U.", "lulu");
            Combo = Menu.AddSubMenu("Combo Menu");
            Combo.AddGroupLabel("L.U.L.U. Combo Menu");
            Combo.AddSeparator();
            Combo.Add("comboq", new CheckBox("Use Q", true));
            Combo.Add("combow", new CheckBox("Use W", true));
            Combo.Add("comboe", new CheckBox("Use E", true));

            Harass = Menu.AddSubMenu("Harass Menu");
            Harass.AddGroupLabel("L.U.L.U. Harass Menu");
            Harass.AddSeparator();
            Harass.Add("harassq", new CheckBox("Use Q", true));
            Harass.Add("harasse", new CheckBox("Use E", true));
            Harass.AddSeparator();
            Harass.AddGroupLabel("Mana Manager");
            Harass.Add("harassmana", new Slider("Min. Mana: {0}%", 30));

            LaneClear = Menu.AddSubMenu("LaneClear Menu");
            LaneClear.AddGroupLabel("L.U.L.U. LaneClear Menu");
            LaneClear.AddSeparator();
            LaneClear.Add("laneq", new CheckBox("Use Q", true));
            LaneClear.AddSeparator();
            LaneClear.AddGroupLabel("Mana Manager");
            LaneClear.Add("lanemana", new Slider("Min. Mana: {0}%", 30));



            Game.OnTick += keys;
        }

        public static void keys(EventArgs args)
        {
            try
            {
                switch (Orbwalker.ActiveModesFlags)
                {
                    case Orbwalker.ActiveModes.Combo:
                        Modes.OnCombo();
                        break;

                    case Orbwalker.ActiveModes.Harass:
                        Modes.OnHarass();
                        break;

                    case Orbwalker.ActiveModes.LaneClear:
                        break;

                }
            }


            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message.ToString());
            }
        }
    }
}
