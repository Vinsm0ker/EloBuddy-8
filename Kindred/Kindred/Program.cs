using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;

namespace Kindred
{
    public static class Program
    {

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static string version = "2.0.0";
        private static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        private static int[] AbilitySequence;
        private static Spell.Skillshot Q;
        private static Spell.Active W;
        private static Spell.Targeted E;
        private static Spell.Targeted R;

        static void Main()
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            

        }



        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Kindred") return;
            AbilitySequence = new int[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };

            Chat.Print("Kindred Dude Loaded!",Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FEED!",Color.Red);
            KindredMenu.loadMenu();
            KindredActivator.loadSpells();
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
        }
        private static void GameOnTick(EventArgs args)
        {
            if (KindredMenu.miscmenu["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            RLogic();
            Flee();


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
            if (KindredActivator.smite != null)
                smite();
            if (KindredActivator.heal != null)
                Heal();
            if (KindredActivator.ignite != null)
                ignite();
        Player.SetSkinId(KindredMenu.skinId());
        }


        private static void GameOnDraw(EventArgs args)
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

        private static void LevelUpSpells()
        {
            int qL = _Player.Spellbook.GetSpell(SpellSlot.Q).Level + qOff;
            int wL = _Player.Spellbook.GetSpell(SpellSlot.W).Level + wOff;
            int eL = _Player.Spellbook.GetSpell(SpellSlot.E).Level + eOff;
            int rL = _Player.Spellbook.GetSpell(SpellSlot.R).Level + rOff;
            if (qL + wL + eL + rL >= ObjectManager.Player.Level) return;
            int[] level = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < ObjectManager.Player.Level; i++)
            {
                level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
            }
            if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
            if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
            if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
        }

        private static void OnCombo()
        {
            Target = TargetSelector.GetTarget(700, DamageType.Magical);

            if (Target == null) return;
            if (!Target.IsValidTarget()) return;
            var alvo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (alvo == null || !alvo.IsValid || alvo.IsDead || alvo.IsZombie || _Player.IsDead) return;


            if (E.IsReady() && KindredMenu.kincombo["combo.E"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 500)
            {
                E.Cast(alvo);

            }

            if (W.State == SpellState.Ready && KindredMenu.kincombo["combo.W"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= 700)
            {
                W.Cast();

            }

            if (Q.IsReady() && KindredMenu.kincombo["combo.Q"].Cast<CheckBox>().CurrentValue && alvo.Distance(_Player) <= Q.Range + 500)
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);

            }


            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >= KindredMenu.itemsYOUMUSenemys() || Player.Instance.HealthPercent >= KindredMenu.itemsYOUMUShp()) && KindredActivator.youmus.IsReady() && KindredMenu.kincombo["combo.Youmuss"].Cast<CheckBox>().CurrentValue && KindredActivator.youmus.IsOwned())
            {
                KindredActivator.youmus.Cast();
                return;
            }
            if (Player.Instance.HealthPercent <= KindredMenu.itemsbilgewaterHp() && KindredMenu.kincombo["combo.Bilgewater"].Cast<CheckBox>().CurrentValue && KindredActivator.bilgewater.IsReady() && KindredActivator.bilgewater.IsOwned())
            {
                KindredActivator.bilgewater.Cast(Target);
                return;
            }

            if (Player.Instance.HealthPercent <= KindredMenu.itemsBOTRKhp() && KindredMenu.kincombo["combo.Botrk"].Cast<CheckBox>().CurrentValue && KindredActivator.botrk.IsReady() && KindredActivator.botrk.IsOwned())
            {
                KindredActivator.botrk.Cast(Target);
            }
        }


        private static void OnLaneClear()
        {
            
            Orbwalker.ForcedTarget = null;
            var count = EntityManager.MinionsAndMonsters.EnemyMinions.Count(o => _Player.Distance(o.Position) <= W.Range);
            var source = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _Player.ServerPosition, W.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;

            if (Q.IsReady() && KindredMenu.kinlcs["lc.MinionsQ"].Cast<Slider>().CurrentValue <= count  && KindredMenu.kinlcs["lc.Q"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) <= Q.Range)
            {
                Q.Cast(Game.ActiveCursorPos);
                return;
            }
            if (W.IsReady() && KindredMenu.kinlcs["lc.W"].Cast<CheckBox>().CurrentValue && KindredMenu.kinlcs["lc.MinionsW"].Cast<Slider>().CurrentValue <= count && source.Distance(_Player) <= W.Range)
            {
                W.Cast();
            }
        }

        private static void OnJungle()
        {
            Orbwalker.ForcedTarget = null;

            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.ServerPosition).OrderByDescending(a=> a.MaxHealth).FirstOrDefault(a => a.Distance(_Player) <= _Player.GetAutoAttackRange());

            if (source == null) return;
            

            if (Q.IsReady() && KindredMenu.kinlcs["jungle.Q"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < Q.Range + 500)
            {
                Q.Cast(Game.CursorPos);
                return;
                
            }
            if (W.IsReady() && KindredMenu.kinlcs["jungle.W"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) <= W.Range)
            {
                W.Cast();
                return;
                
            }
            if (!E.IsReady() || !KindredMenu.kinlcs["jungle.E"].Cast<CheckBox>().CurrentValue ||
                !(source.Distance(_Player) < E.Range)) return;
            E.Cast(source);
        }

        private static void RLogic()
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

        private static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(KindredActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.Instance.GetSpellDamage(autoIgnite, KindredActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= KindredMenu.spellsHealignite())
                KindredActivator.ignite.Cast(autoIgnite);

        }

        private static void Heal()
        {
            if (KindredActivator.heal.IsReady() && Player.Instance.HealthPercent <= KindredMenu.spellsHealhp())
                KindredActivator.heal.Cast();
        }

        private static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => KindredMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player.Instance, a, DamageLibrary.SummonerSpells.Smite) >= a.Health && KindredMenu.smitePage[a.BaseSkinName].Cast<CheckBox>().CurrentValue && KindredActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && KindredActivator.smite.IsReady())
                KindredActivator.smite.Cast(unit);
        }

        private static void Flee()
        {
            var target = TargetSelector.GetTarget(ObjectManager.Player.AttackRange + Q.Range, DamageType.Physical);
            if (target != null)
            {
                if (!Q.IsReady() || !KindredMenu.fleeSmart() ||
                    !(KindredMenu.MinmanaFlee() >= Player.Instance.ManaPercent)) return;
                if (ObjectManager.Player.Distance(target.Position) <= ObjectManager.Player.GetAutoAttackRange() && Player.Instance.HealthPercent <= KindredMenu.minQcombo() || ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >= KindredMenu.minQaggresive())
                    Player.CastSpell(SpellSlot.Q, -1 * target.Position);
                else if (ObjectManager.Player.Distance(target.Position) >= (ObjectManager.Player.GetAutoAttackRange() + Q.Range))
                    Player.CastSpell(SpellSlot.Q, target.Position);
                else
                    Player.CastSpell(SpellSlot.Q, Game.ActiveCursorPos);
            }
            else
            {
                if (Q.IsReady() && KindredMenu.MinmanaFlee() >= Player.Instance.ManaPercent)
                    Player.CastSpell(SpellSlot.Q, Game.CursorPos);
            }
        }
    }
}



