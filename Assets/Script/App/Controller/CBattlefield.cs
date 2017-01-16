using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;


namespace App.Controller{
	public class CBattlefield : CBase {
		[SerializeField]GameObject mapLayer;
		[SerializeField]GameObject characterLayer;
		[SerializeField]GameObject characterPrefab;
		public override IEnumerator OnLoad( ) 
		{  
			SBattlefield sBattlefield = new SBattlefield ();
			yield return StartCoroutine (sBattlefield.Request(this));
			MBattlefield battlefield = sBattlefield.battlefield;
			MCharacter[] enemys = battlefield.enemys;
			foreach (MCharacter chara in enemys) {
				GameObject obj = GameObject.Instantiate (characterPrefab);
				obj.transform.parent = characterLayer.transform;
				obj.SetActive (true);
				Debug.Log ("chara="+chara.id+","+chara.name);
			}
			yield return 0;
		}
	}
}