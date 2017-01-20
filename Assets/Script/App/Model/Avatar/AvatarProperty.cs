using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class AvatarProperty {
		public AvatarProperty(){
		}
		public int index;
		public Vector3 position = Vector3.zero;
		public Vector3 scale = Vector3.one;
	}
}