using UnityEngine;
using System.Collections;
using App.Model;

public class CharacterTest : MonoBehaviour {
	[SerializeField]GameObject characterPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(GUI.Button(new Rect(100, 50, 100, 30), "Create")){
			GameObject obj = GameObject.Instantiate (characterPrefab);
			MCharacter model = new MCharacter ();
			//character.SetAction ("Idle");
		}
		/*
		if(GUI.Button(new Rect(100, 100, 100, 30), "Move")){
			//character.SetAction ("Move");
		}
		if(GUI.Button(new Rect(100, 150, 100, 30), "Attack")){
			//character.SetAction ("Attack");
		}*/
	}
}
