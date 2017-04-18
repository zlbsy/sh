using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View.Character;
using App.Controller.Common;
using App.Service;
using App.Model;


namespace App.Controller.Register{
    public class CRegister : CScene {
        [SerializeField]private VFace faceIcon;
        [SerializeField]private VCharacter vCharacter;
        [SerializeField]private VCharacterStatus vCharacterStatus;
        private MCharacter[] allCharacters;
        private MCharacter[] characters;
        private int index = 0;
        private Gender gender = Gender.male;
        public override IEnumerator OnLoad( Request request ) 
        {  
            SCharacter sCharacter = new SCharacter();
            yield return StartCoroutine(sCharacter.RequestRegisterList());
            allCharacters = sCharacter.characters;
            characters = System.Array.FindAll(allCharacters, chara => chara.Gender == gender);
            yield return StartCoroutine(base.OnLoad(request));
		}
        public void CharacterSelectComplete(){
            
        }
        public void SelectLeft(){
            index -= 1;
            if (index < 0)
            {
                index = characters.Length - 1;
            }
        }
        public void SelectRight(){
            index += 1;
            if (index > characters.Length - 1)
            {
                index = 0;
            }
        }
	}
}