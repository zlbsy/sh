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
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        public BattleCalculateManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
        }
        /// <summary>
        /// 是否可反击
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool CanCounterAttack(MCharacter attackCharacter, MCharacter targetCharacter, int CoordinateX, int CoordinateY, int targetX, int targetY){
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
        /// 攻击伤害=攻击力-防御力
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
            float attack = skillMaster.type == SkillType.attack ? attackCharacter.Ability.PhysicalAttack : attackCharacter.Ability.MagicAttack;
            attack *= skill.Master.power;
            if (attackCharacter.IsPike && targetCharacter.IsKnife)
            {
                //枪剑类克制刀类
                attack *= 1.2f;
            }
            if (attackCharacter.IsKnife && targetCharacter.IsAx)
            {
                //刀类克制斧类
                attack *= 1.2f;
            }
            if (attackCharacter.IsAx && targetCharacter.IsPike)
            {
                //斧类克制枪剑类
                attack *= 1.2f;
            }
            float defense = skillMaster.type == SkillType.attack ? targetCharacter.Ability.PhysicalDefense : targetCharacter.Ability.MagicDefense;
            if (attackCharacter.IsLongWeapon && targetCharacter.IsShortWeapon)
            {
                //长兵器克制短兵器
                defense *= 0.8f;
            }
            if (attackCharacter.IsShortWeapon && targetCharacter.IsArcheryWeapon)
            {
                //短兵器克制远程兵器
                defense *= 0.8f;
            }
            if (attackCharacter.IsArcheryWeapon && targetCharacter.IsLongWeapon)
            {
                //远程类兵器克制长兵器
                defense *= 0.8f;
            }
            float result = (attackCharacter.Level - targetCharacter.Level) * 5 + (attack - defense) * 0.5f;
            if (attackCharacter.MoveType == MoveType.cavalry && targetCharacter.MoveType == MoveType.infantry && !targetCharacter.IsArcheryWeapon)
            {
                //骑兵克制近身步兵
                result *= 1.2f;
            }
            if (attackCharacter.IsArcheryWeapon && targetCharacter.MoveType == MoveType.cavalry && !targetCharacter.IsArcheryWeapon)
            {
                //远程类克制近身类骑兵
                result *= 1.2f;
            }
            if (attackCharacter.MoveType == MoveType.infantry && targetCharacter.WeaponType != WeaponType.archery && targetCharacter.IsArcheryWeapon)
            {
                //近身步兵克制远程类
                result *= 1.2f;
            }
            result = result > 1 ? result : 1;
            result = result > targetCharacter.Hp ? targetCharacter.Hp : result;
            return (int)result;
        }
    }
}