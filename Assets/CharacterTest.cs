using UnityEngine;
using System.Collections;
using App.Model;
using App.Service;
using App.View;
using App.Model.Avatar;

public class CharacterTest : MonoBehaviour {
	[SerializeField]GameObject characterPrefab;
	[SerializeField]Canvas layer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	VCharacter view;
	MCharacter model;
	void OnGUI(){
		if(GUI.Button(new Rect(100, 50, 100, 30), "Create")){
			GameObject obj = GameObject.Instantiate (characterPrefab);
			obj.transform.parent = layer.transform;
			obj.transform.localPosition = new Vector3 (100f, 100f,0f);
			obj.SetActive (true);
			//obj.GetComponent<RectTransform> ().localScale = new Vector3(2f,2f,1f);
			model = new MCharacter ();
			model.MoveType = MoveType.cavalry;
			model.WeaponType = WeaponType.longKnife;
			model.Weapon = 1;
			model.Clothes = 1;
			model.Action = ActionType.attack;
			model.Horse = 1;
			model.Head = 1;
			model.Hat = 1;
			//model.body = 1;
			view = obj.GetComponent<VCharacter> ();
			view.BindingContext = model.ViewModel;
		}
		if(GUI.Button(new Rect(100, 100, 100, 30), "ChangeHead")){
			if (model.Head == 3) {
				model.Head = 1;
			} else {
				model.Head += 1;
			}
		}
		if(GUI.Button(new Rect(200, 100, 100, 30), "ChangeAction")){
			
		}
		if(GUI.Button(new Rect(100, 150, 100, 30), "ChangeHat")){
			if (model.Hat == 5) {
				model.Hat = 1;
			} else {
				model.Hat += 1;
			}
		}
		if(GUI.Button(new Rect(200, 150, 100, 30), "ChangeHorse")){
			if (model.Horse == 2) {
				model.Horse = 1;
			} else {
				model.Horse += 1;
			}
		}
		if(GUI.Button(new Rect(100, 200, 100, 30), "ChangeWeapon")){
			if (model.Weapon == 2) {
				model.Weapon = 1;
			} else {
				model.Weapon += 1;
			}
		}
		if(GUI.Button(new Rect(200, 200, 100, 30), "ChangeClothes")){
			if (model.Clothes == 2) {
				model.Clothes = 1;
			} else {
				model.Clothes += 1;
			}
		}

		if(GUI.Button(new Rect(100, 250, 100, 30), "HttpTest")){
			StartCoroutine (httpTest());
		}
	}
	IEnumerator httpTest(){
		SBattlefield battleS = new SBattlefield ();
		yield return StartCoroutine (battleS.Request(this));
		MBattlefield battlefield = battleS.battlefield;
		MCharacter[] enemys = battlefield.enemys;
		foreach (MCharacter chara in enemys) {
			Debug.Log ("chara="+chara.id+","+chara.name);
		}
	}
}
