using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller{
	public class CBase : MonoBehaviour {
		public virtual IEnumerator Start()
		{
			yield return StartCoroutine (OnLoad(null));
		}
        public virtual IEnumerator OnLoad( Request request ) 
		{  
			yield return 0;
		}
        public GameObject GetObject(GameObject obj){
            return Instantiate(obj) as GameObject;
        }
	}
}