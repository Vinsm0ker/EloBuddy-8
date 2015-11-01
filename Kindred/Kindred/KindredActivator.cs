//Here will posted the use of heal/smite
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
    class KindredActivator
    {
        public static Spell.Skillshot privQ;
        public static Spell.Targeted smite;
        public static Item youmus,botrk;
        public static Spell.Active heal;
        public void loadSpells()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("smite"))
                smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("smite"))
                smite = new Spell.Targeted(SpellSlot.Summoner2, 570);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner1, 580);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner2, 580);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner1);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner2);
            youmus = new Item(3142);
            botrk = new Item(3153, 500);
        }
    }
}
//To add on OnGameUpdate
public static void gameUpdate(EventArgs args)
        {
            if(KindredActivator.smite != null)
            smite();
            if(KindredActivator.heal != null)
            Heal();
            if (KindredActivator.ignite != null)
            ignite();
//To add in the Program
 public void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(KindredActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player.Instance, autoIgnite, KindredActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= KindredMenu.spellsHealignite())
                KindredActivator.ignite.Cast(autoIgnite);
        }
        public void Heal()
        {
            if (KindredActivator.heal.IsReady() && Player.Instance.HealthPercent <= KindredMenu.spellsHealhp())
                KindredActivator.heal.Cast();
        }
        public void smite()
        {
          var unit =ObjectManager.Get<Obj_AI_Base>().Where(a =>KindredMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player.Instance,a,DamageLibrary.SummonerSpells.Smite) >= a.Health  &&KindredMenu.smitePage[a.BaseSkinName].Cast<CheckBox>().CurrentValue && KindredActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
          if (unit != null && KindredActivator.smite.IsReady())
              KindredActivator.smite.Cast(unit);
        }
//To add in Combo
if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >= KindredMenu.itemsYOUMUSSenemys() || Player.Instance.HealthPercent >= KindredMenu.itemsYOUMUSShp()) && KindredActivator.youmus.IsReady())
                KindredActivator.youmus.Cast();
if (Player.Instance.HealthPercent <= KindredMenu.itemsBOTRKhp() && KindredActivator.botrk.IsReady())
                KindredActivator.botrk.Cast(target);
//To add in KindredMenu
//In ComboPage
           kindum.Add("combo.Smite", new CheckBox("Use Smite"));
           kindum.Add("combo.Botrk", new CheckBox("Use Botrk"));
           kindum.Add("combo.Youmus", new CheckBox("Use Youmuss"));
//Make a new page to the menu and declare the var 
        public void Activator()
        {
           itemsMenu = myMenu.AddSubMenu("Items Settings", "Items");
            itemsMenu.AddGroupLabel("Items usage");
            itemsMenu.AddSeparator();
            itemsMenu.AddGroupLabel("Youmuss");
            itemsMenu.Add("items.Youmuss.HP", new Slider("Use Youmuss if hp is lower than (%)", 60, 1, 100));
            itemsMenu.Add("items.Youmuss.Enemys", new Slider("Use Youmuss if enemys in range", 2, 1, 5));
            itemsMenu.AddSeparator();
            itemsMenu.AddGroupLabel("Botrk");
            itemsMenu.Add("items.Botrk.HP", new Slider("Use Botrk if hp is lower than (%)", 30, 1, 100));
            itemsMenu.AddSeparator();
            smitePage.AddGroupLabel("Smite settings");
            smitePage.AddSeparator();
            smitePage.Add("SRU_Red", new CheckBox("Smite Red"));
            smitePage.Add("SRU_Blue", new CheckBox("Smite Blue"));
            smitePage.Add("SRU_Dragon", new CheckBox("Smite Dragon"));
            smitePage.Add("SRU_Baron", new CheckBox("Smite Baron"));
            smitePage.Add("SRU_Gromp", new CheckBox("Smite Gromp"));
            smitePage.Add("SRU_Murkwolf", new CheckBox("Smite Wolf"));
            smitePage.Add("SRU_Razorbeak", new CheckBox("Smite Bird"));
            smitePage.Add("SRU_Krug", new CheckBox("Smite Golem"));
            smitePage.Add("Sru_Crab",new CheckBox("Smite Crab"));
            smitePage.AddSeparator();
            spellsPage.AddGroupLabel("Spells settings");
            spellsPage.AddSeparator();
            spellsPage.AddGroupLabel("Heal settings");
            spellsPage.Add("spells.Heal.Hp", new Slider("Use Heal when HP is lower than (%)", 30, 1, 100));
            spellsPage.AddGroupLabel("Ignite settings");
            spellsPage.Add("spells.Ignite.Focus", new Slider("Use Ignire when target HP isl lower than (%)", 10, 1, 100));
            spellsPage.Add("spells.Ignite.Kill", new CheckBox("Use ignite if killable"));
        }
        public static float itemsYOUMUSShp()
        {
            return kitems["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }
        public static float itemsYOUMUSSenemys()
        {
            return kindum["items.Youmuss.Enemys"].Cast<Slider>().CurrentValue;
        }
        public static float itemsBOTRKhp()
        {
            return kindum["items.Botrk.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool useBotrk()
        {
            return kindum["combo.Botrk"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useYoumuss()
        {
            return kindum["combo.Youmus"].Cast<CheckBox>().CurrentValue;
        }
        public static float spellsHealhp() 
        {
            return kspells["spells.Heal.Hp"].Cast<Slider>().CurrentValue;
        }
        public static float spellsHealignite()
        {
            return kspells["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }

