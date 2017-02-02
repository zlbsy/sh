using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using Holoville.HOTween;


namespace App.Controller{
    public class CDialog : CScene {
        Transform panel;
        UnityEngine.UI.Image background;
        public virtual void OnEnable(){
            if (panel == null){
                panel = this.transform.FindChild("Panel");
                GameObject backgroundObj = App.Util.Global.SceneManager.LoadPrefab("DialogBackground");
                backgroundObj.transform.parent = this.transform;
                RectTransform rect = backgroundObj.GetComponent<RectTransform>();
                rect.offsetMin = new Vector2(0f, 0f);
                rect.offsetMax = new Vector2(0f, 0f);
                backgroundObj.transform.SetAsFirstSibling();
                background = backgroundObj.GetComponent<UnityEngine.UI.Image>();
            }
            panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
            background.color = new Color(0, 0, 0, 0);
        }
        public override IEnumerator OnLoad( ) 
        {  
            HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)));
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0.5f)));
            yield return 0;
        }
        public void Close(){
            background.transform.SetAsLastSibling();
            HOTween.To(panel, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(Delete));
        }
        public void Delete(){
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0)).OnComplete(()=>{
                GameObject.Destroy(this.gameObject);
            }));
        }
	}
}