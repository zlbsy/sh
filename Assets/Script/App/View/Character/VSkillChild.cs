using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;


namespace App.View.Character{
    public class VSkillChild : VBase {
        [SerializeField]private Text skillName;
        [SerializeField]private Text skillLevel;
        [SerializeField]private Text money;
        [SerializeField]private Image icon;

        #region VM处理
        public VMSkill ViewModel { get { return (VMSkill)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMSkill oldVm = oldViewModel as VMSkill;
            if (oldVm != null)
            {
                oldVm.Level.OnValueChanged -= LevelChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Level.OnValueChanged += LevelChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(ViewModel.SkillId.Value, ViewModel.Level.Value);
            skillLevel.text = string.Format("Lv.{0}", newvalue) + (skillMaster.character_level > 0 ? "" : "(MAX)");
            money.text = skillMaster.character_level > 0 ? skillMaster.price.ToString() : "--";
        }
        #endregion
        public override void UpdateView()
        {
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(ViewModel.SkillId.Value);
            skillName.text = skillMaster.name;
            LevelChanged(0, ViewModel.Level.Value);
            //skillLevel.text = string.Format("Lv.{0}", ViewModel.Level.Value) + (skillMaster.character_level > 0 ? "" : "(MAX)");
            //money.text = skillMaster.character_level > 0 ? skillMaster.price.ToString() : "--";
            icon.sprite = ImageAssetBundleManager.GetSkillIcon(ViewModel.SkillId.Value);
        }
        public void LevelUp(){
            this.Controller.SendMessage("SkillLevelUp", ViewModel.Id.Value);
        }
    }
}