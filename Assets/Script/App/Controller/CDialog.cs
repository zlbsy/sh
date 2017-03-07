﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using Holoville.HOTween;
using UnityEngine.UI;


namespace App.Controller{
    /// <summary>
    /// 窗口弹出方式
    /// </summary>
    public enum OpenType{
        Middle,//从中间扩大
        Down,//从下面上升
        None,//无
        Fade//逐渐显示
    }
    /// <summary>
    /// 新窗口
    /// </summary>
    public class CDialog : CBase {
        [SerializeField]OpenType opentype;
        Transform panel;
        protected UnityEngine.UI.Image background;
        [HideInInspector]public int index;
        private bool _isClose;
        private Vector2 _savePosition;
        private static int dialogIndex = 0;
        protected System.Action closeEvent;
        public override IEnumerator Start()
        {
            //覆盖CBase的Start，防止OnLoad自动发生
            yield break;
        }
        public static int GetIndex(){
            return dialogIndex++;
        }
        public virtual void OnEnable(){
            if (panel == null){
                panel = this.transform.FindChild("Panel");
                GameObject backgroundObj = App.Util.Global.SceneManager.LoadPrefab("DialogBackground");
                backgroundObj.transform.SetParent(this.transform);
                RectTransform rect = backgroundObj.GetComponent<RectTransform>();
                rect.offsetMin = new Vector2(0f, 0f);
                rect.offsetMax = new Vector2(0f, 0f);
                background = backgroundObj.GetComponent<UnityEngine.UI.Image>();
            }
            background.transform.SetAsFirstSibling();
            background.color = new Color(0, 0, 0, 0);
            if (opentype == OpenType.Middle)
            {
                panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
            }else if (opentype == OpenType.Down)
            {
                RectTransform trans = panel as RectTransform;
                _savePosition = trans.anchoredPosition;
                trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, trans.sizeDelta.y * -0.5f);
            }else if (opentype == OpenType.Fade)
            {
                CanvasGroup canvasGroup = panel.gameObject.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 0;
            }
            this.GetComponent<Canvas>().sortingOrder = ++App.Util.Global.DialogSortOrder;
        }
        /// <summary>
        /// 设置新窗口唯一标示索引
        /// </summary>
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
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0.6f)));
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)));
            }else if (opentype == OpenType.Down)
            {
                HOTween.To(panel as RectTransform, 0.3f, new TweenParms().Prop("anchoredPosition", _savePosition));
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
            App.Util.Global.DialogSortOrder--;
            background.transform.SetAsLastSibling();
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(Delete));
            }
            else if (opentype == OpenType.Down)
            {
                RectTransform trans = panel as RectTransform;
                HOTween.To(panel as RectTransform, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(trans.anchoredPosition.x, trans.sizeDelta.y * -0.5f)).OnComplete(Delete));
            }
            else if (opentype == OpenType.Fade)
            {
                HOTween.To(panel.gameObject.GetComponent<CanvasGroup>(), 0.3f, new TweenParms().Prop("alpha", 0).OnComplete(Delete));
            }
            else if (opentype == OpenType.None)
            {
                Delete();
            }
        }
        public virtual void Delete(){
            HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0)).OnComplete(()=>{
                App.Util.Global.SceneManager.DestoryDialog(this);
                if(closeEvent != null){
                    closeEvent();
                }
            }));
        }
	}
}