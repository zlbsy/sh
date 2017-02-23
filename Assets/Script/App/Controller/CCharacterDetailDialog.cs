using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;


namespace App.Controller{
    public class CCharacterDetailDialog : CDialog {
        [SerializeField]private App.View.Character.VCharacterDetail characterDetail;
        [SerializeField]private VCharacter vCharacter;
        App.Model.MCharacter character;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            int characterId = request.Get<int>("character_id");
            character = System.Array.Find(Global.SUser.user.characters, _=>_.CharacterId == characterId);
            characterDetail.BindingContext = character.ViewModel;
            characterDetail.ResetAll();
            vCharacter.BindingContext = character.ViewModel;
            vCharacter.ResetAll();
			yield return 0;
		}
	}
}