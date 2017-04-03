using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Battlefield{
    public class VBattleCharacterPreview : VBase {
        [SerializeField]private Text txtName;
        [SerializeField]private Text skillname;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.CharacterId.OnValueChanged -= CharacterIdChanged;
                oldVm.CurrentSkill.OnValueChanged -= CurrentSkillChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.CharacterId.OnValueChanged += CharacterIdChanged;
                ViewModel.CurrentSkill.OnValueChanged += CurrentSkillChanged;
            }
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
            txtName.text = ViewModel.Name.Value;
        }
        private void CurrentSkillChanged(App.Model.MSkill oldvalue, App.Model.MSkill newvalue)
        {
            skillname.text = Language.Get(ViewModel.CurrentSkill.Value.Master.name);
        }
        public override void UpdateView()
        {
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
        }
        #endregion
        public void ClickLeft(){
            
        }
        /*public IEnumerator LoadFaceIcon(int characterId)
        {
            string url = string.Format(App.Model.Scriptable.FaceAsset.FaceUrl, characterId);
            yield return this.StartCoroutine(Global.SUser.Download(url, App.Util.Global.SUser.versions.face, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.FaceAsset.assetbundle = assetbundle;
                App.Model.Scriptable.MFace mFace = App.Model.Scriptable.FaceAsset.Data.face;
                characterIcon.sprite = Sprite.Create(mFace.image, new Rect (0, 0, mFace.image.width, mFace.image.height), Vector2.zero);
            }));
        }*/

    }
}