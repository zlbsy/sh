using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Character;


namespace App.Controller{
    public class CCharacterListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.user.characters == null)
            {
                SCharacter sCharacter = new SCharacter();
                yield return StartCoroutine(sCharacter.RequestList(Global.SUser.user.id));
                Global.SUser.user.characters = sCharacter.characters;
            }
            foreach(App.Model.MCharacter character in Global.SUser.user.characters){
                GameObject obj = Instantiate(childItem);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                VCharacterIcon vCharacterIcon = obj.GetComponent<VCharacterIcon>();
                vCharacterIcon.BindingContext = character.ViewModel;
                vCharacterIcon.UpdateView();
            }
			yield return 0;
		}
        public void ShowCharacter(int characterId){
            Request req = Request.Create("character_id", characterId);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.CharacterDetailDialog, req));
        }
	}
}