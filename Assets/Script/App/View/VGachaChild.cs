using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View{
    public class VGachaChild : VBase {
        [SerializeField]public Text gachaName;
        [SerializeField]public Image icon;
        #region VM处理
        public VMGacha ViewModel { get { return (VMGacha)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMBuilding oldVm = oldViewModel as VMBuilding;
            if (oldVm != null)
            {
                //ViewModel.Name.OnValueChanged -= NameChanged;
                //ViewModel.TileId.OnValueChanged -= TileIdChanged;
            }
            if (ViewModel!=null)
            {
                //ViewModel.Name.OnValueChanged += NameChanged;
                //ViewModel.TileId.OnValueChanged += TileIdChanged;
            }
        }
        private void NameChanged(string oldvalue, string newvalue)
        {
            gachaName.text = Language.Get(newvalue);
        }
        private void GachaIdChanged(int oldvalue, int newvalue)
        {
            icon.sprite = App.Model.Master.MGacha.GetIcon(newvalue);
        }
        public override void UpdateView(){
            GachaIdChanged(0, ViewModel.GachaId.Value);
        }
        #endregion

    }
}