using UnityEngine;
using System.Collections;
using App.Model;
using App.Service;
using App.View;

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
			obj.transform.localPosition = new Vector3 (0f, 100f,0f);
			obj.SetActive (true);
			obj.GetComponent<RectTransform> ().localScale = new Vector3(3f,3f,1f);
			model = new MCharacter ();
			view = obj.GetComponent<VCharacter> ();
			view.BindingContext = model.ViewModel;
			model.Head = 2;
			//character.SetAction ("Idle");
		}
		if(GUI.Button(new Rect(100, 100, 100, 30), "ChangeHead")){
			model.Head = 3;
		}

		if(GUI.Button(new Rect(100, 150, 100, 30), "HttpTest")){
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
