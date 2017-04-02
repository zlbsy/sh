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

        //废除属性
        public int force;//武力
        public int intelligence;//智力
        public int command;//统率
        public int agility;//敏捷
        public int luck;//运气

        public int qualification;//资质
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
        public int[] skills;
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
双击 = 技巧+速度+谋略
爆击 = 谋略+速度
        */

	}
}