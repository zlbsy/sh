using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;


namespace App.Controller.Common{
    /// <summary>
    /// 场景
    /// </summary>
    public class CScene : CBase {
        public override IEnumerator Start()
        {
            //场景子窗口排序初始化
            App.Util.Global.DialogSortOrder = 0;
            //保存当前场景
            App.Util.SceneManager.CurrentScene = this;
            yield return StartCoroutine (OnLoad(App.Util.SceneManager.CurrentSceneRequest));
            App.Util.SceneManager.CurrentSceneRequest = null;
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            SUser sUser = Global.SUser;
            if (sUser != null && sUser.self != null && Global.Constant != null)
            {
                int tutorial = sUser.self.GetValue("tutorial");
                if (tutorial < Global.Constant.tutorial_end)
                {
                    this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.TutorialDialog));
                }
            }
			yield return 0;
		}
	}
}