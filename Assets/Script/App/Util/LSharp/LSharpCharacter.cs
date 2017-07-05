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
            int npcId = int.Parse(arguments[0]);
            string action = arguments[1];
            if (action == "stand")
            {
                action = "idle";
            }
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
            cBaseMap.AddCharacter(npcId, actionType, direction, x, y);
            LSharpScript.Instance.Analysis();
        }
        public void Hide(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.HideNpc(npcId, true);
            LSharpScript.Instance.Analysis();
        }
        public void Show(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.HideNpc(npcId, false);
            LSharpScript.Instance.Analysis();
        }
        public void Move(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            int x = int.Parse(arguments[1]);
            int y = int.Parse(arguments[2]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.MoveNpc(npcId, x, y);
        }
	}
}