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
    public class VEquipmentIcon : VBase {
        [SerializeField]private Image icon;
        [SerializeField]private GameObject[] stars;
        [SerializeField]private Text level;
        #region VM处理
        public VMEquipment ViewModel { get { return (VMEquipment)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMEquipment oldVm = oldViewModel as VMEquipment;
            if (oldVm != null)
            {
                ViewModel.EquipmentId.OnValueChanged -= LevelChanged;
                //ViewModel.EquipmentType.OnValueChanged -= StarChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.EquipmentId.OnValueChanged += LevelChanged;
                //ViewModel.EquipmentType.OnValueChanged += StarChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            level.text = newvalue.ToString();
        }
        private void StarChanged(int oldvalue, int newvalue)
        {
            int count = 0;
            foreach (GameObject star in stars)
            {
                star.SetActive(count++ < newvalue);
            }
        }
        public override void UpdateView(){
            //App.Model.MEquipment mEquipment = System.Array.Find(Global.SUser.user.equipments, _=>_.Id == ViewModel.Id.Value);
            icon.sprite = ImageAssetBundleManager.GetEquipmentIcon(string.Format("{0}_{1}", ViewModel.EquipmentType.Value, ViewModel.EquipmentId.Value));
        }
        #endregion
        public void ClickChild(){
            this.Controller.SendMessage("EquipmentIconClick", ViewModel.Id.Value);
        }
    }
}