using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class Arms {
		public Arms(){
		}
		public AvatarAction[] attack;
		public AvatarAction[] move;
		public AvatarAction[] stand;
		public AvatarAction[] block;
		public AvatarAction GetAvatarAction(ActionType actionType, int index){
			AvatarAction[] actions = null;
			switch(actionType){
			case ActionType.stand:
				actions = stand;
				break;
			case ActionType.move:
				actions = move;
				break;
			case ActionType.attack:
			default:
				actions = attack;
				break;
			}
			return actions[index];
		}
	}
}