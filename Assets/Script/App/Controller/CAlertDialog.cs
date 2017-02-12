using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using UnityEngine.UI;


namespace App.Controller{
    public class CAlertDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Text message;
        [SerializeField]private Text buttonText;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            message.text = request.Get<string>("message");
            if (request.Has("buttonText"))
            {
                buttonText.text = request.Get<string>("buttonText");
            }
            if (request.Has("title"))
            {
                title.text = request.Get<string>("title");
            }
            else
            {
                title.text = string.Empty;
            }
		}
        public static void Show(string title, string message, string buttonText, System.Action closeEvent){
            Request req = new Request();
            req.Set("message", message);
            if (!string.IsNullOrEmpty(title))
            {
                req.Set("title", title);
            }
            if (!string.IsNullOrEmpty(buttonText))
            {
                req.Set("buttonText", buttonText);
            }
            if (closeEvent != null)
            {
                req.Set("closeEvent", closeEvent);
            }
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.AlertDialog, req));
        }
        public static void Show(string title, string message, System.Action closeEvent){
            Show(title, message, null, closeEvent);
        }
        public static void Show(string message, System.Action closeEvent){
            Show(null, message, null, closeEvent);
        }
        public static void Show(string message){
            Show(null, message, null, null);
        }
	}
}