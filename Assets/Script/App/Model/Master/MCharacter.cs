using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model.Master{
    [System.Serializable]
	public class MCharacter : MBase {
		public MCharacter(){
		}
		public string name;
		public int hp;//
		public int mp;//
        public int sp;//怒气
        public int force;//武力
        public int intelligence;//智力
        public int command;//统率
        public int agility;//敏捷
        public int luck;//运气
        public int power;//力量
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
        public float[] face_rect;//小头像范围
        private UnityEngine.Rect _faceRect;
        public UnityEngine.Rect FaceRect{
            get{ 
                if (_faceRect == null)
                {
                    _faceRect = new UnityEngine.Rect(face_rect[0], face_rect[1], face_rect[2], face_rect[3]);
                }
                return _faceRect;
            }
        }

	}
}