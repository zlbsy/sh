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
            float x = float.Parse(arguments[1]);
            float y = float.Parse(arguments[2]);
            float w = float.Parse(arguments[3]);
            float h = float.Parse(arguments[4]);
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            CTutorialDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog) as CTutorialDialog;
            dialog.ShowFocus(target.position.x -w*0.5f + x, Camera.main.pixelHeight - target.position.y - h*0.5f + y, w, h);
        }
        public void Clickmask3d(string[] arguments){
            string[] paths = arguments[0].Split('.');
            float x = float.Parse(arguments[1]);
            float y = float.Parse(arguments[2]);
            float w = float.Parse(arguments[3]);
            float h = float.Parse(arguments[4]);
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            CTutorialDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog) as CTutorialDialog;
            int intDefault = LayerMask.NameToLayer("Default");
            //Debug.LogError("intDefault = " + intDefault);
            //int intUI = LayerMask.NameToLayer("UI");
            //Debug.LogError("intUI = " + intUI);
            Camera[] cameras = App.Util.SceneManager.CurrentScene.GetComponentsInChildren<Camera>();
            Camera camera3D = System.Array.Find(cameras, c=>c.gameObject.layer == intDefault);
            //Camera cameraUI = System.Array.Find(cameras, c=>c.gameObject.layer == intUI);
            //Debug.LogError("cameraUI w h = " + cameraUI.pixelWidth + ", " + cameraUI.pixelHeight);
            //Debug.LogError("camera3D = " + RectTransformUtility.WorldToScreenPoint(camera3D,target.position));
            //Debug.LogError("cameraUI = " + RectTransformUtility.WorldToScreenPoint(cameraUI,target.position));
            Vector2 vec = RectTransformUtility.WorldToScreenPoint(camera3D, target.position);
            dialog.ShowFocus(vec.x + x, Camera.main.pixelHeight - vec.y + y, w, h);
        }
        public void Call(string[] arguments){
            string[] paths = arguments[0].Split('.');
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
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
        public void Camerato(string[] arguments){
            int worldId = int.Parse(arguments[0]);
            CBaseMap cBasemap = SceneManager.CurrentScene as CBaseMap;
            cBasemap.CameraTo(worldId);
        }
	}
}