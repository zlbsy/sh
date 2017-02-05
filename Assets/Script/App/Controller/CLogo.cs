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
            StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.LoadingDialog));
            return;
            StartCoroutine(ToLogin( ));
        }
        public IEnumerator ToLogin( ) 
        {  
            yield return StartCoroutine (App.Util.Global.SUser.RequestLogin("aaa", "bbb"));
            if (App.Util.Global.SUser.user == null)
            {
                yield break;
            }
            yield return StartCoroutine(App.Util.Global.SUser.VersionCheck( App.Util.Global.SUser.versions ));
            TileCacher.Instance.Reset(App.Model.Scriptable.TileAsset.Data.tiles);
            TopMapCacher.Instance.Reset(App.Model.Scriptable.TopMapAsset.Data.topMaps);
            BuildingCacher.Instance.Reset(App.Model.Scriptable.BuildingAsset.Data.buildings);
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}