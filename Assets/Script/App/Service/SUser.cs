using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    /**
     * Master数据每次检索更新版本号，只获取需要更新的数据AssetBundle
     * 数据保存格式为ScriptableObject
    */
	public class SUser : SBase {
        public MUser user;
        public MVersion versions;
        public SUser(){
		}
        public class ResponseAll : ResponseBase
		{
            public MUser user;
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
        public IEnumerator VersionCheck(MVersion versions)
        {yield break;
            Debug.LogError("VersionCheck");
            if (App.Model.Scriptable.TileAsset.Data == null || App.Model.Scriptable.TileAsset.Data.version < versions.tile)
            {
                Debug.LogError("TileAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(Download(App.Model.Scriptable.TileAsset.Url, App.Model.Scriptable.TileAsset.Path));
                App.Model.Scriptable.TileAsset.Clear();
            }
            if (App.Model.Scriptable.TopMapAsset.Data == null || App.Model.Scriptable.TopMapAsset.Data.version < versions.top_map)
            {
                Debug.LogError("TopMapAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(Download(App.Model.Scriptable.TopMapAsset.Url, App.Model.Scriptable.TopMapAsset.Path));
                App.Model.Scriptable.TopMapAsset.Clear();
            }
            yield return 0;
		}
	}
}