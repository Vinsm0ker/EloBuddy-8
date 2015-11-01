using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

using Color = System.Drawing.Color;

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
        

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            

        }



        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Kindred") return;

            Chat.Print("Kindred Dude Loaded!",Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FEED!",Color.CornflowerBlue);
            KindredMenu.loadMenu();
            KindredItems.loadSpells();
            Game.OnTick += GameOnTick;
            Game.OnUpdate += OnGameUpdate;

            #region Skill

            Q = new Spell.Skillshot(SpellSlot.Q, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SData.CastRangeDisplayOverride, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);

            #endregion

            

            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += GameOnDraw;

            Gapcloser.OnGapcloser += AntiGapCloser;
            //Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            

        }
        private static void GameOnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();

            //KillSteal();
            RLogic();
            //Activator();


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
            
            if (KindredItems.ignite != null)
            {
                ignite();
            }
                    
            
        }


        public static void GameOnDraw(EventArgs args)
        {
            if (KindredMenu.nodraw()) return;




            if (!KindredMenu.onlyReady())
            {
                if (KindredMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (KindredMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (KindredMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (KindredMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }

            }
            else
            {
                if (!Q.IsOnCooldown && KindredMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!W.IsOnCooldown && KindredMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!E.IsOnCooldown && KindredMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!R.IsOnCooldown && KindredMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }

            }







        }



        public static void OnCombo()
        {

            var alvo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (alvo == null || !alvo.IsValid || alvo.IsDead || alvo.IsZombie || _Player.IsDead) return;


            if (Q.IsReady() && KindredMenu.kincombo["combo.Q"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                Q.Cast(Game.ActiveCursorPos);
            }

            if (W.State == SpellState.Ready && KindredMenu.kincombo["combo.W"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 700)
            {
                W.Cast();
            }

            if (E.IsReady() && KindredMenu.kincombo["combo.E"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                E.Cast(alvo);
            }
        }

        public static void OnLaneClear()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, W.Range).ToArray().Length;


            if (Q.IsReady() && KindredMenu.kinlcs["lc.MinionsQ"].Cast<Slider>().CurrentValue <= count  && KindredMenu.kinlcs["lc.Q"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(Game.ActiveCursorPos);
                return;
            }
            if (W.State == SpellState.Ready && KindredMenu.kinlcs["lc.W"].Cast<CheckBox>().CurrentValue && KindredMenu.kinlcs["lc.MinionsW"].Cast<Slider>().CurrentValue <= count)
            {
                W.Cast();
                return;
            }
            return;

        }

        public static void OnJungle()
        {
            Orbwalker.ForcedTarget = null;

            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.ServerPosition).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (source == null) return;
            

            if (Q.IsReady() && KindredMenu.kinlcs["jungle.Q"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < Q.Range + 500)
            {
                Q.Cast(Game.ActiveCursorPos);
                return;
                
            }
            if (W.State == SpellState.Ready && KindredMenu.kinlcs["jungle.W"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) <= W.Range)
            {
                W.Cast();
                return;
                
            }
            if (E.IsReady() && KindredMenu.kinlcs["jungle.E"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < E.Range)
            {
                E.Cast(source);
                return;
            }
            return;
        }

        public static void KillSteal()
        {
            var Target = TargetSelector.GetTarget(1500, DamageType.Physical);
            if (Target == null) return;
            var hp = Target.Health;
            var ap = _Player.FlatMagicDamageMod + _Player.BaseAbilityDamage;
            var ad = _Player.FlatMagicDamageMod + _Player.BaseAttackDamage;

            var useq = KindredMenu.ks["ksq"].Cast<CheckBox>().CurrentValue;
            //var usei = Ks["ksi"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useq && !Target.IsZombie && Target.Health < Damage.CalculateDamageOnUnit(_Player,Target,DamageType.Physical, 60 + ((Q.Level - 1) * 30) + ad/5))
            {
                Q.Cast(Target.Position);

            }
        }

        public static void RLogic()
        {
            var mHP = KindredMenu.kinr["rlogic.minhp"].Cast<Slider>().CurrentValue;
            var eHP = KindredMenu.kinr["rlogic.ehp"].Cast<Slider>().CurrentValue;
            foreach (var ally in EntityManager.Heroes.Allies.Where(o => o.HealthPercent < mHP && !o.IsRecalling() && !o.IsDead && !o.IsZombie && _Player.Distance(o.Position) < R.Range && !o.IsInShopRange()))
            {
                var enemy = EntityManager.Heroes.Enemies.Where(o => ally.Distance(o.Position) <= 800 && o.HealthPercent <= eHP).ToArray();
                if (enemy.Length != 0) return;
                

                if (KindredMenu.kinr["r" + ally.BaseSkinName].Cast<CheckBox>().CurrentValue && ally.CountEnemiesInRange(800) >= 1)
                {
                    R.Cast(ally);
                }

            }
        }
        public static void ignite()
        {
            if (!KindredMenu.ks["spell.Ignite.Use"].Cast<CheckBox>().CurrentValue) return;
            var autoIgnite = TargetSelector.GetTarget(KindredItems.ignite.Range, DamageType.True);
            if (autoIgnite != null  && KindredMenu.ks["spell.Ignite.Kill"].Cast<CheckBox>().CurrentValue)
            {
                if (autoIgnite.Health >= DamageLibrary.GetSpellDamage(Player.Instance, autoIgnite, KindredItems.ignite.Slot)) return;
                KindredItems.ignite.Cast(autoIgnite);
            }
            else if(autoIgnite != null && autoIgnite.HealthPercent <= KindredMenu.spellsHealignite())
            {
                KindredItems.ignite.Cast(autoIgnite);
            }
                
        }
        /*public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs spell)
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
        public static void Activator()
        {
            if (Item.HasItem(ItemId.Blade_of_the_Ruined_King))
            {
                var target = TargetSelector.GetTarget(500, DamageType.Physical);
                Item.UseItem(ItemId.Blade_of_the_Ruined_King, target);
            }


        }*/



    }
}


