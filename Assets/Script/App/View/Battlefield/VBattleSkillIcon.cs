using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Battlefield{
    public class VBattleSkillIcon : VBase {
        [SerializeField]private Image icon;
        [SerializeField]private Text mp;
        #region VM处理
        public VMSkill ViewModel { get { return (VMSkill)BindingContext; } }
        public override void UpdateView(){
            icon.sprite = ImageAssetBundleManager.GetSkillIcon(ViewModel.SkillId.Value);
            mp.text = "14";
        }
        #endregion
        public void ClickChild(){
            this.Controller.SendMessage("SkillIconClick", ViewModel.SkillId.Value);
        }
    }
}