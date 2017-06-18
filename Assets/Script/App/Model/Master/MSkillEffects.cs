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