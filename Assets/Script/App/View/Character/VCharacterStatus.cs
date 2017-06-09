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
                //oldVm.Weapon.OnValueChanged -= WeaponChanged;
                //oldVm.Clothes.OnValueChanged -= ClothesChanged;
                //oldVm.Horse.OnValueChanged -= HorseChanged;
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
            statusContent.parent.gameObject.SetActive(false);
            App.Model.MCharacterAbility ability = ViewModel.Ability.Value;
            Dictionary<string, string> statusList = new Dictionary<string, string>();
            statusList.Add("资质","99");
            statusList.Add("力量",ability.Power.ToString());
            statusList.Add("技巧",ability.Knowledge.ToString());
            statusList.Add("谋略",ability.Trick.ToString());
            statusList.Add("速度",ability.Speed.ToString());
            statusList.Add("耐力",ability.Endurance.ToString());
            statusList.Add("HP",ability.HpMax.ToString());
            statusList.Add("MP",ability.MpMax.ToString());
            statusList.Add("物攻",ability.PhysicalAttack.ToString());
            statusList.Add("法攻",ability.MagicAttack.ToString());
            statusList.Add("物防",ability.PhysicalDefense.ToString());
            statusList.Add("法防",ability.MagicDefense.ToString());

            statusList.Add("轻功",ability.MovingPower.ToString());
            statusList.Add("骑术",ability.Riding.ToString());
            statusList.Add("步战",ability.Walker.ToString());
            statusList.Add("长枪",ability.Pike.ToString());
            statusList.Add("短剑",ability.Sword.ToString());
            statusList.Add("大刀",ability.LongKnife.ToString());
            statusList.Add("短刀",ability.Knife.ToString());
            statusList.Add("长斧",ability.LongAx.ToString());
            statusList.Add("短斧",ability.Ax.ToString());
            statusList.Add("棍棒",ability.LongSticks.ToString());
            statusList.Add("拳脚",ability.Sticks.ToString());
            statusList.Add("箭术",ability.Archery.ToString());
            statusList.Add("暗器",ability.HiddenWeapons.ToString());
            statusList.Add("双手",ability.DualWield.ToString());
            foreach (string key in statusList.Keys)
            {
                GameObject obj = Instantiate(statusChild);
                obj.transform.SetParent(statusContent);
                obj.transform.localScale = Vector3.one;
                VStatusChild vStatusChild = obj.GetComponent<VStatusChild>();
                vStatusChild.Set(key,statusList[key]);
            }
            statusContent.parent.gameObject.SetActive(true);
        }
    }
}