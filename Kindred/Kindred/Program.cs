using EloBuddy;
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


        
        public static Menu Kindred, Combo, Ks, LaneClear, Draw, Rmenu, Jungle, Misc;
        public static string version = "1.0";

        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        public static Spell.Targeted Ignite;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += GameOnDraw;
            Game.OnTick += GameOnTick;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }



        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Kindred") return;

            Chat.Print("Kindred Dude Loaded!", Color.CornflowerBlue);

            Hacks.RenderWatermark = false;

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
            Ks.Add("ksi", new CheckBox("Ks with Ignite [WIP]", true));

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
            Rmenu.Add("ehp", new Slider("Max enemy hp to use R", 10, 0, 100));
            foreach (var ally in ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsAlly && !o.IsStructure() && !o.IsMinion && _Player.CanCast))
            {
                if (ally.BaseSkinName == "KindredWolf") return;
                Rmenu.Add("r" + ally.BaseSkinName, new CheckBox("R on " + ally.BaseSkinName, true));
            }
            Rmenu.AddSeparator();
            Rmenu.AddGroupLabel("Protector Skill Whitelist");
            Rmenu.Add("dprotector", new CheckBox("Disable Protector", false));
            Rmenu.Add("hpprotector", new Slider("Min. Hp Protector", 30, 0, 100));
            foreach (var enemy in ObjectManager.Get<Obj_AI_Base>().Where(o=> o.IsEnemy))
            {
                foreach(var skillshot in SpellDB.SpellDatabase.Spells.Where(x => x.charName == enemy.BaseSkinName))
                {
                    Rmenu.Add("champ." + skillshot.spellName, new CheckBox(skillshot.spellName + "|" + skillshot.charName, true));
                }
            }


            Misc = Kindred.AddSubMenu("Misc Menu", "kinmisc");
            Misc.AddGroupLabel("AntiGap Closer -BETA");
            Misc.Add("antigp", new CheckBox("AntiGap Closer With Q", true));

                







            #endregion


        }
        private static void GameOnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();

            KillSteal();
            RLogic();


        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            
            if(gapcloser.End.Distance(_Player.ServerPosition) <= 300)
            {
                Q.Cast(gapcloser.End.Extend(_Player.Position,_Player.Distance(gapcloser.End) + Q.Range).To3D());
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {

            
            



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
            if (alvo == null || !alvo.IsValid || alvo.IsDead || alvo.IsZombie || _Player.IsDead) return;

            if (Q.IsReady() && Combo["useq"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                Q.Cast(Game.ActiveCursorPos);
            }

            if (W.State == SpellState.Ready && Combo["usew"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 700)
            {
                W.Cast();
            }

            if (E.IsReady() && Combo["usee"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                E.Cast(alvo);
            }
        }

        public static void OnLaneClear()
        {
            var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,_Player.Position,540).Count();
            var q = LaneClear["qlane"].Cast<CheckBox>().CurrentValue;
            var w = LaneClear["wlane"].Cast<CheckBox>().CurrentValue;
            var mp = LaneClear["mana"].Cast<Slider>().CurrentValue;
            var min = LaneClear["min"].Cast<Slider>().CurrentValue;
            var wmin = LaneClear["wmin"].Cast<Slider>().CurrentValue;


            if (_Player.ManaPercent < mp) return;

            if (q && Q.IsReady() && Minions >= min)
            {
                Q.Cast(Game.CursorPos);
            }

            if (w && W.State == SpellState.Ready && Minions >= wmin)
            {
                W.Cast();
            }

        }

        public static void OnJungle()
        {
            var q = Jungle["qjungle"].Cast<CheckBox>().CurrentValue;
            var w = Jungle["wjungle"].Cast<CheckBox>().CurrentValue;
            var e = Jungle["ejungle"].Cast<CheckBox>().CurrentValue;
            var mana = Jungle["manaj"].Cast<Slider>().CurrentValue;
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.Position, W.Range).ToArray();



            if (monster.Length == 0)
            {
                return;
            }

            if (_Player.ManaPercent >= mana)
            {
                if (Q.IsReady() && q )
                {
                    Q.Cast(Game.CursorPos);
                }
                if (W.State == SpellState.Ready && w) 
                {
                    W.Cast();
                }
                if (E.IsReady() && e)
                {
                    E.Cast(monster[0]);
                }
            }



        }

        public static void KillSteal()
        {
            var Target = TargetSelector.GetTarget(1500, DamageType.Physical);
            if (Target == null) return;
            var hp = Target.Health;
            var ap = _Player.FlatMagicDamageMod + _Player.BaseAbilityDamage;
            var ad = _Player.FlatMagicDamageMod + _Player.BaseAttackDamage;

            var useq = Ks["ksq"].Cast<CheckBox>().CurrentValue;
            var usei = Ks["ksi"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useq && !Target.IsZombie && Target.Health < Damage.CalculateDamageOnUnit(_Player,Target,DamageType.Physical, 60 + ((Q.Level - 1) * 30) + ad/5))
            {
                Q.Cast(Target.Position);

            }
        }

        public static void RLogic()
        {
            var mHP = Rmenu["minhp"].Cast<Slider>().CurrentValue;
            var eHP = Rmenu["ehp"].Cast<Slider>().CurrentValue;

            foreach (var ally in EntityManager.Heroes.Allies.Where(o => o.HealthPercent < mHP && !o.IsRecalling() && !o.IsDead && !o.IsZombie && _Player.Distance(o.Position) < R.Range && !o.IsInShopRange()))
            {
                var enemy = EntityManager.Heroes.Enemies.Where(o => ally.Distance(o.Position) <= 800 && o.HealthPercent <= eHP).ToArray();
                if (enemy.Length != 0) return;
                

                if (Rmenu["r" + ally.BaseSkinName].Cast<CheckBox>().CurrentValue && ally.CountEnemiesInRange(800) >= 1)
                {
                    R.Cast(ally);
                }

            }
        }
        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs spell)
        {
            var hp = Rmenu["hpprotector"].Cast<Slider>().CurrentValue;
            if (!R.IsReady() && _Player.IsDead && _Player.IsZombie && sender.IsAlly && !sender.IsMe && !Rmenu["dprotector"].Cast<CheckBox>().CurrentValue) return;

            if (sender is Obj_AI_Base && R.IsReady() && sender.IsEnemy && spell.SData.ConsideredAsAutoAttack == 1 && !sender.IsDead && !sender.IsZombie && sender.IsValidTarget(1000))
            {
                foreach(var protector in SpellDB.SpellDatabase.Spells.Where(x=> x.spellName == spell.SData.Name && Rmenu["champ." + x.spellName].Cast<CheckBox>().CurrentValue))
                {
                    if(protector.spellType == SpellDB.SpellType.Circular && _Player.Distance(spell.End) <= 200 && _Player.HealthPercent <= hp )
                    {
                        R.Cast(_Player);
                    }
                    if (protector.spellType == SpellDB.SpellType.Cone && _Player.Distance(spell.End) <= 200 && _Player.HealthPercent <= hp)
                    {
                        R.Cast(_Player);
                    }
                    if (protector.spellType == SpellDB.SpellType.Line && _Player.Distance(spell.End) <= 200 && _Player.HealthPercent <= hp)
                    {
                        R.Cast(_Player);
                    }
                }
            

            
            }

        }
        


    }
}

