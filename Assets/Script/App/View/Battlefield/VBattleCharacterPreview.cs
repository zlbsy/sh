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
        [SerializeField]private Text hp;
        [SerializeField]private Text mp;
        [SerializeField]private Text attack;
        [SerializeField]private Text physicalDefense;
        [SerializeField]private Text magicDefense;
        [SerializeField]private Text movePower;
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
                oldVm.Hp.OnValueChanged -= HpChanged;
                oldVm.Mp.OnValueChanged -= MpChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.CharacterId.OnValueChanged += CharacterIdChanged;
                ViewModel.CurrentSkill.OnValueChanged += CurrentSkillChanged;
                ViewModel.Hp.OnValueChanged += HpChanged;
                ViewModel.Mp.OnValueChanged += MpChanged;
            }
        }
        private void HpChanged(int oldvalue, int newvalue)
        {
            hp.text = string.Format("HP <color=\"#ff0000\">{0}</color>/{1}", newvalue, ViewModel.Ability.Value.HpMax);
        }
        private void MpChanged(int oldvalue, int newvalue)
        {
            mp.text = string.Format("MP <color=\"#ff0000\">{0}</color>/{1}", newvalue, ViewModel.Ability.Value.MpMax);
        }
        private void CharacterIdChanged(int oldvalue, int newvalue)
        {
            txtName.text = ViewModel.Name.Value;


            App.Model.SkillType type = ViewModel.CurrentSkill.Value.Master.type;
            if (type == App.Model.SkillType.attack)
            {
                attack.text = string.Format("物攻 {0}", ViewModel.Ability.Value.PhysicalAttack);
            }
            else if (type == App.Model.SkillType.magic)
            {
                attack.text = string.Format("法攻 {0}", ViewModel.Ability.Value.MagicAttack);
            }
            else if (type == App.Model.SkillType.heal)
            {
                attack.text = string.Format("回复 {0}", ViewModel.Ability.Value.MagicAttack);
            }
            physicalDefense.text = string.Format("物防 {0}", ViewModel.Ability.Value.PhysicalDefense);
            magicDefense.text = string.Format("法防 {0}", ViewModel.Ability.Value.MagicDefense);
            movePower.text = string.Format("移动 {0}", ViewModel.Ability.Value.MovingPower);
        }
        private void CurrentSkillChanged(App.Model.MSkill oldvalue, App.Model.MSkill newvalue)
        {
            skillname.text = newvalue.Master.name;
        }
        public override void UpdateView()
        {
            CharacterIdChanged(0, ViewModel.CharacterId.Value);
            CurrentSkillChanged(null, ViewModel.CurrentSkill.Value);
            HpChanged(0, ViewModel.Hp.Value);
            MpChanged(0, ViewModel.Mp.Value);
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