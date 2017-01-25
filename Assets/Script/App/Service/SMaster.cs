using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    /**
     * Master数据每次检索更新版本号，只获取需要更新的数据AssetBundle
     * 数据保存格式为ScriptableObject
    */
	public class SMaster : SBase {
        public App.Model.Master.MCharacter[] characters;
        public SMaster(){
		}
        public class ResponseAll : ResponseBase
		{
            public App.Model.Master.MCharacter[] characters;
		}
		public IEnumerator RequestAll()
		{
            var url = "master/all";
            WWWForm form = new WWWForm();
            form.AddField("character", 1);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            characters = client.Deserialize<ResponseAll>().characters;
		}
	}
}