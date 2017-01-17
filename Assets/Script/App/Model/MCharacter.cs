using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


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
			viewModel = new VMCharacter ();
		}
		public VMCharacter ViewModel { get { return (VMCharacter)viewModel; } }
		public int id;
		public string name;
		public int hp;
		public int mp;
		public int sp;
		public int body;
		public int hat;
		public int horse;
		public WeaponType weaponType;
		public MoveType moveType;
		public int Head{
			set{ 
				this.ViewModel.Head.Value = value;
			}
			get{ 
				return this.ViewModel.Head.Value;
			}
		}
	}
}