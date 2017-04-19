using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SRegister : SBase {
        public MCharacter[] characters;
        public SRegister(){
        }
        public class ResponseList : ResponseBase
		{
            public MCharacter[] characters;
		}
        public IEnumerator RequestList()
		{
            var url = "register/character_list";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseList response = client.Deserialize<ResponseList>();
            this.characters = response.characters;
        }
	}
}