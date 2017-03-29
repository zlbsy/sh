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
        #endregion
        public override void UpdateView()
        {
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(ViewModel.SkillId.Value);
            skillName.text = Language.Get(skillMaster.name);
            skillLevel.text = string.Format("Lv.{0}", ViewModel.Level.Value);
            money.text = "500";
            icon.sprite = ImageAssetBundleManager.GetSkillIcon(ViewModel.SkillId.Value);
        }
    }
}