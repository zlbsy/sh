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


namespace App.Controller{
    public class CCharacterDetailDialog : CDialog {
        [SerializeField]private App.View.Character.VCharacterDetail characterDetail;
        [SerializeField]private VCharacter vCharacter;
        [SerializeField]private GameObject objStatus;
        [SerializeField]private VEquipments vEquipment;
        [SerializeField]private GameObject objSkill;
        private GameObject currentContent;
        App.Model.MCharacter character;
        public override IEnumerator OnLoad( Request request ) 
        {  
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
            vEquipment.BindingContext = character.ViewModel;
            vEquipment.ResetAll();
            currentContent = objStatus;
            yield return StartCoroutine(base.OnLoad(request));
		}
        public void EquipmentIconClick(int id){
            App.Model.MEquipment mEquipment = System.Array.Find(Global.SUser.user.equipments, _=>_.Id == id);
            Request req = Request.Create("id", id, "equipmentType", mEquipment.EquipmentType);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.EquipmentListDialog, req));
        }
        public void ChangeToEquipment(){
            if (currentContent.name == vEquipment.gameObject.name)
            {
                return;
            }
            currentContent.SetActive(false);
            vEquipment.gameObject.SetActive(true);
        }
	}
}