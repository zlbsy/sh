
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Battlefield{
    public class VOperatingMenu : VBottomMenu{
        public void OpenBuildingDialog(){
            
        }
        public override void Open()
        {
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,100f)));
        }
        public override void Close(System.Action complete){
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,0f)).OnComplete(()=>{
                if(complete != null){
                    complete();
                }
            }));
        }
	}
}