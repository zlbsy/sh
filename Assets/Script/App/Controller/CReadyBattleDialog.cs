using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using App.View.Character;
using UnityEngine.UI;
using App.Util;


namespace App.Controller{
    public class CReadyBattleDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Transform selectCharacterContent;
        [SerializeField]private Transform selectShadowContent;
        [SerializeField]private GameObject selectShadow;
        [SerializeField]private Transform characterContent;
        [SerializeField]private GameObject characterIcon;
        App.Model.Master.MBattlefield battleFieldMaster;
        public override IEnumerator OnLoad( Request request ) 
        {  
            int battleId = request.Get<int>("battleId");
            battleFieldMaster = BattlefieldCacher.Instance.Get(battleId);
            title.text = App.Util.Language.Get(battleFieldMaster.name);
            SelectCharacterContentInit();
            yield return this.StartCoroutine(base.OnLoad(request));
            if (App.Util.Global.SUser.self.characters == null)
            {
                SCharacter sCharacter = new SCharacter();
                yield return StartCoroutine(sCharacter.RequestList(App.Util.Global.SUser.self.id));
                App.Util.Global.SUser.self.characters = sCharacter.characters;
            }
            BaseCharacterList();
        }
        private void SelectCharacterContentInit(){
            for (int i = 0; i < battleFieldMaster.owns.Length; i++)
            {
                GameObject obj = Instantiate(selectShadow);
                obj.SetActive(true);
                obj.transform.SetParent(selectShadowContent);
            }
        }
        private void BaseCharacterList(){
            ScrollViewSets(characterContent, characterIcon, App.Util.Global.SUser.self.characters);
        }
        public void ClickCharacterIcon(VCharacterIcon vCharacterIcon){
            if (!vCharacterIcon.isSelected && selectCharacterContent.childCount >= selectShadowContent.childCount)
            {
                CAlertDialog.Show("人数满了");
                return;
            }
            ToSelectCharacter(vCharacterIcon.ViewModel.CharacterId.Value, !vCharacterIcon.isSelected);
            vCharacterIcon.isSelected = !vCharacterIcon.isSelected;
        }
        private void ToSelectCharacter(int characterId, bool isSelected){
            if (isSelected)
            {
                App.Model.MCharacter character = System.Array.Find(App.Util.Global.SUser.self.characters, _ => _.CharacterId == characterId);
                GameObject shadowObj = Instantiate(selectShadow);
                shadowObj.SetActive(true);
                shadowObj.transform.SetParent(selectCharacterContent);
                GameObject obj = Instantiate(characterIcon);
                obj.transform.SetParent(shadowObj.transform);
                obj.transform.localScale = Vector3.one * 0.65f;
                VCharacterIcon vCharacterIcon = obj.GetComponent<VCharacterIcon>();
                vCharacterIcon.BindingContext = character.ViewModel;
                vCharacterIcon.UpdateView();
            }
            else
            {
                VCharacterIcon[] icons = selectCharacterContent.GetComponentsInChildren<VCharacterIcon>();
                VCharacterIcon icon = System.Array.Find(icons, _=>_.ViewModel.CharacterId.Value == characterId);
                GameObject.Destroy(icon.transform.parent.gameObject);
            }
        }
        public void BattleStart(){
            List<int> characterIds = new List<int>();
            VCharacterIcon[] icons = selectCharacterContent.GetComponentsInChildren<VCharacterIcon>();
            if (icons.Length == 0)
            {
                return;
            }
            foreach (VCharacterIcon icon in icons)
            {
                characterIds.Add(icon.ViewModel.CharacterId.Value);
            }
            Request req = Request.Create("battlefieldId", battleFieldMaster.id, "characterIds", characterIds);
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Battlefield.ToString(), req);
        }
	}
}