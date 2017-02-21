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
        App.Model.MCharacter character;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            int characterId = request.Get<int>("character_id");
            Debug.LogError("characterId = " + characterId);
            character = System.Array.Find(Global.SUser.user.characters, _=>_.CharacterId == characterId);
            Debug.LogError("character = " + character);
            characterDetail.BindingContext = character.ViewModel;
            characterDetail.ResetAll();
			yield return 0;
		}
	}
}