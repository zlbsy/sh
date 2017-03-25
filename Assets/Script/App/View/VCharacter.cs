﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Model.Master;
using App.Util.Cacher;

namespace App.View{
    public class VCharacter : VBase {

        [SerializeField]SpriteRenderer imgHorse;
        [SerializeField]SpriteRenderer imgBody;
        [SerializeField]SpriteRenderer imgClothes;
        [SerializeField]SpriteRenderer imgHead;
        [SerializeField]SpriteRenderer imgHat;
        [SerializeField]SpriteRenderer imgWeapon;
        [SerializeField]RectTransform content;
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
                oldVm.Head.OnValueChanged -= HeadChanged;
                oldVm.Hat.OnValueChanged -= HatChanged;
                oldVm.Horse.OnValueChanged -= HorseChanged;
                oldVm.Clothes.OnValueChanged -= ClothesChanged;
                oldVm.Weapon.OnValueChanged -= WeaponChanged;
                oldVm.Action.OnValueChanged -= ActionChanged;
                oldVm.CoordinateX.OnValueChanged -= CoordinateXChanged;
                oldVm.CoordinateY.OnValueChanged -= CoordinateYChanged;
                oldVm.X.OnValueChanged -= XChanged;
                oldVm.Y.OnValueChanged -= YChanged;
                oldVm.Direction.OnValueChanged -= DirectionChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Head.OnValueChanged += HeadChanged;
                ViewModel.Hat.OnValueChanged += HatChanged;
                ViewModel.Horse.OnValueChanged += HorseChanged;
                ViewModel.Clothes.OnValueChanged += ClothesChanged;
                ViewModel.Weapon.OnValueChanged += WeaponChanged;
                ViewModel.Action.OnValueChanged += ActionChanged;
                ViewModel.CoordinateX.OnValueChanged += CoordinateXChanged;
                ViewModel.CoordinateY.OnValueChanged += CoordinateYChanged;
                ViewModel.X.OnValueChanged += XChanged;
                ViewModel.Y.OnValueChanged += YChanged;
                ViewModel.Direction.OnValueChanged += DirectionChanged;
            }
        }
        private App.Controller.CBaseMap cBaseMap{
            get{
                return this.Controller as App.Controller.CBaseMap;
            }
        }
        private void DirectionChanged(App.Model.Direction oldvalue, App.Model.Direction newvalue)
        {
            content.localScale = new Vector3(newvalue == App.Model.Direction.left ? 1 : -1, 1, 1);
        }
        private void XChanged(float oldvalue, float newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            this.transform.localPosition = new Vector3(newvalue, this.transform.localPosition.y, 0f);
            if (newvalue > oldvalue)
            {
                ViewModel.Direction.Value = App.Model.Direction.right;
            }
            else if (newvalue < oldvalue)
            {
                ViewModel.Direction.Value = App.Model.Direction.left;
            }
        }
        private void YChanged(float oldvalue, float newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, newvalue, 0f);
        }
        private void CoordinateXChanged(int oldvalue, int newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            VBaseMap vBaseMap = cBaseMap.GetVBaseMap();
            int i = ViewModel.CoordinateY.Value * vBaseMap.mapWidth + newvalue;
            VTile vTile = vBaseMap.tileUnits[i];
            Debug.LogError("vTile = " + vTile.gameObject.name + ","+vTile.transform.localPosition.x);
            ViewModel.X.Value = vTile.transform.localPosition.x;
        }
        private void CoordinateYChanged(int oldvalue, int newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            VBaseMap vBaseMap = cBaseMap.GetVBaseMap();
            int i = ViewModel.CoordinateY.Value * vBaseMap.mapWidth + newvalue;
            VTile vTile = vBaseMap.tileUnits[i];
            //this.transform.localPosition = vTile.transform.localPosition;
            ViewModel.Y.Value = vTile.transform.localPosition.y;
        }
        private void ActionChanged(App.Model.ActionType oldvalue, App.Model.ActionType newvalue)
        {
            animator.Play(newvalue.ToString());
            //animationIndex = 0;
            //UpdateView();
            /*if (newvalue == App.Model.ActionType.stand || newvalue == App.Model.ActionType.block)
            {
                UpdateView();
            }*/
        }
        private void HeadChanged(int oldvalue, int newvalue)
        {
            //imgHead.sprite = ImageAssetBundleManager.GetAvatarHead(newvalue);
            UpdateView();
        }
        private void HatChanged(int oldvalue, int newvalue)
        {
            //imgHat.sprite = ImageAssetBundleManager.GetAvatarHat(newvalue);
            UpdateView();
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            UpdateView();
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            UpdateView();
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            UpdateView();
        }
        public override void UpdateView(){
            //Debug.Log(ViewModel.MoveType.Value+", " + ViewModel.WeaponType.Value + ", " + ViewModel.Action.Value + ", " + animationIndex);
            AvatarAction avatarAction = AvatarAsset.Data.GetAvatarAction(ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, animationIndex);
            string key;
            //Horse
            if (avatarAction.horse == null) {
                imgHorse.gameObject.SetActive (false);
            } else {
                imgHorse.gameObject.SetActive (true);
                key = string.Format("horse_{0}_{1}_{2}", ViewModel.Horse.Value, ViewModel.Action.Value, avatarAction.horse.index);
                imgHorse.sprite = ImageAssetBundleManager.GetHorse(key);
                imgHorse.transform.localPosition = avatarAction.horse.position;
                imgHorse.sortingOrder = avatarAction.horse.sibling;
            }

            //Body
            key = string.Format("body_{0}_{1}_{2}_{3}", ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
            imgBody.sprite = ImageAssetBundleManager.GetAvatarBody(key);
            //Clothes
            key = string.Format("clothes_{0}_{1}_{2}_{3}_{4}", ViewModel.Clothes.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.clothes.index);
            imgClothes.sprite = ImageAssetBundleManager.GetClothes(key);
            //Head
            if (imgHead.gameObject.activeSelf && avatarAction.head.index == 0)
            {
                imgHead.gameObject.SetActive(false);
            }else if (!imgHead.gameObject.activeSelf && avatarAction.head.index > 0)
            {
                imgHead.gameObject.SetActive(true);
            }
            if (imgHead.gameObject.activeSelf)
            {
                imgHead.sprite = ImageAssetBundleManager.GetAvatarHead(ViewModel.Head.Value);
                imgHat.sprite = ImageAssetBundleManager.GetAvatarHat(ViewModel.Hat.Value);
                imgHead.sortingOrder = avatarAction.head.sibling;
                imgHat.sortingOrder = avatarAction.hat.sibling;
            }

            //Weapon
            key = string.Format("weapon_{0}_{1}_{2}_{3}_{4}", ViewModel.Weapon.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
            imgWeapon.sprite = ImageAssetBundleManager.GetWeapon(key);

            imgBody.transform.localPosition = avatarAction.body.position;
            imgClothes.transform.localPosition = avatarAction.clothes.position;
            imgHead.transform.localPosition = avatarAction.head.position;
            imgWeapon.transform.localPosition = avatarAction.weapon.position;

            imgBody.sortingOrder = avatarAction.body.sibling;
            imgClothes.sortingOrder = avatarAction.clothes.sibling;
            imgHead.sortingOrder = avatarAction.head.sibling;
            imgWeapon.sortingOrder = avatarAction.weapon.sibling;
        }
        #endregion


        // Use this for initialization
        void Start () {

        }

        // Update is called once per frame
        void Update () {

        }
        public void ChangeAction(App.Model.ActionType type){
            animator.Stop();
            animationIndex = 0;
            ViewModel.Action.Value = type;
        }
        public void ChangeAnimationIdex(int index){
            animationIndex = index;
            UpdateView ();
        }
        public void EmptyAction(){
        }
    }
}