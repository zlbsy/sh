using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Equipment;


namespace App.Controller.Battle{
    public class CBattleSkillListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            MCharacter mCharacter = request.Get<MCharacter>("character");
            ScrollViewSets(content, childItem, mCharacter.Skills);
        }
        public void SkillIconClick(int skillId){
            
        }
	}
}