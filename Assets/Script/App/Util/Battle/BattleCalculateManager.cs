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
        /// 攻击伤害=攻击力-防御力
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Hert(MCharacter attackCharacter, MCharacter targetCharacter){
            MSkill skill = attackCharacter.CurrentSkill;
            App.Model.Master.MSkill skillMaster = skill.Master;
            float attack = skillMaster.type == SkillType.attack ? attackCharacter.Ability.PhysicalAttack : attackCharacter.Ability.MagicAttack;
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
            return result > 1 ? (int)result : 1;
        }
    }
}