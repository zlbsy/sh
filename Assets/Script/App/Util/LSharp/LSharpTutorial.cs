using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;

namespace App.Util.LSharp{
    public class LSharpTutorial : LSharpBase<LSharpTutorial> {
        public void Close(string[] arguments){
            App.Controller.Common.CDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog);
            dialog.Close();
            LSharpScript.Instance.Analysis();
        }
	}
}