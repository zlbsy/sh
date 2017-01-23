using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
	public class CLogo : CBase {
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
		}
        public void GameStart(){
            App.Util.SceneManager.LoadScene( "Battlefield" );
        }
	}
}