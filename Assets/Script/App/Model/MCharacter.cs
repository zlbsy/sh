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
	public enum ActionType{
		stand,
		move,
		attack,
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
		public string Horse{
			set{
				this.ViewModel.Horse.Value = value;
			}
			get{ 
				return this.ViewModel.Horse.Value;
			}
		}
		public ActionType Action{
			set{
				this.ViewModel.Action.Value = value;
			}
			get{ 
				return this.ViewModel.Action.Value;
			}
		}
		public WeaponType WeaponType{
			set{
				this.ViewModel.WeaponType.Value = value;
			}
			get{ 
				return this.ViewModel.WeaponType.Value;
			}
		}
		public MoveType MoveType{
			set{ 
				this.ViewModel.MoveType.Value = value;
			}
			get{ 
				return this.ViewModel.MoveType.Value;
			}
		}
		public int Head{
			set{ 
				this.ViewModel.Head.Value = value;
			}
			get{ 
				return this.ViewModel.Head.Value;
			}
		}
		public int Hat{
			set{ 
				this.ViewModel.Hat.Value = value;
			}
			get{ 
				return this.ViewModel.Hat.Value;
			}
		}
	}
}