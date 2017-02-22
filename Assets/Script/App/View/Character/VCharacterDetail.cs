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
        [SerializeField]private Text txtNickname;
        [SerializeField]private GameObject statusChild;
        [SerializeField]private Transform statusContent;
        [SerializeField]private GameObject[] stars;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                ViewModel.CharacterId.OnValueChanged -= CharacterIdChanged;
                ViewModel.Star.OnValueChanged -= StarChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.CharacterId.OnValueChanged += CharacterIdChanged;
                ViewModel.Star.OnValueChanged += StarChanged;
            }
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
            txtName.text = Language.GetCharacterWord(ViewModel.Name.Value);
            txtNickname.text = Language.GetCharacterWord(ViewModel.Nickname.Value);
            faceIcon.CharacterId = ViewModel.CharacterId.Value;
        }
        private void StarChanged(int oldvalue, int newvalue)
        {
            int count = 0;
            foreach (GameObject star in stars)
            {
                star.SetActive(count++ < newvalue);
            }
        }
        public void ResetAll()
        {
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
            StarChanged(0, ViewModel.Star.Value);
            SetStatus();
        }
        private void SetStatus()
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