using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace L.U.L.U
{
    class Modes
    {

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(1000, DamageType.Magical);
            if (Target == null) return;

            bool useQ = Program.Combo["comboq"].Cast<CheckBox>().CurrentValue;
            bool useW = Program.Combo["comboW"].Cast<CheckBox>().CurrentValue;
            bool useE = Program.Combo["comboE"].Cast<CheckBox>().CurrentValue;

            if(useW && Target.Distance(_Player.Position) <= Program.W.Range && Program.W.IsReady())
            {
                Program.W.Cast(Target);
            }

            if(useE && Target.Distance(_Player.Position) <= Program.E.Range && Program.E.IsReady())
            {
                Program.E.Cast(Target);
            }

            if (useQ && Target.Distance(_Player.Position) <= Program.Q.Range && Program.Q.IsReady())
            {
                var qPred = Program.Q.GetPrediction(Target);
                Program.Q.Cast(Target);
            }
        }
        public static void OnHarass()
        {
            var Target = myTarget.GetTarget(850, DamageType.Magical);
            if (Target == null) return;
            var mana = Program.Harass["harassmana"].Cast<Slider>().CurrentValue;
            bool useQ = Program.Harass["harassq"].Cast<CheckBox>().CurrentValue;
            bool useE = Program.Harass["harasse"].Cast<CheckBox>().CurrentValue;

            if (useE && Program.E.IsReady() && Program.E.IsInRange(Target) && _Player.ManaPercent >= mana)
            {
                Program.E.Cast(Target);
            }

            if (useQ && Program.Q.IsReady() && _Player.ManaPercent >= mana)
            {
                
                Program.Q.Cast(Target);
            }
            

        }
        public static void OnLaneClear()
        {
            Obj_AI_Base minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,_Player.Position,600,true).FirstOrDefault();
            if (minion == null) return;
            bool useQ = Program.LaneClear["laneq"].Cast<CheckBox>().CurrentValue;
            var Mana = Program.LaneClear["lanemana"].Cast<Slider>().CurrentValue;

            if (useQ && _Player.CalculateDamageOnUnit(minion, DamageType.Magical, 80 + ((_Player.Level - 1) * 45) + 2 / (_Player.BaseAbilityDamage + _Player.FlatMagicDamageMod)) <= minion.Health)
            {
                Program.Q.Cast(minion);
            }
        }
    }
}
