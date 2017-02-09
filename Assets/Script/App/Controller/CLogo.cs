using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using App.Model.Scriptable;


namespace App.Controller{
    public class CLogo : CScene {
        public override IEnumerator Start()
        {
            Global.Initialize();
            yield return StartCoroutine (base.Start());
        }
        public override IEnumerator OnLoad( Request request ) 
		{  
			yield return 0;
		}
        public void ClearCacher(){
            Caching.CleanCache();
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
            CScene scene = SceneManager.CurrentScene;
            SUser sUser = Global.SUser;
            yield return scene.StartCoroutine(sUser.Download(PromptMessageAsset.Url, versions.prompt_message, (AssetBundle assetbundle)=>{
                PromptMessageAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetProgress(10f);
            yield return scene.StartCoroutine(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetProgress(20f);
            yield return scene.StartCoroutine(sUser.Download(BuildingAsset.Url, versions.building, (AssetBundle assetbundle)=>{
                BuildingAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetProgress(30f);
            yield return scene.StartCoroutine(sUser.Download(TopMapAsset.Url, versions.top_map, (AssetBundle assetbundle)=>{
                TopMapAsset.assetbundle = assetbundle;
            }));
            yield return scene.StartCoroutine(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            CLoadingDialog.SetProgress(40f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.characterUrl, versions.character, (AssetBundle assetbundle)=>{
                AssetBundleManager.character = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(50f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.horseUrl, versions.horse, (AssetBundle assetbundle)=>{
                AssetBundleManager.horse = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(60f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.hatUrl, versions.hat, (AssetBundle assetbundle)=>{
                AssetBundleManager.hat = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(70f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                AssetBundleManager.map = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(80f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.clothesUrl, versions.clothes, (AssetBundle assetbundle)=>{
                AssetBundleManager.clothes = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(90f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.weaponUrl, versions.weapon, (AssetBundle assetbundle)=>{
                AssetBundleManager.weapon = assetbundle;
            }, false));
            CLoadingDialog.SetProgress(100f);
            yield return 0;
        }
	}
}