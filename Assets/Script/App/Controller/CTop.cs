using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
	public class CTop : CScene {
		public override IEnumerator OnLoad( ) 
		{  
            SMaster sMaster = new SMaster();
            yield return StartCoroutine (sMaster.RequestAll());
            App.Model.Master.MCharacter[] characters = sMaster.characters;
            App.Util.Cacher.CharacterCacher.Instance.Reset(sMaster.characters);
            foreach (App.Model.Master.MCharacter character in characters)
            {
                Debug.Log("character.id=" + App.Util.Cacher.CharacterCacher.Instance.Get(character.id));
            }

			yield return 0;
		}
	}
}