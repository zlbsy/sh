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
                oldVm.Weapon.OnValueChanged -= WeaponChanged;
                oldVm.Clothes.OnValueChanged -= ClothesChanged;
                oldVm.Horse.OnValueChanged -= HorseChanged;
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
            StartCoroutine(EquipmentChanged(weapon, newvalue, App.Model.Master.MEquipment.EquipmentType.weapon));
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            StartCoroutine(EquipmentChanged(clothes, newvalue, App.Model.Master.MEquipment.EquipmentType.clothes));
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            StartCoroutine(EquipmentChanged(horse, newvalue, App.Model.Master.MEquipment.EquipmentType.horse));
        }
        private IEnumerator EquipmentChanged(VEquipmentIcon equipmentIcon, int equipmentId, App.Model.Master.MEquipment.EquipmentType equipmentType)
        {
            Debug.LogError("EquipmentChanged "+equipmentId);
            App.Model.MEquipment mEquipment;
            if (ViewModel.UserId.Value > 0)
            {
                App.Model.MUser user = UserCacher.Instance.Get(ViewModel.UserId.Value);
                if (user == null)
                {
                    yield return StartCoroutine(Global.SUser.RequestGet( ViewModel.UserId.Value ));
                    user = UserCacher.Instance.Get(ViewModel.UserId.Value);
                }
                mEquipment = System.Array.Find(user.equipments, 
                    _=>_.Id == equipmentId && _.character_id == ViewModel.CharacterId.Value && _.EquipmentType == equipmentType);
                Debug.LogError("mEquipment = " + mEquipment);
            }
            else
            {
                mEquipment = NpcEquipmentCacher.Instance.GetEquipment(equipmentId);
            }
            if (mEquipment == null)
            {
                mEquipment = new App.Model.MEquipment();
                mEquipment.EquipmentType = equipmentType;
                mEquipment.EquipmentId = equipmentId;
                mEquipment.Level = 1;
            }
            equipmentIcon.BindingContext = mEquipment.ViewModel;
            equipmentIcon.UpdateView();
            yield break;
        }
        public override void UpdateView(){
            App.Util.SceneManager.CurrentScene.StartCoroutine(CoroutineUpdateView());
        }
        public IEnumerator CoroutineUpdateView(){
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(EquipmentChanged(weapon, ViewModel.Weapon.Value, App.Model.Master.MEquipment.EquipmentType.weapon));
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(EquipmentChanged(clothes, ViewModel.Clothes.Value, App.Model.Master.MEquipment.EquipmentType.clothes));
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(EquipmentChanged(horse, ViewModel.Horse.Value, App.Model.Master.MEquipment.EquipmentType.horse));
        }
        #endregion

    }
}