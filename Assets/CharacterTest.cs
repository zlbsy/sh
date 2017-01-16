using UnityEngine;
using System.Collections;
using App.Model;
using App.Service;

public class CharacterTest : MonoBehaviour {
	[SerializeField]GameObject characterPrefab;
	[SerializeField]Canvas layer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(GUI.Button(new Rect(100, 50, 100, 30), "Create")){
			GameObject obj = GameObject.Instantiate (characterPrefab);
			string strJson = "{\"id\":1,\"name\":\"Tester\"}";
			MCharacter model = JsonUtility.FromJson <MCharacter>(strJson);
			Debug.Log ("model="+model.id+","+model.name);
			//MCharacter model = new MCharacter ();
			obj.transform.parent = layer.transform;
			obj.SetActive (true);
			//character.SetAction ("Idle");
		}

		if(GUI.Button(new Rect(100, 100, 100, 30), "HttpTest")){
			StartCoroutine (httpTest());
		}
		/*if(GUI.Button(new Rect(100, 150, 100, 30), "Attack")){
			//character.SetAction ("Attack");
		}*/
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
