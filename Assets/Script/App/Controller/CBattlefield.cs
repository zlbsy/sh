using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
    public class CBattlefield : CScene {
		[SerializeField]GameObject mapLayer;
		[SerializeField]GameObject characterLayer;
		[SerializeField]GameObject characterPrefab;
		public override IEnumerator OnLoad( ) 
		{  
			SBattlefield sBattlefield = new SBattlefield ();
			yield return StartCoroutine (sBattlefield.Request(this));
			MBattlefield battlefield = sBattlefield.battlefield;
			MCharacter[] enemys = battlefield.enemys;
            Vector3[] positions = new Vector3[]{ new Vector3 (100f, 100f,0f), new Vector3 (200f, 100f,0f)};
            int count = 0;
			foreach (MCharacter chara in enemys) {
				GameObject obj = GameObject.Instantiate (characterPrefab);
                obj.transform.parent = characterLayer.transform;
                obj.transform.localPosition = positions[count++];
				obj.SetActive (true);
                Debug.Log ("chara="+chara.id+","+chara.name);
                VCharacter view = obj.GetComponent<VCharacter> ();
                view.BindingContext = chara.ViewModel;
                chara.Action = ActionType.stand;
                view.ResetAll();
			}
			yield return 0;
		}
	}
}