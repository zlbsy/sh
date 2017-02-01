﻿using System.Collections;
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
            yield return StartCoroutine (OnLoad());
        }
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
		}
	}
}