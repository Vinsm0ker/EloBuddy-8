using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindred
{
    class KindredMenu
    {
        public static Menu kindum, kincombo, kindraw, kinr, ks, kinlcs;
        public static void loadMenu()
        {
            kindudepage();
            kindrawpage();
            kincombopage();       
            //kinjcpage();
            kinlcspage();
            //kindrapage();
            //kinkspage();
            kinrpage();

        }

      

        public static void kindudepage()
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

        public static void kindrawpage()
        {
            kindraw = kindum.AddSubMenu("Draw  settings", "Draw");
            kindraw.AddGroupLabel("Draw Settings");
            kindraw.Add("nodraw", new CheckBox("No Display Drawing", false));
            kindraw.Add("onlyReady", new CheckBox("Display only Ready", true));
            kindraw.AddSeparator();
            kindraw.Add("draw.Q", new CheckBox("Draw Q Range", true));
            kindraw.Add("draw.W", new CheckBox("Draw W Range", true));
            kindraw.Add("draw.E", new CheckBox("Draw E Range", true));
            kindraw.Add("draw.R", new CheckBox("Draw R Range", true));
            kindraw.AddSeparator();
            kindraw.AddGroupLabel("Pro Tips");
            kindraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific spell draw");
        }

        public static void kincombopage()
        {
            kincombo = kindum.AddSubMenu("Combo settings", "Combo");
            kincombo.AddGroupLabel("Combo settings");
            kincombo.Add("combo.Q", new CheckBox("Use Q"));
            kincombo.Add("combo.W", new CheckBox("Use W"));
            kincombo.Add("combo.E", new CheckBox("Use E"));
            kincombo.Add("combo.R", new CheckBox("Use R"));
            kincombo.AddSeparator();
            kincombo.AddGroupLabel("Pro Tips");
            kincombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");

        }
        public static void kinlcspage()
        {
            kinlcs = kindum.AddSubMenu("Lane Clear Settings", "laneclear");
            kinlcs.AddGroupLabel("Lane clear settings");
            kinlcs.Add("lc.Q", new CheckBox("Use Q"));
            kinlcs.Add("lc.W", new CheckBox("Use W"));
            kinlcs.Add("lc.Mana", new Slider("Min. Mana%",30));
            kinlcs.Add("lc.MinionsQ", new Slider("Min. Mana%", 3,0,3));
            kinlcs.Add("lc.MinionsW", new Slider("Min. Mana%", 3,0,10));
            kinlcs.AddSeparator();
            kinlcs.AddGroupLabel("Jungle Settings");
            kinlcs.Add("jungle.Q", new CheckBox("Use Q jungle"));
            kinlcs.Add("jungle.W", new CheckBox("Use E jungle"));
            kinlcs.Add("jungle.E", new CheckBox("Use W jungle "));
            kinlcs.AddSeparator();
            kinlcs.AddGroupLabel("Pro Tips");
            kinlcs.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");

        }
        public static void kinrpage()
        {
            kinr = kindum.AddSubMenu("Ultimate Menu", "rlogic");
            kinr.AddGroupLabel("Lamb's Respite Menu");
            kinr.AddSeparator();
            kinr.Add("rlogic.minhp", new Slider("Min. HP to use R", 30, 0, 100));
            kinr.Add("rlogic.ehp", new Slider("Max enemy hp to use R", 10, 0, 100));
            kinr.AddSeparator();
            
            foreach (var ally in ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsAlly && !o.IsStructure() && !o.IsMinion && Program._Player.CanCast))
            {
                if (ally.BaseSkinName == "kindredwolf" || ally.BaseSkinName == "KindredWolf") return;
                kinr.Add("r" + ally.BaseSkinName, new CheckBox("R on " + ally.BaseSkinName, true));
            }
        }
        public void kinkspage()
        {
            ks = kindum.AddSubMenu("KillSteal settings", "ks");
            ks.AddGroupLabel("KillSteal settings");
            ks.AddSeparator();
            ks.Add("ksq", new CheckBox("Ks using Q"));
            ks.Add("ksi", new CheckBox("Ks using Ignite[WIP]"));
        }



        public static int rLogicMinHp()
        {
            return kinr["rlogic.minhp"].Cast<Slider>().CurrentValue;
        }
        public static int rLogicEnemyMinHp()
        {
            return kinr["rlogic.ehp"].Cast<Slider>().CurrentValue;
        }
        public static bool ksQ()
        {
            return ks["ksq"].Cast<CheckBox>().CurrentValue;
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

