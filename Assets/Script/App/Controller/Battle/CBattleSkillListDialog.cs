using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using UnityEngine.UI;
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBattleSkillListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        [SerializeField]private GameObject preview;
        private MCharacter mCharacter;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            mCharacter = request.Get<MCharacter>("character");
            HidePreview();
            ScrollViewSets(content, childItem, mCharacter.Skills);
        }
        public void SkillIconClick(int skillId){
            mCharacter.CurrentSkill = System.Array.Find(mCharacter.Skills, _=>_.SkillId == skillId);
            this.Close();
        }
        public void ShowPreview(int skillId){
            int index = System.Array.FindIndex(mCharacter.Skills, _=>_.SkillId == skillId);
            MSkill skill = mCharacter.Skills[index];
            RectTransform trans = preview.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(110 * index - 190, trans.anchoredPosition.y);
            preview.SetActive(true);
            preview.transform.Find("Name").GetComponent<Text>().text = Language.Get(skill.Master.name);
        }
        public void HidePreview(){
            preview.SetActive(false);
        }
	}
}