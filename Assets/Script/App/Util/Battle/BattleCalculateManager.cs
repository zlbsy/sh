using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using App.Controller.Battle;
using App.View.Character;

namespace App.Util.Battle{
    /// <summary>
    /// 战场计算相关
    /// </summary>
    public class BattleCalculateManager{
        private CBattlefield cBattlefield;
        //private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        public BattleCalculateManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            //mBaseMap = model;
            //vBaseMap = view;
        }
        /// <summary>
        /// 攻击命中
        /// 技巧+速度*2
        /// </summary>
        /// <returns><c>true</c>, if hitrate was attacked, <c>false</c> otherwise.</returns>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool AttackHitrate(MCharacter attackCharacter, MCharacter targetCharacter){
            if (attackCharacter.IsForceHit)
            {
                return true;
            }
            int attackValue = attackCharacter.Ability.Knowledge + attackCharacter.Ability.Speed * 2;
            int targetValue = targetCharacter.Ability.Knowledge + targetCharacter.Ability.Speed * 2;
            int r;
            if(attackValue > 2*targetValue){
                r = 100;
            }else if(attackValue > targetValue){
                r=(attackValue-targetValue)*10/targetValue+90;
            }else if(attackValue > targetValue * 0.5){
                r=(attackValue-targetValue/2)*30/(targetValue/2)+60;
            }else{
                r=(attackValue-targetValue/3)*30/(targetValue/3)+30;
            }
            int randValue = Random.Range(0, 100);
            if (randValue <= r)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否可反击
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool CanCounterAttack(MCharacter attackCharacter, MCharacter targetCharacter, int CoordinateX, int CoordinateY, int targetX, int targetY){
            if (targetCharacter.IsForceBackAttack)
            {
                return true;
            }
            if (!cBattlefield.charactersManager.IsInSkillDistance(CoordinateX, CoordinateY, targetX, targetY, targetCharacter))
            {
                //不在攻击范围内
                return false;
            }
            if (attackCharacter.MoveType == MoveType.infantry && targetCharacter.MoveType == MoveType.cavalry)
            {
                //步兵攻击骑兵不受反击
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取反击次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int CounterAttackCount(MCharacter attackCharacter, MCharacter targetCharacter){
            int attackCount = 1;
            if (targetCharacter.WeaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                attackCount = 2;
            }
            //TODO::技能双击
            //MSkill skill = attackCharacter.CurrentSkill;
            //App.Model.Master.MSkill skillMaster = skill.Master;
            return attackCount;
        }
        /// <summary>
        /// 获取主动次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int SkillCount(MCharacter currentCharacter, MCharacter targetCharacter){
            int count = 1;
            if (currentCharacter.WeaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                count = 2;
            }
            //TODO::技能双击
            //MSkill skill = attackCharacter.CurrentSkill;
            //App.Model.Master.MSkill skillMaster = skill.Master;
            return count;
        }
        /// <summary>
        /// 恢复量=
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Heal(MCharacter attackCharacter, MCharacter targetCharacter, VTile tile = null, VTile targetTile = null){
            MSkill skill = attackCharacter.CurrentSkill;
            App.Model.Master.MSkill skillMaster = skill.Master;
            return 1 + attackCharacter.Level + attackCharacter.Ability.MagicAttack * skillMaster.strength;
        }
        /// <summary>
        /// 攻击伤害=技能*0.3+攻击力-防御力*2
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Hert(MCharacter attackCharacter, MCharacter targetCharacter, VTile tile = null, VTile targetTile = null){
            MSkill skill = attackCharacter.CurrentSkill;
            App.Model.Master.MSkill skillMaster = skill.Master;
            if (tile == null)
            {
                tile = cBattlefield.mapSearch.GetTile(attackCharacter.CoordinateX, attackCharacter.CoordinateY);
            }
            if (targetTile == null)
            {
                targetTile = cBattlefield.mapSearch.GetTile(targetCharacter.CoordinateX, targetCharacter.CoordinateY);
            }
            float attack = System.Array.Exists(skillMaster.types, s=>s==SkillType.attack) ? attackCharacter.Ability.PhysicalAttack : attackCharacter.Ability.MagicAttack;
            //attack *= skill.Master.power;
            if (attackCharacter.IsPike && targetCharacter.IsKnife)
            {
                //枪剑类克制刀类
                attack *= 1.2f;
            }else if (attackCharacter.IsKnife && targetCharacter.IsAx)
            {
                //刀类克制斧类
                attack *= 1.2f;
            }else if (attackCharacter.IsAx && targetCharacter.IsPike)
            {
                //斧类克制枪剑类
                attack *= 1.2f;
            }
            float defense = System.Array.Exists(skillMaster.types, s=>s==SkillType.attack) ? targetCharacter.Ability.PhysicalDefense : targetCharacter.Ability.MagicDefense;
            if (attackCharacter.IsLongWeapon && targetCharacter.IsShortWeapon)
            {
                //长兵器克制短兵器
                defense *= 0.8f;
            }else if (attackCharacter.IsShortWeapon && targetCharacter.IsArcheryWeapon)
            {
                //短兵器克制远程兵器
                defense *= 0.8f;
            }else if (attackCharacter.IsArcheryWeapon && targetCharacter.IsLongWeapon)
            {
                //远程类兵器克制长兵器
                defense *= 0.8f;
            }
            //Debug.LogError("skillMaster.strength="+skill.Master.strength + ", attack=" + attack+", defense="+defense);
            float result = skillMaster.strength * 0.3f + attack - defense * 2f;
            if (attackCharacter.MoveType == MoveType.cavalry && targetCharacter.MoveType == MoveType.infantry && !targetCharacter.IsArcheryWeapon)
            {
                //骑兵克制近身步兵
                result *= 1.2f;
            }else if (attackCharacter.IsArcheryWeapon && targetCharacter.MoveType == MoveType.cavalry && !targetCharacter.IsArcheryWeapon)
            {
                //远程类克制近身类骑兵
                result *= 1.2f;
            }else if (attackCharacter.MoveType == MoveType.infantry && targetCharacter.WeaponType != WeaponType.archery && targetCharacter.IsArcheryWeapon)
            {
                //近身步兵克制远程类
                result *= 1.2f;
            }
            if (targetCharacter.MoveType == MoveType.cavalry && skillMaster.effect.special == App.Model.Master.SkillEffectSpecial.horse_hert)
            {
                //对骑兵技能伤害加成
                result *= (1f + skillMaster.effect.special_value * 0.01f);
            }
            result = result > 1 ? result : 1;
            result = result > targetCharacter.Hp ? targetCharacter.Hp : result;
            return (int)result;
        }
    }
}