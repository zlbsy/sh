using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class MoveArms {
		public MoveArms(){
		}
		public Arms shortKnife;
		public Arms longKnife;
		public AvatarAction GetAvatarAction(WeaponType weaponType, ActionType actionType, int index){
			Arms arms = null;
			switch(weaponType){
			case WeaponType.knife:
				arms = shortKnife;
				break;
			case WeaponType.longKnife:
			default:
				arms = longKnife;
				break;
			}
			return arms.GetAvatarAction(actionType, index);
		}
	}
}