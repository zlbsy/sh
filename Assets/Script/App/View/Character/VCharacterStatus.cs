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
    public class VCharacterStatus : VBase {
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
            Global.ClearChild(statusContent.gameObject);
            Dictionary<string, string> statusList = new Dictionary<string, string>();
            statusList.Add("资质","99");
            statusList.Add("力量","99");
            statusList.Add("技巧","99");
            statusList.Add("谋略","99");
            statusList.Add("速度","99");
            statusList.Add("耐力","99");
            statusList.Add("HP","99");
            statusList.Add("MP","99");
            statusList.Add("物攻","99");
            statusList.Add("法攻","99");
            statusList.Add("物防","99");
            statusList.Add("法防","99");

            statusList.Add("轻功","99");
            statusList.Add("骑术","99");
            statusList.Add("步战","99");
            statusList.Add("长枪","99");
            statusList.Add("短剑","99");
            statusList.Add("大刀","99");
            statusList.Add("短刀","99");
            statusList.Add("长斧","99");
            statusList.Add("短斧","99");
            statusList.Add("棍棒","99");
            statusList.Add("拳脚","99");
            statusList.Add("箭术","99");
            statusList.Add("暗器","99");
            statusList.Add("双手","99");
            foreach (string key in statusList.Keys)
            {
                GameObject obj = Instantiate(statusChild);
                obj.transform.SetParent(statusContent);
                obj.transform.localScale = Vector3.one;
                VStatusChild vStatusChild = obj.GetComponent<VStatusChild>();
                vStatusChild.Set(key,statusList[key]);
            }
        }
    }
}