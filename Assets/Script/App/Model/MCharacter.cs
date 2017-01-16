using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model{
	public enum MoveType{
		infantry,//步兵
		cavalry,//骑兵
	}
	public enum WeaponType{
		shortKnife,//短刀
		longKnife,//长刀
		sword,//剑
		gun,//枪
		shortAx,//短斧
		longAx,//长斧
	}
	public class MCharacter : MBase {
		public MCharacter(){
		}
		public int id;
		public string name;
		public int hp;
		public int mp;
		public int sp;
		public WeaponType weaponType;
		public MoveType moveType;
	}
}