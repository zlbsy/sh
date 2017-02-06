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
        public App.Model.Master.MTile[] tiles;
        public SMaster(){
		}
        public class ResponseAll : ResponseBase
		{
            public int character;
            public int tile;
            public int top_map;
		}
		public IEnumerator RequestAll()
		{
            var url = "master/version";
            //WWWForm form = new WWWForm();
            //form.AddField("character", 1);
            //form.AddField("tile", 1);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url));
            //yield return App.Util.SceneManager.CurrentScene.StartCoroutine(VersionCheck( client.Deserialize<ResponseAll>() ));
        }
        /*public IEnumerator VersionCheck(ResponseAll response)
        {yield break;
            Debug.LogError("VersionCheck");
            if (App.Model.Scriptable.TileAsset.Data == null || App.Model.Scriptable.TileAsset.Data.version < response.tile)
            {
                Debug.LogError("TileAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(Download(App.Model.Scriptable.TileAsset.Url, App.Model.Scriptable.TileAsset.Path));
                App.Model.Scriptable.TileAsset.Clear();
            }
            if (App.Model.Scriptable.TopMapAsset.Data == null || App.Model.Scriptable.TopMapAsset.Data.version < response.top_map)
            {
                Debug.LogError("TopMapAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(Download(App.Model.Scriptable.TopMapAsset.Url, App.Model.Scriptable.TopMapAsset.Path));
                App.Model.Scriptable.TopMapAsset.Clear();
            }
            yield return 0;
		}*/
	}
}