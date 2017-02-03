
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;

namespace App.View{
    public class VBuildingMenu : VBase{
        public void OpenBuildingDialog(){
            StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BuildingDialog));
        }
	}
}