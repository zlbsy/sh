using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;

namespace App.Util.LSharp{
    public class LSharpTalk : LSharpBase<LSharpTalk> {
        public void set(string[] arguments){
            int characterId = int.Parse(arguments[0]);
            int faceType = int.Parse(arguments[1]);
            string message = arguments[2];
            bool isLeft = arguments[3] == "true";
            CTalkDialog.ToShow(characterId, message, isLeft, LSharpScript.Instance.analysis);
        }
	}
}