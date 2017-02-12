using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
	public class SUser : SBase {
        public MUser user;
        public MVersion versions;
        public SUser(){
		}
        public class ResponseAll : ResponseBase
		{
            public string ssid;
            public MVersion versions;
		}
        public IEnumerator RequestLogin(string name, string pass)
		{
            var url = "user/login";
            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("pass", pass);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            ResponseAll response = client.Deserialize<ResponseAll>();
            this.user = response.user;
            this.versions = response.versions;
            App.Util.Global.ssid = response.ssid;
        }
        public IEnumerator RequestGet(string name = "")
        {
            var url = "user/get";
            HttpClient client = new HttpClient();
            WWWForm form = null;
            if (!string.IsNullOrEmpty(name))
            {
                form = new WWWForm();
                form.AddField("name", name);
            }
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            this.user = client.Deserialize<MUser>();
        }
	}
}