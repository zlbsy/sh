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
        //public MCharacter[] characters;
        public SRegister(){
        }
        //public class ResponseList : ResponseBase
		//{
        //    public MCharacter[] characters;
		//}
        public IEnumerator RequestInsert(int selectId, string account, string password, string name)
		{
            var url = "register/insert";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("selectId", selectId);
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("name", name);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            //ResponseList response = client.Deserialize<ResponseList>();
        }
	}
}