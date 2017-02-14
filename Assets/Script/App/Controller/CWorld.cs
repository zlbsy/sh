using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;
using App.View.Top;


namespace App.Controller{
	public class CWorld : CScene {
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield break;
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}