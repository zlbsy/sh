
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Controller;

namespace App.View.Top{
    public class VTopFooterMenu : VTopMenu{
        [SerializeField]private Transform mainMenu;
        public void OpenMenu(){
            if (mainMenu.transform.localRotation.z == 0)
            {
                (this.Controller as CTop).OpenMenu(this);
                //HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,45f)));
            }
            else
            {
                (this.Controller as CTop).CloseMenu();
                //HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,0)));
            }
        }
        public override void Open()
        {
            HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,45f)));
        }
        public override void Close(System.Action complete){
            HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,0)).OnComplete(()=>{
                if(complete != null){
                    complete();
                }
            }));
        }
	}
}