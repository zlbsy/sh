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
    public class VCharacterIcon : VBase {
        [SerializeField]private RawImage icon;
        [SerializeField]private App.View.Character.VRawFace faceIcon;
        [SerializeField]private GameObject[] stars;
        [SerializeField]private Text level;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                ViewModel.Level.OnValueChanged -= LevelChanged;
                ViewModel.Star.OnValueChanged -= StarChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Level.OnValueChanged += LevelChanged;
                ViewModel.Star.OnValueChanged += StarChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            level.text = newvalue.ToString();
        }
        private void StarChanged(int oldvalue, int newvalue)
        {
            int count = 0;
            foreach (GameObject star in stars)
            {
                star.SetActive(count++ < newvalue);
            }
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
            faceIcon.CharacterId = newvalue;
            //StartCoroutine(LoadFaceIcon(newvalue));
            /*return;
            App.Model.Master.MCharacter mCharacter = CharacterCacher.Instance.Get(newvalue);

            icon.uvRect = mCharacter.FaceRect;*/
        }
        public void ResetAll(){
            LevelChanged(0, ViewModel.Level.Value);
            StarChanged(0, ViewModel.Star.Value);
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
        }
        #endregion
        public void ClickChild(){
            (this.Controller as CCharacterListDialog).ShowCharacter(ViewModel.CharacterId.Value);
        }
        /*public IEnumerator LoadFaceIcon(int characterId)
        {
            string url = string.Format(App.Model.Scriptable.FaceAsset.FaceUrl, characterId);
            yield return this.StartCoroutine(Global.SUser.Download(url, App.Util.Global.SUser.versions.face, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.FaceAsset.assetbundle = assetbundle;
                App.Model.Scriptable.MFace mFace = App.Model.Scriptable.FaceAsset.Data.face;
                icon.texture = mFace.image as Texture;
                //icon.texture = Sprite.Create(mFace.image, new Rect (0, 0, mFace.image.width, mFace.image.height), Vector2.zero).texture as Texture;
            }));
        }
        */
    }
}