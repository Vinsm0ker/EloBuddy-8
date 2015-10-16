﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindred
{
    public static class Program
    {

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Menu Kindred, Combo, Ks, LaneClear, Draw, Rmenu, Jungle;
        public static string version = "1.0";

        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Game.OnUpdate += GameOnUpdate;
            Game.OnTick += GameOnTick;
            Drawing.OnDraw += GameOnDraw;
        }



        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Kindred") return;

            Chat.Print("Kindred Dude Loaded!", Color.CornflowerBlue);


            #region Skill

            Q = new Spell.Skillshot(SpellSlot.Q, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SData.CastRangeDisplayOverride, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);

            #endregion




            #region 

            Kindred = MainMenu.AddMenu("Kindred Dude", "kindude");
            Kindred.AddGroupLabel("Kindred Dude - " + version);
            Kindred.AddLabel("Made by -Koka");

            Draw = Kindred.AddSubMenu("Draw Menu", "kindra");
            Draw.AddGroupLabel("Draw Settings");
            Draw.Add("nodraw", new CheckBox("No Display Drawing", false));
            Draw.Add("onlyR", new CheckBox("Display only Ready", true));
            Draw.AddSeparator();
            Draw.Add("drawq", new CheckBox("Draw Q Range", true));
            Draw.Add("drawW", new CheckBox("Draw W Range", true));
            Draw.Add("drawE", new CheckBox("Draw E Range", true));
            Draw.Add("drawr", new CheckBox("Draw R Range", true));

            Combo = Kindred.AddSubMenu("Combo Menu", "kincombo");
            Combo.AddGroupLabel("Combo");
            Combo.Add("useq", new CheckBox("Use Q in Combo", true));
            Combo.Add("usew", new CheckBox("Use W in Combo", true));
            Combo.Add("usee", new CheckBox("Use E in Combo", true));

            Ks = Kindred.AddSubMenu("KillSteal Menu", "kinks");
            Ks.AddGroupLabel("Kill Steal");
            Ks.Add("ksq", new CheckBox("Ks with Q", true));
            Ks.Add("ksi", new CheckBox("Ks with Ignite", true));

            LaneClear = Kindred.AddSubMenu("LaneClear Settings", "kinlcs");
            LaneClear.AddGroupLabel("LaneClear Settings");
            LaneClear.Add("qlane", new CheckBox("Use Q in LaneClear", true));
            LaneClear.Add("wlane", new CheckBox("Use W in LaneClear", true));
            LaneClear.Add("qcount", new Slider("Min. Minions to Q", 3, 1, 3));
            LaneClear.Add("wcount", new Slider("Min. Minions to W", 5, 1, 20));
            LaneClear.Add("mana", new Slider("Min. Mana %", 30, 0, 100));
            LaneClear.Add("min", new Slider("Min. Minions to Q", 2, 0, 10));
            LaneClear.Add("wmin", new Slider("Min. Minions to W", 2, 0, 10));

            

            Jungle = Kindred.AddSubMenu("Jungle Settings", "kinjc");
            Jungle.AddGroupLabel("Jungle Settings");
            Jungle.Add("qjungle", new CheckBox("Use Q in Jungle", true));
            Jungle.Add("wjungle", new CheckBox("Use W in Jungle", true));
            Jungle.Add("ejungle", new CheckBox("Use E in Jungle", true));
            Jungle.Add("manaj", new Slider("Min. Mana %", 30, 0, 100));

            



            Rmenu = Kindred.AddSubMenu("R Menu", "kinr");
            Rmenu.AddGroupLabel("R Menu");
            Rmenu.Add("minhp", new Slider("Min. HP to use R", 30, 0, 100));
            foreach (var ally in ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsAlly && !o.IsStructure() && !o.IsMinion && _Player.CanCast))
            {
                if (ally.BaseSkinName == "KindredWolf") return;
                Rmenu.Add("r" + ally.BaseSkinName, new CheckBox("R on " + ally.BaseSkinName, true));
            }

            




            #endregion


        }
        private static void GameOnTick(EventArgs args)
        {
            KillSteal();

        }


        public static void GameOnDraw(EventArgs args)
        {
            if (Draw["nodraw"].Cast<CheckBox>().CurrentValue) return;

            if (!Draw["onlyR"].Cast<CheckBox>().CurrentValue)
            {
                if (Draw["drawq"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (Draw["draww"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (Draw["drawe"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (Draw["drawr"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }

            }
            else
            {
                if (!Q.IsOnCooldown && Draw["drawq"].Cast<CheckBox>().CurrentValue)
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!W.IsOnCooldown && Draw["draww"].Cast<CheckBox>().CurrentValue)
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!E.IsOnCooldown && Draw["drawe"].Cast<CheckBox>().CurrentValue)
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!R.IsOnCooldown && Draw["drawr"].Cast<CheckBox>().CurrentValue)
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }

            }







        }


        public static void OnCombo()
        {
            var alvo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (!alvo.IsValid || alvo.IsDead || alvo.IsZombie || _Player.IsDead) return;

            if (!Q.IsOnCooldown && Combo["useq"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 800)
            {
                Q.Cast(Game.CursorPos);
            }

            if (!W.IsOnCooldown && Combo["usew"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 700)
            {
                W.Cast();
            }

            if (!E.IsOnCooldown && Combo["usee"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                E.Cast(alvo);
            }
        }

        public static void OnLaneClear()
        {
            var Minions = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy).Count();
            var q = LaneClear["qlane"].Cast<CheckBox>().CurrentValue;
            var w = LaneClear["wlane"].Cast<CheckBox>().CurrentValue;
            var mp = LaneClear["mana"].Cast<Slider>().CurrentValue;
            var min = LaneClear["min"].Cast<Slider>().CurrentValue;
            var wmin = LaneClear["min"].Cast<Slider>().CurrentValue;

            if (_Player.ManaPercent < mp) return;

            if (q && Q.IsReady() && Minions >= min)
            {
                Q.Cast(Game.CursorPos);
            }

            if (w && W.IsReady() && Minions >= wmin)
            {
                W.Cast(Game.CursorPos);
            }

        }

        public static void OnJungle()
        {
            var q = Jungle["qjungle"].Cast<CheckBox>().CurrentValue;
            var w = Jungle["wjungle"].Cast<CheckBox>().CurrentValue;
            var e = Jungle["ejungle"].Cast<CheckBox>().CurrentValue;
            var mana = Jungle["manaj"].Cast<Slider>().CurrentValue;
            var monster = ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.Health).FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(E.Range));



            if (monster == null) return;

            if (_Player.ManaPercent > mana)
            {
                if (Q.IsReady() && q)
                {
                    Q.Cast(Game.CursorPos);
                }
                if (W.IsReady() && w)
                {
                    W.Cast();
                }
                if (E.IsReady() && e)
                {
                    E.Cast(monster);
                }
            }



        }

        public static void KillSteal()
        {
            var Target = TargetSelector.GetTarget(500, DamageType.Physical);
            if (Target == null) return;
            var hp = Target.Health;
            var ap = _Player.FlatMagicDamageMod + _Player.BaseAbilityDamage;
            var ad = _Player.FlatMagicDamageMod + _Player.BaseAttackDamage;

            var useq = Ks["ksq"].Cast<CheckBox>().CurrentValue;
            var usei = Ks["ksi"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useq && !Target.IsZombie && Target.Health < _Player.GetSpellDamage(Target, SpellSlot.Q))
            {
                Q.Cast(Target.Position);

            }
        }

        public static void RLogic()
        {
            var mHP = Rmenu["minhp"].Cast<Slider>().CurrentValue;

            foreach (var ally in EntityManager.Heroes.Allies.Where(o => o.HealthPercent < mHP && !o.IsRecalling() && !o.IsDead && !o.IsZombie && _Player.Distance(o.Position) < R.Range && !o.IsInShopRange()))
            {
                if (Rmenu["r" + ally.BaseSkinName].Cast<CheckBox>().CurrentValue && _Player.CountEnemiesInRange(1500) > 1 && ally.CountEnemiesInRange(1500) > 1)
                {
                    R.Cast(ally);
                }

            }
        }
        private static void GameOnUpdate(EventArgs args)
        {

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;

                case Orbwalker.ActiveModes.LaneClear:
                    OnLaneClear();
                    break;

                case Orbwalker.ActiveModes.JungleClear:
                    OnJungle();
                    break;
            }

        }



    }
}

