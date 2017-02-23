using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Equipment{
    public class VEquipments : VBase {
        [SerializeField]private VEquipmentIcon weapon;
        [SerializeField]private VEquipmentIcon clothes;
        [SerializeField]private VEquipmentIcon horse;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                ViewModel.Weapon.OnValueChanged -= WeaponChanged;
                ViewModel.Clothes.OnValueChanged -= ClothesChanged;
                ViewModel.Horse.OnValueChanged -= HorseChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Weapon.OnValueChanged += WeaponChanged;
                ViewModel.Clothes.OnValueChanged += ClothesChanged;
                ViewModel.Horse.OnValueChanged += HorseChanged;
            }
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            EquipmentChanged(weapon, newvalue, App.Model.Master.MEquipment.EquipmentType.weapon);
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            EquipmentChanged(clothes, newvalue, App.Model.Master.MEquipment.EquipmentType.clothes);
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            EquipmentChanged(horse, newvalue, App.Model.Master.MEquipment.EquipmentType.horse);
        }
        private void EquipmentChanged(VEquipmentIcon equipmentIcon, int equipmentId, App.Model.Master.MEquipment.EquipmentType equipmentType)
        {
            App.Model.MEquipment mEquipment = System.Array.Find(Global.SUser.user.equipments, 
                _=>_.EquipmentId == equipmentId && _.character_id == ViewModel.CharacterId.Value && _.EquipmentType == equipmentType);
            equipmentIcon.BindingContext = mEquipment.ViewModel;
            equipmentIcon.ResetAll();
        }
        public void ResetAll(){
            EquipmentChanged(weapon, ViewModel.Weapon.Value, App.Model.Master.MEquipment.EquipmentType.weapon);
            EquipmentChanged(clothes, ViewModel.Clothes.Value, App.Model.Master.MEquipment.EquipmentType.clothes);
            EquipmentChanged(horse, ViewModel.Horse.Value, App.Model.Master.MEquipment.EquipmentType.horse);
        }
        #endregion

    }
}