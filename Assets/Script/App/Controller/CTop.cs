using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;


namespace App.Controller{
	public class CTop : CScene {
        [SerializeField]VTopMap topMap;
		public override IEnumerator OnLoad( ) 
		{  
            yield return StartCoroutine (App.Util.Global.SUser.RequestGet());
            //topMap.BindingContext = sUser.user.top_map.ViewModel;
            //topMap.ResetAll();
            /*SMaster sMaster = new SMaster();
            yield return StartCoroutine (sMaster.RequestAll());
            App.Model.Master.MCharacter[] characters = sMaster.characters;
            App.Util.Cacher.CharacterCacher.Instance.Reset(sMaster.characters);
            yield break;
            foreach (App.Model.Master.MCharacter character in characters)
            {
                Debug.Log("character.id=" + App.Util.Cacher.CharacterCacher.Instance.Get(character.id));
            }
            */
			yield return 0;
		}
	}
}