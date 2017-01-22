using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class AvatarProperty {
		public AvatarProperty(){
		}
		public int index = 0;
		public Vector3 position = Vector3.zero;
		public int sibling = -1;
		public Vector3 scale = Vector3.one;
	}
}