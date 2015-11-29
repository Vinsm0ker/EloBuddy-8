using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace Kindred
{
    internal static class KindredMenu
    {
        private static Menu kindum;
        public static Menu kincombo, kindraw, kinr, itemsMenu,smitePage,spellsPage, kinlcs, miscmenu;

        public static void loadMenu()
        {
            kindudepage();
            kindrawpage();
            kincombopage();       
            kinlcspage();
            kinrpage();
            Activator();
            miscpage();
            

        }


        private static void kindudepage()
        {
            kindum = MainMenu.AddMenu("Kindred Dude", "main");
            kindum.AddGroupLabel("About this script");
            kindum.AddLabel(" Kindred Dude - " + Program.version);
            kindum.AddLabel(" Made by -Koka and -iRaxe");
            kindum.AddSeparator();
            kindum.AddGroupLabel("Hotkeys");
            kindum.AddLabel(" - Use SpaceBar for Combo");
            kindum.AddLabel(" - Use the key V For LaneClear/JungleClear");

        }

        private static void kindrawpage()
        {
            kindraw = kindum.AddSubMenu("Draw  settings", "Draw");
            kindraw.AddGroupLabel("Draw Settings");
            kindraw.Add("nodraw", new CheckBox("No Display Drawing", false));
            kindraw.Add("onlyReady", new CheckBox("Display only Ready"));
            kindraw.AddSeparator();
            kindraw.Add("draw.Q", new CheckBox("Draw Q Range"));
            kindraw.Add("draw.W", new CheckBox("Draw W Range"));
            kindraw.Add("draw.E", new CheckBox("Draw E Range"));
            kindraw.Add("draw.R", new CheckBox("Draw R Range"));
            kindraw.AddSeparator();
            kindraw.AddGroupLabel("Combo Damage Draw");
            kindraw.Add("draw.combo.q", new CheckBox("Use Q Damage to Calculate"));
            kindraw.Add("draw.combo.e", new CheckBox("Use E Damage to Calculate"));
            kindraw.Add("draw.combo.aa", new Slider("Use {0} AA to Calculate",2,1,5));
            kindraw.AddSeparator();
            kindraw.AddGroupLabel("Pro Tips");
            kindraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific spell draw");
        }

        private static void kincombopage()
        {
            kincombo = kindum.AddSubMenu("Combo settings", "Combo");
            kincombo.AddGroupLabel("Combo settings");
            kincombo.Add("combo.Q", new CheckBox("Use Q Spell"));
            kincombo.Add("combo.W", new CheckBox("Use W Spell"));
            kincombo.Add("combo.E", new CheckBox("Use E Spell"));
            kincombo.Add("combo.R", new CheckBox("Use R Spell"));
            kincombo.Add("combo.Smite", new CheckBox("Use Smite"));
            kincombo.Add("combo.Botrk", new CheckBox("Use Blade of the ruined king"));
            kincombo.Add("combo.Youmus", new CheckBox("Use Youmuss"));
            kincombo.Add("combo.Bilgewater", new CheckBox("Use Bilgewater Cutlass"));
            kincombo.AddSeparator();
            kincombo.AddGroupLabel("Pro Tips");
            kincombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");

        }

        private static void kinlcspage()
        {
            kinlcs = kindum.AddSubMenu("Lane Clear Settings", "laneclear");
            kinlcs.AddGroupLabel("Lane clear settings");
            kinlcs.Add("lc.Q", new CheckBox("Use Q Spell",false));
            kinlcs.Add("lc.W", new CheckBox("Use W Spell",false));
            kinlcs.Add("lc.Mana", new Slider("Min. Mana%",30));
            kinlcs.Add("lc.MinionsQ", new Slider("Min. Minions for Q", 3,0,3));
            kinlcs.Add("lc.MinionsW", new Slider("Min. Minions for W ", 3,0,10));
            kinlcs.AddSeparator();
            kinlcs.AddGroupLabel("Jungle Settings");
            kinlcs.Add("jungle.Q", new CheckBox("Use Q Spell in Jungle"));
            kinlcs.Add("jungle.W", new CheckBox("Use E Spell in Jungle"));
            kinlcs.Add("jungle.E", new CheckBox("Use W Spell in Jungle "));
            kinlcs.AddSeparator();
            kinlcs.AddGroupLabel("Pro Tips");
            kinlcs.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");

        }

        private static void kinrpage()
        {
            kinr = kindum.AddSubMenu("Ultimate Menu", "rlogic");
            kinr.AddGroupLabel("Lamb's Respite Menu");
            kinr.AddSeparator();
            kinr.Add("rlogic.minhp", new Slider("Min. HP to use Lamb's Respite", 30, 0, 100));
            kinr.Add("rlogic.ehp", new Slider("Max enemy hp to use Lamb's Respite", 10, 0, 100));
            kinr.AddSeparator();
            
            foreach (var ally in ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsAlly && !o.IsStructure() && !o.IsMinion && Program._Player.CanCast))
            {
                if (ally.BaseSkinName == "kindredwolf" || ally.BaseSkinName == "KindredWolf") return;
                kinr.Add("r" + ally.BaseSkinName, new CheckBox("R on " + ally.BaseSkinName, true));
            }
            kinr.AddGroupLabel("Pro Tips");
            kinr.AddLabel(" -Remember to play safe and don't be a teemo");
        }

        private static void Activator()
        {
            itemsMenu = kindum.AddSubMenu("Items Settings", "Items");
            itemsMenu.AddGroupLabel("Items usage");
            itemsMenu.AddSeparator();
            itemsMenu.AddGroupLabel("Youmuss");
            itemsMenu.Add("items.Youmuss.HP", new Slider("Use Youmuss if hp is lower than {0}(%)", 60, 1, 100));
            itemsMenu.Add("items.Youmuss.Enemys", new Slider("Use Youmuss if {0} enemys in range", 2, 1, 5));
            itemsMenu.AddSeparator();
            itemsMenu.AddGroupLabel("Blade Of The Ruined King");
            itemsMenu.Add("items.Botrk.HP", new Slider("Use Botrk if hp is lower than {0}(%)", 30, 1, 100));
            itemsMenu.AddGroupLabel("Bilgewater Cutlass");
            itemsMenu.Add("items.bilgewater.HP", new Slider("Use Bilgewater cutlass if hp is lower than {0}(%)", 30, 1, 100));
            smitePage = kindum.AddSubMenu("Smite Settings", "Smite");
            smitePage.AddGroupLabel("Smite settings");
            smitePage.AddSeparator();
            smitePage.Add("SRU_Red", new CheckBox("Smite Red Buff"));
            smitePage.Add("SRU_Blue", new CheckBox("Smite Blue Buff"));
            smitePage.Add("SRU_Dragon", new CheckBox("Smite Dragon"));
            smitePage.Add("SRU_Baron", new CheckBox("Smite Baron"));
            smitePage.Add("SRU_Gromp", new CheckBox("Smite Gromp"));
            smitePage.Add("SRU_Murkwolf", new CheckBox("Smite Wolf"));
            smitePage.Add("SRU_Razorbeak", new CheckBox("Smite Bird"));
            smitePage.Add("SRU_Krug", new CheckBox("Smite Golem"));
            smitePage.Add("Sru_Crab", new CheckBox("Smite Crab"));
            smitePage.AddSeparator();
            spellsPage = kindum.AddSubMenu("Spells Settings");
            spellsPage.AddGroupLabel("Spells settings");
            spellsPage.AddSeparator();
            spellsPage.AddGroupLabel("Heal settings");
            spellsPage.Add("spells.Heal.Hp", new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            spellsPage.AddGroupLabel("Ignite settings");
            spellsPage.Add("spell.Ignite.Use", new CheckBox("Use Ignite for KillSteal"));
            spellsPage.Add("spells.Ignite.Focus", new Slider("Use Ignire when target HP isl lower than {0}(%)", 10, 1, 100));
            spellsPage.Add("spells.Ignite.Kill", new CheckBox("Use ignite if killable"));

        }

        private static void miscpage()
        {
            miscmenu = kindum.AddSubMenu("Misc Menu", "othermenu");
            miscmenu.AddGroupLabel("Misc Menu");
            miscmenu.Add("lvlup", new CheckBox("Auto Level Up Spells", false));
            miscmenu.AddSeparator();
            miscmenu.AddGroupLabel("Skin settings");
            miscmenu.Add("skin.Id", new Slider("Skin Editor", 0, 0, 1));
            miscmenu.AddSeparator();
            miscmenu.AddGroupLabel("Flee settings");
            miscmenu.Add("flee.Smart.Q", new CheckBox("Smart Flee"));
            miscmenu.AddSeparator();
            miscmenu.AddGroupLabel("Safety Settings");
            miscmenu.Add("flee.Min.Mana", new Slider("Min mana for using Q Spell", 30, 1, 100));
            miscmenu.Add("combo.Qmin", new Slider("Play safe when Hp (%) is lower than", 60, 1, 100));
            miscmenu.Add("combo.QminAG", new Slider("Play safe when enemys in range", 3, 1, 5));
        }
        public static float itemsbilgewaterHp()
        {
            return itemsMenu["items.bilgewater.HP"].Cast<Slider>().CurrentValue;
        }
        public static float minQaggresive()
        {
            return miscmenu["combo.QminAG"].Cast<Slider>().CurrentValue;
        }
        public static float minQcombo()
        {
            return miscmenu["combo.Qmin"].Cast<Slider>().CurrentValue;
        }

        public static bool fleeSmart()
        {
            return miscmenu["flee.Smart.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static float MinmanaFlee()
        {
            return miscmenu["flee.Min.Mana"].Cast<Slider>().CurrentValue;
        }
        public static int skinId()
        {
            return miscmenu["skin.Id"].Cast<Slider>().CurrentValue;
        }
            

        //
        public static float spellsHealignite()
        {
            return spellsPage["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        public static bool spellsIgniteOnlyHpLow()
        {
            return spellsPage["spells.Ignite.Kill"].Cast<CheckBox>().CurrentValue;
        }
        public static bool spellsUseIgnite()
        {
            return spellsPage["spells.Ignite.Use"].Cast<CheckBox>().CurrentValue;
        }


        public static float itemsYOUMUShp()
        {
            return itemsMenu["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }
        public static float itemsYOUMUSenemys()
        {
            return itemsMenu["items.Youmuss.Enemys"].Cast<Slider>().CurrentValue;
        }
        public static float itemsBOTRKhp()
        {
            return itemsMenu["items.Botrk.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool useBilgewater()
        {
            return kincombo["combo.Bilgewater"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useBotrk()
        {
            return kincombo["combo.Botrk"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useYoumuss()
        {
            return kincombo["combo.Youmus"].Cast<CheckBox>().CurrentValue;
        }
        public static float spellsHealhp()
        {
            return spellsPage["spells.Heal.Hp"].Cast<Slider>().CurrentValue;
        }



        public static int rLogicMinHp()
        {
            return kinr["rlogic.minhp"].Cast<Slider>().CurrentValue;
        }
        public static int rLogicEnemyMinHp()
        {
            return kinr["rlogic.ehp"].Cast<Slider>().CurrentValue;
        }
        public static bool useWjungle()
        {
            return kinlcs["jungle.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useEjungle()
        {
            return kinlcs["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useQjungle()
        {
            return kinlcs["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useQ()
        {
            return kinlcs["combo.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useW()
        {
            return kinlcs["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useE()
        {
            return kinlcs["combo.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool nodraw()
        {
            return kindraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }
        public static bool onlyReady()
        {
            return kindraw["onlyready"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsQ()
        {
            return kindraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsW()
        {
            return kindraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsE()
        {
            return kindraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsR()
        {
            return kindraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool useQlc()
        {
            return kinlcs["lc.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useWlc()
        {
            return kinlcs["lc.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useElc()
        {
            return kinlcs["lc.E"].Cast<CheckBox>().CurrentValue;
        }
    }
}


