using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	[SerializeField]private Animator animation;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetAction(string action){
		animation.Play (action);
	}
}
