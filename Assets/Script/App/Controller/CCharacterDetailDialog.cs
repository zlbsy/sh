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
using App.Controller.Common;


namespace App.Controller{
    public class CCharacterDetailDialog : CDialog {
        [SerializeField]private App.View.Character.VCharacterDetail characterDetail;
        [SerializeField]private App.View.Character.VCharacter vCharacter;
        [SerializeField]private VCharacterStatus vCharacterStatus;
        [SerializeField]private VEquipments vEquipment;
        [SerializeField]private VCharacterSkill vCharacterSkill;
        private GameObject currentContent;
        private VBase[] contents;
        App.Model.MCharacter character;
        SEquipment sEquipment = new SEquipment();
        public override IEnumerator OnLoad( Request request ) 
        {  
            int characterId = request.Get<int>("character_id");
            if (Global.SUser.self.equipments == null)
            {
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.self.equipments = sEquipment.equipments;
            }
            character = System.Array.Find(Global.SUser.self.characters, _=>_.CharacterId == characterId);
            character.StatusInit();
            characterDetail.BindingContext = character.ViewModel;
            characterDetail.UpdateView();
            vCharacter.BindingContext = character.ViewModel;
            vCharacter.UpdateView();
            vCharacterStatus.BindingContext = character.ViewModel;
            vCharacterStatus.UpdateView();
            vEquipment.BindingContext = character.ViewModel;
            vEquipment.UpdateView();
            vCharacterSkill.BindingContext = character.ViewModel;
            vCharacterSkill.UpdateView();
            contents = new VBase[]{ vCharacterStatus, vEquipment, vCharacterSkill };
            ShowContentFromIndex(0);
            yield return StartCoroutine(base.OnLoad(request));
		}
        public void EquipmentIconClick(VCharacterEquipmentIcon vEquipmentIcon){
            System.Action<int> selectEvent = (int selectId)=>{
                StartCoroutine(EquipmentChange(selectId));
            };
            Request req = Request.Create("equipmentType", vEquipmentIcon.equipmentType, "selectEvent", selectEvent);
            if (vEquipmentIcon.equipmentType == App.Model.Master.MEquipment.EquipmentType.horse)
            {
                req.Set("moveType", vEquipmentIcon.moveType);
            }
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.EquipmentListDialog, req));
        }
        private IEnumerator EquipmentChange(int id){
            yield return StartCoroutine(sEquipment.RequestEquip(character.CharacterId, id));
            App.Model.MEquipment mEquipment = System.Array.Find(Global.SUser.self.equipments, _=>_.Id == id);
            Global.SUser.self.equipments = sEquipment.equipments;
            if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.weapon)
            {
                character.Weapon = mEquipment.EquipmentId;
            }else if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.clothes)
            {
                character.Clothes = mEquipment.EquipmentId;
            }else if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.horse)
            {
                character.Horse = mEquipment.EquipmentId;
            }
        }
        public void SkillLevelUp(int id){
            Debug.Log("SkillLevelUp id="+id);
            App.Model.MSkill mSkill = System.Array.Find(character.Skills, s=>s.Id == id);
            if (Global.SUser.self.Silver < mSkill.Master.price)
            {
                CAlertDialog.Show("银两不够");
                return;
            }
            StartCoroutine(SkillLevelUpRun(id, mSkill));
        }
        private IEnumerator SkillLevelUpRun(int id, App.Model.MSkill mSkill){
            SSkill sSkill = new SSkill();
            yield return StartCoroutine(sSkill.RequestLevelUp(id));
            mSkill.Update(sSkill.skill);
        }
        public void ChangeContent(){
            int index = System.Array.FindIndex(contents, _=>_.gameObject.name == currentContent.name) + 1;
            ShowContentFromIndex(index);
        }
        public void ShowContentFromIndex(int index){
            System.Array.ForEach(contents, _=>_.gameObject.SetActive(false));
            if (index >= contents.Length)
            {
                index = 0; 
            }
            currentContent = contents[index].gameObject;
            currentContent.SetActive(true);
        }
	}
}