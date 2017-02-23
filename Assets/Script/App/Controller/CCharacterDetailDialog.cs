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
        [SerializeField]private GameObject objStatus;
        [SerializeField]private GameObject objEquipment;
        [SerializeField]private GameObject objSkill;
        private GameObject currentContent;
        App.Model.MCharacter character;
        public override IEnumerator OnLoad( Request request ) 
        {  
            Debug.Log("CCharacterDetailDialog");
            yield return StartCoroutine(base.OnLoad(request));
            int characterId = request.Get<int>("character_id");
            if (Global.SUser.user.equipments == null)
            {
                SEquipment sEquipment = new SEquipment();
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.user.equipments = sEquipment.equipments;
            }
            character = System.Array.Find(Global.SUser.user.characters, _=>_.CharacterId == characterId);
            characterDetail.BindingContext = character.ViewModel;
            characterDetail.ResetAll();
            vCharacter.BindingContext = character.ViewModel;
            vCharacter.ResetAll();
            currentContent = objStatus;
            Debug.Log("currentContent = " + currentContent);
            Debug.Log("objStatus = " + objStatus);
			yield return 0;
		}
        public void EquipmentIconClick(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.EquipmentListDialog));
        }
        public void ChangeToEquipment(){
            Debug.Log("currentContent = " + currentContent);
            Debug.Log("objEquipment = " + objEquipment);
            if (currentContent.name == objEquipment.name)
            {
                return;
            }
            currentContent.SetActive(false);
            objEquipment.SetActive(true);
        }
	}
}