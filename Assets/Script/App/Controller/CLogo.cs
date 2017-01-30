using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;


namespace App.Controller{
    public class CLogo : CScene {
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
		}
        public void GameStart(){
            Global.Initialize();
            StartCoroutine(ToLogin( ));
        }
        public IEnumerator ToLogin( ) 
        {  
            yield return StartCoroutine (App.Util.Global.SUser.RequestLogin("aaa", "bbb"));
            if (App.Util.Global.SUser.user == null)
            {
                yield break;
            }
            yield return StartCoroutine(App.Util.Global.SUser.VersionCheck( App.Util.Global.SUser.versions ));
            App.Util.SceneManager.LoadScene( "Top" );
        }
	}
}