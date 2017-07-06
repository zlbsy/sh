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
            icon = rect.Find("Icon") as RectTransform;
            left = rect.Find("Left") as RectTransform;
            right = rect.Find("Right") as RectTransform;
            up = rect.Find("Up") as RectTransform;
            down = rect.Find("Down") as RectTransform;
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
            int screenWidth = 640;
            int screenHeight = Camera.main.pixelHeight * 640 / Camera.main.pixelWidth;
            float scale = screenWidth * 1f / Camera.main.pixelWidth;
            float lightSize = 26f * scale;
            width *= scale;
            height *= scale;
            x *= scale;
            y *= scale;
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = new Vector2(x + width * 0.5f,-y - height * 0.5f);

            icon.sizeDelta = new Vector2(width, height);
            left.sizeDelta = new Vector2(x + lightSize, screenHeight);
            left.anchoredPosition = new Vector2(-(width+x)*0.5f, -rect.anchoredPosition.y - screenHeight * 0.5f);

            right.sizeDelta = new Vector2(screenWidth - x - width + lightSize, screenHeight);
            right.anchoredPosition = new Vector2((screenWidth - x)*0.5f, -rect.anchoredPosition.y - screenHeight * 0.5f);

            up.sizeDelta = new Vector2(width - lightSize, y + lightSize);
            up.anchoredPosition = new Vector2(0f, (height + y) * 0.5f);

            down.sizeDelta = new Vector2(width - lightSize, screenHeight - y - height + lightSize);
            down.anchoredPosition = new Vector2(0f, (y - screenHeight)*0.5f);
            rect.gameObject.SetActive(true);
        }
	}
}