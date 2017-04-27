using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;
using System;

namespace App.Util.LSharp{
    public class LSharpCharacter : LSharpBase<LSharpCharacter> {
        public void Add(string[] arguments){
            int characterId = int.Parse(arguments[0]);
            string action = arguments[1];
            string direction = arguments[2];
            int x = int.Parse(arguments[3]);
            int y = int.Parse(arguments[4]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            App.Model.ActionType actionType = (App.Model.ActionType)Enum.Parse(typeof(App.Model.ActionType), action);
            cBaseMap.addCharacter(characterId, actionType, direction, x, y);
            LSharpScript.Instance.Analysis();
        }
	}
}