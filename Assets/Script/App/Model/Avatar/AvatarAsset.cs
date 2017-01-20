﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	public class AvatarAsset : ScriptableObject {
		public AvatarAsset(){
		}
		[SerializeField]public MoveArms cavalry;
		[SerializeField]public MoveArms infantry;

		public const string PATH = "avatarAsset";
		private static AvatarAsset _data;
		public  static AvatarAsset  Data{
			get{ 
				if (_data == null) {
					_data = Resources.Load<AvatarAsset>(PATH);
				}
				return _data;
			}
		}
		public AvatarAction GetAvatarAction(MoveType moveType, WeaponType weaponType, ActionType actionType, int index){
			MoveArms moveArms = moveType == MoveType.cavalry ? cavalry : infantry;
			return moveArms.GetAvatarAction (weaponType, actionType, index);
		}
	}
}