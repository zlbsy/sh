using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using Holoville.HOTween;
using UnityEngine.UI;


namespace App.Controller{
    public enum OpenType{
        Middle,//从中间扩大
        None,//无
        Fade//无
    }
    public class CDialog : CBase {
        [SerializeField]OpenType opentype;
        Transform panel;
        UnityEngine.UI.Image background;
        [HideInInspector]public int index;
        private bool _isClose;
        private static int dialogIndex = 0;
        protected System.Action closeEvent;
        public static int GetIndex(){
            return dialogIndex++;
        }
        public override IEnumerator Start()
        {
            yield break;
        }
        public virtual void OnEnable(){
            if (panel == null){
                panel = this.transform.FindChild("Panel");
                GameObject backgroundObj = App.Util.Global.SceneManager.LoadPrefab("DialogBackground");
                backgroundObj.transform.SetParent(this.transform);
                RectTransform rect = backgroundObj.GetComponent<RectTransform>();
                rect.offsetMin = new Vector2(0f, 0f);
                rect.offsetMax = new Vector2(0f, 0f);
                backgroundObj.transform.SetAsFirstSibling();
                background = backgroundObj.GetComponent<UnityEngine.UI.Image>();
            }
            background.color = new Color(0, 0, 0, 0);
            if (opentype == OpenType.Middle)
            {
                panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
            }else if (opentype == OpenType.Fade)
            {
                panel.gameObject.AddComponent<CanvasGroup>().alpha = 0;
            }
        }
        public void SetIndex(){
            this.index = CDialog.GetIndex();
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            if (request != null && request.Has("closeEvent"))
            {
                closeEvent = request.Get<System.Action>("closeEvent");
            }
            else
            {
                closeEvent = null;
            }
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0.5f)));
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)));
            }else if (opentype == OpenType.Fade)
            {
                HOTween.To(panel.gameObject.GetComponent<CanvasGroup>(), 0.3f, new TweenParms().Prop("alpha", 1));
            }
            _isClose = false;
            yield return 0;
        }
        public void Close(){
            if (_isClose)
            {
                return;
            }
            _isClose = true;
            background.transform.SetAsLastSibling();
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(Delete));
            }else if (opentype == OpenType.Fade)
            {
                HOTween.To(panel.gameObject.GetComponent<CanvasGroup>(), 0.3f, new TweenParms().Prop("alpha", 0).OnComplete(Delete));
            }
        }
        public void Delete(){
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0)).OnComplete(()=>{
                if(closeEvent != null){
                    closeEvent();
                }
                App.Util.Global.SceneManager.DestoryDialog(this);
            }));
        }
	}
}