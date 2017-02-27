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
        [SerializeField]private GameObject statusChild;
        [SerializeField]private Transform statusContent;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                //ViewModel.Weapon.OnValueChanged -= WeaponChanged;
                //ViewModel.Clothes.OnValueChanged -= ClothesChanged;
                //ViewModel.Horse.OnValueChanged -= HorseChanged;
            }
            if (ViewModel!=null)
            {
                //ViewModel.Weapon.OnValueChanged += WeaponChanged;
                //ViewModel.Clothes.OnValueChanged += ClothesChanged;
                //ViewModel.Horse.OnValueChanged += HorseChanged;
            }
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            
        }
        #endregion

        public override void UpdateView()
        {
            
        }
    }
}