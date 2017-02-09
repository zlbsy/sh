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
    public class VHeaderFace : VBase {
        [SerializeField]public RawImage icon;
        [SerializeField]public Text nickname;
        [SerializeField]public Text level;
        #region VM处理
        public VMUser ViewModel { get { return (VMUser)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMUser oldVm = oldViewModel as VMUser;
            if (oldVm != null)
            {
                //ViewModel.Nickname.OnValueChanged -= NicknameChanged;
                ViewModel.Level.OnValueChanged -= LevelChanged;
            }
            if (ViewModel!=null)
            {
                //ViewModel.Nickname.OnValueChanged += NicknameChanged;
                ViewModel.Level.OnValueChanged += LevelChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            level.text = newvalue.ToString();
        }
        #endregion
        public void ResetAll(){
            level.text = ViewModel.Level.Value.ToString();
            nickname.text = ViewModel.Nickname.Value.ToString();
        }
        void OnMouseUp(){
            
        }

    }
}