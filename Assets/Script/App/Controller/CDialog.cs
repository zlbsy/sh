using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using Holoville.HOTween;


namespace App.Controller{
    public class CDialog : CScene {
        Transform panel;
        GameObject background;
        public virtual void OnEnable(){
            if (panel == null){
                panel = this.transform.FindChild("Panel");
                background = App.Util.Global.SceneManager.LoadPrefab("DialogBackground");
                background.transform.parent = this.transform;
                RectTransform rect = background.GetComponent<RectTransform>();
                rect.offsetMin = new Vector2(0f, 0f);
                rect.offsetMax = new Vector2(0f, 0f);
                background.transform.SetAsFirstSibling();
            }
            panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
        }
        public override IEnumerator OnLoad( ) 
        {  
            HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)));
            yield return 0;
        }
        public void Close(){
            background.transform.SetAsLastSibling();
            HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(()=>{
                GameObject.Destroy(this.gameObject);
            }));
        }
	}
}