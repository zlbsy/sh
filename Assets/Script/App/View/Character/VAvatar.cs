using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Model.Master;
using App.Util.Cacher;
using App.Util.Battle;

namespace App.View.Character{
    public partial class VAvatar : VBase {

        [SerializeField]Anima2D.SpriteMeshInstance weapon;
        [SerializeField]Anima2D.SpriteMeshInstance[] weapons;
        private int index = 0;
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {
            base.OnBindingContextChanged(oldViewModel, newViewModel);
        }
        void OnGUI(){
            if (GUI.Button(new Rect(50, 100, 200, 30), "ChangeWeapon"))
            {
                if (index >= weapons.Length)
                {
                    index = 0;
                }
                weapon.spriteMesh = weapons[index++].spriteMesh;
            }
        }
    }
}