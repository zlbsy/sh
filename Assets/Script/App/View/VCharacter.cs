using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;

namespace App.View{
    public class VCharacter : VBase {

        [SerializeField]SpriteRenderer imgHorse;
        [SerializeField]SpriteRenderer imgBody;
        [SerializeField]SpriteRenderer imgClothes;
        [SerializeField]SpriteRenderer imgHead;
        [SerializeField]SpriteRenderer imgHat;
        [SerializeField]SpriteRenderer imgWeapon;
        private int animationIndex = 0;
        private Animator _animator;
        private Animator animator{
            get{ 
                if (_animator == null)
                {
                    _animator = this.GetComponent<Animator>();
                }
                return _animator;
            }
        }
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                ViewModel.Head.OnValueChanged -= HeadChanged;
                ViewModel.Hat.OnValueChanged -= HatChanged;
                ViewModel.Horse.OnValueChanged -= HorseChanged;
                ViewModel.Clothes.OnValueChanged -= ClothesChanged;
                ViewModel.Weapon.OnValueChanged -= WeaponChanged;
                ViewModel.Action.OnValueChanged -= ActionChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Head.OnValueChanged += HeadChanged;
                ViewModel.Hat.OnValueChanged += HatChanged;
                ViewModel.Horse.OnValueChanged += HorseChanged;
                ViewModel.Clothes.OnValueChanged += ClothesChanged;
                ViewModel.Weapon.OnValueChanged += WeaponChanged;
                ViewModel.Action.OnValueChanged += ActionChanged;
            }
        }
        private void ActionChanged(App.Model.ActionType oldvalue, App.Model.ActionType newvalue)
        {
            animationIndex = 0;
            animator.Play(newvalue.ToString());
            ResetAll();
            if (newvalue == App.Model.ActionType.stand || newvalue == App.Model.ActionType.block)
            {
                ResetAll();
            }
        }
        private void HeadChanged(int oldvalue, int newvalue)
        {
            imgHead.sprite = AssetBundleManager.GetAvatarHead("head_" + newvalue);
        }
        private void HatChanged(int oldvalue, int newvalue)
        {
            imgHat.sprite = AssetBundleManager.GetAvatarHat("hat_" + newvalue);
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            ResetAll();
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            ResetAll();
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            ResetAll();
        }
        public void ResetAll(){Debug.LogError(ViewModel.MoveType.Value+", " + ViewModel.WeaponType.Value + ", " + ViewModel.Action.Value + ", " + animationIndex);
            AvatarAction avatarAction = AvatarAsset.Data.GetAvatarAction(ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, animationIndex);
            string key;
            //Horse
            if (avatarAction.horse == null) {
                imgHorse.gameObject.SetActive (false);
            } else {
                imgHorse.gameObject.SetActive (true);
                key = string.Format("horse_{0}_{1}_{2}", ViewModel.Horse.Value, ViewModel.Action.Value, avatarAction.horse.index);
                imgHorse.sprite = AssetBundleManager.GetHorse(key);
                //imgHorse.SetNativeSize ();
                imgHorse.GetComponent<RectTransform> ().localPosition = avatarAction.horse.position;
            }
            //Body
            key = string.Format("body_{0}_{1}_{2}_{3}", ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
            imgBody.sprite = AssetBundleManager.GetAvatarBody(key);
            //imgBody.SetNativeSize ();
            //Clothes
            key = string.Format("clothes_{0}_{1}_{2}_{3}_{4}", ViewModel.Clothes.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.clothes.index);
            imgClothes.sprite = AssetBundleManager.GetClothes(key);
            imgClothes.transform.SetSiblingIndex (avatarAction.clothes.sibling);
            //imgClothes.SetNativeSize ();
            if (imgHead.gameObject.activeSelf && avatarAction.head.index == 0)
            {
                imgHead.gameObject.SetActive(false);
            }else if (!imgHead.gameObject.activeSelf && avatarAction.head.index > 0)
            {
                imgHead.gameObject.SetActive(true);
            }
            //Weapon
            key = string.Format("weapon_{0}_{1}_{2}_{3}_{4}", ViewModel.Weapon.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
            imgWeapon.sprite = AssetBundleManager.GetWeapon(key);
            if (avatarAction.weapon.sibling >= 0)
            {
                imgWeapon.transform.SetSiblingIndex(avatarAction.weapon.sibling);
            }
            //imgWeapon.SetNativeSize ();

            imgBody.GetComponent<RectTransform> ().localPosition = avatarAction.body.position;
            imgClothes.GetComponent<RectTransform> ().localPosition = avatarAction.clothes.position;
            imgHead.GetComponent<RectTransform> ().localPosition = avatarAction.head.position;
            imgWeapon.GetComponent<RectTransform> ().localPosition = avatarAction.weapon.position;
        }
        #endregion


        // Use this for initialization
        void Start () {

        }

        // Update is called once per frame
        void Update () {

        }
        public void ChangeAction(App.Model.ActionType type){
            ViewModel.Action.Value = type;
        }
        public void ChangeAnimationIdex(int index){
            animationIndex = index;
            ResetAll ();
        }
        public void EmptyAction(){
        }
    }
}