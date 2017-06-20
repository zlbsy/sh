using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public enum SkillEffectSpecial{
        none,
        /// <summary>
        /// 引导攻击
        /// </summary>
        continue_attack,
        /// <summary>
        /// 反击后反击
        /// </summary>
        attack_back_attack,
        /// <summary>
        /// 先手攻击
        /// </summary>
        force_first,
        /// <summary>
        /// 吸血
        /// </summary>
        vampire,
        /// <summary>
        /// 能力变化
        /// </summary>
        aid,
        /// <summary>
        /// 攻击范围扩大
        /// </summary>
        attack_distance,
        /// <summary>
        /// 反击不受任何限制
        /// </summary>
        force_back_attack,
        /// <summary>
        /// 必中
        /// </summary>
        force_hit,
        /// <summary>
        /// 溅射一人
        /// </summary>
        quantity_plus,
    }
    [System.Serializable]
	public class MSkillEffects : MBase {
        public MSkillEffects(){
        }
        public SkillEffectSpecial special;
        public MSkillEffect enemy;
        public MSkillEffect self;
	}
}