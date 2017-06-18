using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model.Master{
    [System.Serializable]
	public class MCharacter : MBase {
		public MCharacter(){
		}
        public string name;
        public string nickname;
		public int hp;//
        public int mp;//
        public int head;//
        public int hat;//
        public int weapon;//默认兵器
        public int clothes;//默认衣服
        public int horse;//默认马
        /// <summary>
        /// 资质
        /// 种类：白，蓝，紫，橙
        /// 初始技能个数分别为1，1，2，3
        /// 武将达到3星和5星，分别解锁其他两个技能
        /// 每个英雄都有一个技能空位，可以学习新技能
        /// </summary>
        public int qualification;
        public string introduction;
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
        public int long_sticks;//长棍棒
        public int sticks;//短棍棒
        public int archery;//箭术
        public int hidden_weapons;//暗器
        public int dual_wield;//双手
        public MCharacterSkill[] skills;
        /*
资质
力量
技巧
谋略
速度
耐力

物攻 = 力量*2+技巧
法攻 = 谋略*2+技巧
物防 = 力量+技巧
法防 = 谋略+技巧

物理攻击
命中/躲闪 = 技巧+速度*2
双击 = 技巧+速度-力量
爆击 = 力量+速度

法术攻击
命中/躲闪 = 技巧+速度*2
双击 = 技巧+速度-谋略
爆击 = 谋略+速度
        */

	}
}