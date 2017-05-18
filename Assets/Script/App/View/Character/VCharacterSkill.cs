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
    public class VCharacterSkill : VBase {
        [SerializeField]private GameObject skillChild;
        [SerializeField]private Transform skillContent;
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        #endregion

        public override void UpdateView()
        {
            this.Controller.ScrollViewSets(skillContent, skillChild, ViewModel.Skills.Value);
            App.Model.Master.MCharacterSkill[] skills = CharacterCacher.Instance.Get(ViewModel.CharacterId.Value).skills;
            foreach (App.Model.Master.MCharacterSkill skill in skills)
            {
                if (System.Array.Exists(ViewModel.Skills.Value, s => s.SkillId == skill.skill_id))
                {
                    continue;
                }
                App.Model.MSkill mSkill = new App.Model.MSkill();
                mSkill.SkillId = skill.skill_id;
                mSkill.CanUnlock = skill.star <= ViewModel.Star.Value;
                this.Controller.ScrollViewSetChild(skillContent, skillChild, mSkill);
            }
            int skillCount = skillContent.childCount;
            for (int i = skillCount; i < 5; i++)
            {
                App.Model.MSkill mSkill = new App.Model.MSkill();
                this.Controller.ScrollViewSetChild(skillContent, skillChild, mSkill);
            }
        }
    }
}