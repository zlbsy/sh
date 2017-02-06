using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;


namespace App.Controller{
    public class CLogo : CScene {
        public override IEnumerator Start()
        {
            Global.Initialize();
            yield return StartCoroutine (base.Start());
        }
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
		}
        public void GameStart(){
            CConnectingDialog.ToShow();
            StartCoroutine(ToLogin( ));
        }
        public IEnumerator ToLogin( ) 
        {  
            /*StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ConnectingDialog));
            while (Global.SceneManager.CurrentDialog == null)
            {
                yield return 0;
            }
            yield break;
            CLoadingDialog dialog = Global.SceneManager.CurrentDialog as CLoadingDialog;
            dialog.Progress = 30.66f;
            yield break;
            */

            yield return StartCoroutine (App.Util.Global.SUser.RequestLogin("aaa", "bbb"));
            CConnectingDialog.ToClose();
            if (App.Util.Global.SUser.user == null)
            {
                yield break;
            }
            CLoadingDialog.ToShow();
            yield return StartCoroutine(VersionCheck( App.Util.Global.SUser.versions ));
            TileCacher.Instance.Reset(App.Model.Scriptable.TileAsset.Data.tiles);
            TopMapCacher.Instance.Reset(App.Model.Scriptable.TopMapAsset.Data.topMaps);
            BuildingCacher.Instance.Reset(App.Model.Scriptable.BuildingAsset.Data.buildings);
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
        public IEnumerator VersionCheck(MVersion versions)
        {
            Debug.LogError("VersionCheck");
            if (App.Model.Scriptable.TileAsset.Data == null || App.Model.Scriptable.TileAsset.Data.version < versions.tile)
            {
                Debug.LogError("TileAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(App.Util.Global.SUser.Download(App.Model.Scriptable.TileAsset.Url, App.Model.Scriptable.TileAsset.Path));
                App.Model.Scriptable.TileAsset.Clear();
            }
            /*
            CLoadingDialog.SetProgress(20f);
            if (App.Model.Scriptable.BuildingAsset.Data == null || App.Model.Scriptable.BuildingAsset.Data.version < versions.building)
            {
                Debug.LogError("BuildingAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(App.Util.Global.SUser.Download(App.Model.Scriptable.TileAsset.Url, App.Model.Scriptable.BuildingAsset.Path));
                App.Model.Scriptable.TileAsset.Clear();
            }
            CLoadingDialog.SetProgress(40f);
            if (App.Model.Scriptable.PromptMessageAsset.Data == null || App.Model.Scriptable.PromptMessageAsset.Data.version < versions.prompt_message)
            {
                Debug.LogError("TopMapAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(App.Util.Global.SUser.Download(App.Model.Scriptable.TopMapAsset.Url, App.Model.Scriptable.TopMapAsset.Path));
                App.Model.Scriptable.TopMapAsset.Clear();
            }
            CLoadingDialog.SetProgress(40f);
            if (App.Model.Scriptable.TopMapAsset.Data == null || App.Model.Scriptable.TopMapAsset.Data.version < versions.top_map)
            {
                Debug.LogError("TopMapAsset Download");
                yield return App.Util.SceneManager.CurrentScene.StartCoroutine(App.Util.Global.SUser.Download(App.Model.Scriptable.TopMapAsset.Url, App.Model.Scriptable.TopMapAsset.Path));
                App.Model.Scriptable.TopMapAsset.Clear();
            }*/
            CLoadingDialog.SetProgress(100f);
            yield return 0;
        }
	}
}