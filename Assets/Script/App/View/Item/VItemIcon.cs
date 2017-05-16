﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Item{
    public class VItemIcon : VBase {
        [SerializeField]private Image background;
        [SerializeField]private Image icon;
        [SerializeField]private Text cnt;
        #region VM处理
        public VMItem ViewModel { get { return (VMItem)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMItem oldVm = oldViewModel as VMItem;
            if (oldVm != null)
            {
                oldVm.Cnt.OnValueChanged -= CntChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Cnt.OnValueChanged += CntChanged;
            }
        }
        private void CntChanged(int oldvalue, int newvalue)
        {
            cnt.text = newvalue.ToString();
        }
        public override void UpdateView(){
            icon.sprite = ImageAssetBundleManager.GetItemIcon(ViewModel.ItemId.Value);
            CntChanged(0, ViewModel.Cnt.Value);
        }
        #endregion
        public void ClickChild(){
            this.Controller.SendMessage("ItemIconClick", ViewModel.Id.Value);
            //this.Controller.SendMessage("EquipmentIconClick", 1);
        }
    }
}