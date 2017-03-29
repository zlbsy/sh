using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Character{
    public class VCharacterSkill : VBase {
        [SerializeField]private GameObject skillChild;
        [SerializeField]private Transform skillContent;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        #endregion

        public override void UpdateView()
        {
            this.Controller.ScrollViewSets(skillContent, skillChild, ViewModel.Skills.Value);
        }
    }
}