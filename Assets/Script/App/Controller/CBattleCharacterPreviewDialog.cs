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
using App.View.Character;


namespace App.Controller{
    public class CBattleCharacterPreviewDialog : CSingleDialog {
        [SerializeField]private VCharacter vCharacter;
        [SerializeField]private VCharacterIcon icon;
        public override IEnumerator OnLoad( Request request ) 
        {  
            MCharacter mCharacter = request.Get<MCharacter>("character");
            icon.BindingContext = mCharacter.ViewModel;
            icon.UpdateView();
            //int characterId = request.Get<int>("character_id");
            yield return StartCoroutine(base.OnLoad(request));
		}
	}
}