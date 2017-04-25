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
        public void Clickmask(string[] arguments){
            string[] paths = arguments[0].Split('.');
            float w = float.Parse(arguments[1]);
            float h = float.Parse(arguments[2]);
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.FindChild(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            CTutorialDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog) as CTutorialDialog;
            dialog.ShowFocus(target.position.x -w*0.5f, Camera.main.pixelHeight - target.position.y - h*0.5f, w, h);
        }
        public void Call(string[] arguments){
            string[] paths = arguments[0].Split('.');
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.FindChild(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            target.gameObject.SendMessage(arguments[1]);
            LSharpScript.Instance.Analysis();
        }
        public void Wait(string[] arguments){
            string[] paths = arguments[0].Split('.');
            App.Util.SceneManager.CurrentScene.GetComponent<App.Controller.Common.CScene>().WaitScript(paths);
        }
	}
}