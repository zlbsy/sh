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
using App.View.Character;
using App.Controller.Common;


namespace App.Controller{
    public class CTutorialDialog : CDialog {
        [SerializeField]private RectTransform rect;
        private RectTransform icon;
        private RectTransform left;
        private RectTransform right;
        private RectTransform up;
        private RectTransform down;
        public override IEnumerator OnLoad( Request request ) 
		{  
            icon = rect.FindChild("Icon") as RectTransform;
            left = rect.FindChild("Left") as RectTransform;
            right = rect.FindChild("Right") as RectTransform;
            up = rect.FindChild("Up") as RectTransform;
            down = rect.FindChild("Down") as RectTransform;
            HideFocus();
            yield return StartCoroutine(base.OnLoad(request));
        }
        public void ClickFocus(){
            HideFocus();
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        public void HideFocus(){
            rect.gameObject.SetActive(false);
        }
        public void ShowFocus(float x, float y, float width, float height){
            const float lightSize = 26f;
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = new Vector2(x + width * 0.5f,-y - height * 0.5f);

            icon.sizeDelta = new Vector2(width, height);

            left.sizeDelta = new Vector2(x + lightSize, Camera.main.pixelHeight);
            left.anchoredPosition = new Vector2(-(width+x)*0.5f, -rect.anchoredPosition.y - Camera.main.pixelHeight * 0.5f);

            right.sizeDelta = new Vector2(Camera.main.pixelWidth - x - width + lightSize, Camera.main.pixelHeight);
            right.anchoredPosition = new Vector2((Camera.main.pixelWidth - x)*0.5f, -rect.anchoredPosition.y - Camera.main.pixelHeight * 0.5f);

            up.sizeDelta = new Vector2(width - lightSize, y + lightSize);
            up.anchoredPosition = new Vector2(0f, (height + y) * 0.5f);

            down.sizeDelta = new Vector2(width - lightSize, Camera.main.pixelHeight - y - height + lightSize);
            down.anchoredPosition = new Vector2(0f, (y - Camera.main.pixelHeight)*0.5f);
            rect.gameObject.SetActive(true);
        }
	}
}