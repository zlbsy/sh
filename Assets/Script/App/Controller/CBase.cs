using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller{
	public class CBase : MonoBehaviour {
		public virtual IEnumerator Start()
		{
			yield return StartCoroutine (OnLoad());
		}
		public virtual IEnumerator OnLoad( ) 
		{  
			yield return 0;
		}
	}
}