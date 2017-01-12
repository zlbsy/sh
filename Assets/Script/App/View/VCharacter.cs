using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.View{
	public class VCharacter : VBase {
		[SerializeField]Image image;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public void ChangeAction(string str){
			Debug.Log (str);
		}
	}
}