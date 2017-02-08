using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
    public class CScene : CBase {
        public override IEnumerator Start()
        {
            App.Util.SceneManager.CurrentScene = this;
            yield return StartCoroutine (OnLoad(App.Util.SceneManager.CurrentSceneRequest));
            App.Util.SceneManager.CurrentSceneRequest = null;
        }
        public override IEnumerator OnLoad( Request request ) 
		{  
			yield return 0;
		}
	}
}