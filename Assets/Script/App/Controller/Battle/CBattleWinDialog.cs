using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Controller.Common;
using Holoville.HOTween;


namespace App.Controller.Battle{
    public class CBattleWinDialog : CDialog {
        [SerializeField]private GameObject buttonContents;
        public override IEnumerator OnLoad( Request request ) 
        {  
            buttonContents.SetActive(false);
            int[] characterIds = request.Get<int[]>("characterIds");
            int[] dieIds = request.Get<int[]>("dieIds");
            int star = request.Get<int>("star");
            yield return this.StartCoroutine(App.Util.Global.SBattlefield.RequestBattleEnd(characterIds, dieIds, star));
            yield return StartCoroutine(base.OnLoad(request));

        }
        public void ButtonContentsShow(){
            if (buttonContents.activeSelf)
            {
                return;
            }
            buttonContents.SetActive(true);
            buttonContents.transform.localScale = Vector3.zero;
            HOTween.To(buttonContents.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInQuart));

        }
        public void BattleOver(){
            (App.Util.SceneManager.CurrentScene as CBattlefield).BattleEnd();
        }
        public void GoldRelive(){
            CAlertDialog.Show("待开发");
        }
        public void ItemRelive(){
            CAlertDialog.Show("待开发");
        }
    }
}