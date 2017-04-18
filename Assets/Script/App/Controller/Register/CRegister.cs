using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View.Character;
using App.Controller.Common;


namespace App.Controller.Register{
    public class CRegister : CScene {
        [SerializeField]private VFace faceIcon;
        [SerializeField]private VCharacter vCharacter;
        [SerializeField]private VCharacterStatus vCharacterStatus;
        public override IEnumerator OnLoad( Request request ) 
        {  
			yield return 0;
		}
        public void GameStart(){
            
        }
	}
}