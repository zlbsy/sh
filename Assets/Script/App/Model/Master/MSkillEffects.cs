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
        /// <summary>
        /// 对骑兵攻击加成
        /// </summary>
        horse_hert,
        /// <summary>
        /// 攻击后移动
        /// </summary>
        move_after_attack,
        /// <summary>
        /// 每回合固定伤害
        /// </summary>
        bout_fixed_damage,
        /// <summary>
        /// 攻击次数
        /// </summary>
        attack_count,
        /// <summary>
        /// 反击次数
        /// </summary>
        counter_attack_count,
        /*
        /// <summary>
        /// 固定伤害攻击
        /// </summary>
        fixed_damage,*/
    }
    [System.Serializable]
	public class MSkillEffects : MBase {
        public MSkillEffects(){
        }
        public SkillEffectSpecial special;
        public int special_value;
        public MSkillEffect enemy;
        public MSkillEffect self;
	}
}