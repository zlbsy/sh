using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MSkill : MBase {
        public MSkill(){
        }
        public int level;
        public string name;//
        public SkillType[] types;//
        public int price;//升级所需费用
        public int character_level;//升级所需英雄等级
        public WeaponType[] weapon_types;
        public int[] distance;
        public int strength;
        /// <summary>
        /// 半径种类
        /// </summary>
        public RadiusType radius_type;
        public int radius;
        public MSkillEffects effect;
        public string animation;
        public string explanation;

        //SkillType为ability时下列数据有效
        public int hp;//
        public int mp;//
        public int power;//力量
        public int knowledge;//技巧
        public int speed;//速度
        public int trick;//谋略
        public int endurance;//耐力

        public int moving_power;//轻功
        public int riding;//骑术
        public int walker;//步战
        public int pike;//长枪
        public int sword;//短剑
        public int long_knife;//大刀
        public int knife;//短刀
        public int long_ax;//长斧
        public int ax;//短斧
        public int long_sticks;//棍棒
        public int sticks;//棍棒
        public int archery;//箭术
        public int hidden_weapons;//暗器
        public int dual_wield;//双手
        public int resistance_metal;//抗金
        public int resistance_wood;//抗木
        public int resistance_water;//抗水
        public int resistance_fire;//抗火
        public int resistance_earth;//抗土
        public static bool IsSkillType(MSkill skill, SkillType type){
            return System.Array.Exists(skill.types, s => s == type);
        }
        public static bool IsWeaponType(MSkill skill, WeaponType type){
            return System.Array.Exists(skill.weapon_types, s => s == type);
        }
	}
}