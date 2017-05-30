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
    public class VExpCharacterIcon : VBase {
        [SerializeField]private VCharacterIcon characterIcon;
        [SerializeField]private Text txtExp;
        [SerializeField]private Image imgExp;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.CharacterId.OnValueChanged -= CharacterIdChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.CharacterId.OnValueChanged += CharacterIdChanged;
            }
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
        }
        public override void UpdateView()
        {
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
        }
        #endregion
    }
}