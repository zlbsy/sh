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
            SUser sUser = new SUser ();
            yield return StartCoroutine (sUser.RequestLogin("aaa", "bbb"));
            if (sUser.user == null)
            {
                yield break;
            }
            Global.User = sUser.user;
            yield return StartCoroutine(sUser.VersionCheck( sUser.versions ));
            App.Util.SceneManager.LoadScene( "Top" );
        }
	}
}