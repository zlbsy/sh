using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;
using System;

namespace App.Util.LSharp{
    public class LSharpBattle : LSharpBase<LSharpBattle> {
        public void Start(string[] arguments){
            int battleId = int.Parse(arguments[0]);
            Request req = Request.Create("", battleId);
            IEnumerator enumerator = Global.SceneManager.ShowDialog(SceneManager.Prefabs.ReadyBattleDialog, req);
            App.Util.SceneManager.CurrentScene.StartCoroutine(enumerator);
            //LSharpScript.Instance.Analysis();
        }
	}
}