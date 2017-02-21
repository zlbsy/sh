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
    public class VCharacterDetail : VBase {
        [SerializeField]private VFace faceIcon;
        [SerializeField]private Text txtName;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                //ViewModel.Name.OnValueChanged -= NameChanged;
                //ViewModel.TileId.OnValueChanged -= TileIdChanged;
            }
            if (ViewModel!=null)
            {
                //ViewModel.Name.OnValueChanged += NameChanged;
                //ViewModel.TileId.OnValueChanged += TileIdChanged;
            }
        }
        private void NameChanged(string oldvalue, string newvalue)
        {
            txtName.text = Language.Get(newvalue);
        }
        public void ResetAll()
        {
            NameChanged("", ViewModel.Name.Value);

            //App.Model.Master.MCharacter mCharacter = CharacterCacher.Instance.Get(ViewModel.CharacterId.Value);
            //name.text = Language.Get(mCharacter.name);
            faceIcon.CharacterId = ViewModel.CharacterId.Value;
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