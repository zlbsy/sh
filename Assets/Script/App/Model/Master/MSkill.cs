using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MSkill : MBase {
        public MSkill(){
        }
        public string name;//
        public SkillType type;//
        public int level;
        public int price;//升级所需费用
        public int character_level;//升级所需英雄等级
        public WeaponType[] weapon_types;
        public int[] distance;
        public int strength;
        public RadiusType radius_type;
        public int radius;


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
        public int sticks;//棍棒
        public int fist;//拳脚
        public int archery;//箭术
        public int hidden_weapons;//暗器
        public int dual_wield;//双手
	}
}