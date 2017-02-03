
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;

namespace App.View{
    public class VTopFooterMenu : VBase{
        [SerializeField]private Transform mainMenu;
        public void OpenMenu(){
            if (mainMenu.transform.localRotation.z == 0)
            {
                HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,45f)));
            }
            else
            {
                HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,0)));
            }
            //mainMenu.transform.localRotation = Quaternion.Euler(0,0,45f);
        }
	}
}