﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
	public class SUser : SBase {
        public MUser self;
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
            this.self = App.Util.Cacher.UserCacher.Instance.Get(response.user.id);
            this.versions = response.versions;
            App.Util.Global.ssid = response.ssid;
        }
        public IEnumerator RequestGet(int id)
        {
            var url = "user/get";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("id", id);
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
        }
        public IEnumerator RequestGet()
        {
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(RequestGet( this.self.id ));
        }
	}
}