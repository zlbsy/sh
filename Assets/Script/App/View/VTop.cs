using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Model;

namespace App.View{
    public class VTop : VBase{
        [SerializeField]Image tileUnit;

        #region VM处理
        public VMTop ViewModel { get { return (VMTop)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMTop oldVm = oldViewModel as VMTop;
            if (oldVm != null)
            {
                ViewModel.TopMap.OnValueChanged -= TopMapChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.TopMap.OnValueChanged += TopMapChanged;
            }
        }
        private void TopMapChanged(MTopMap oldvalue, MTopMap newvalue)
        {
            //imgHead.sprite = AssetBundleManager.GetAvatarHead("head_" + newvalue);
        }
        #endregion
	}
}