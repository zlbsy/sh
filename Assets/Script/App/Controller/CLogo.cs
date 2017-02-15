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
            BaseMapCacher.Instance.Reset(App.Model.Scriptable.BaseMapAsset.Data.baseMaps);
            BuildingCacher.Instance.Reset(App.Model.Scriptable.BuildingAsset.Data.buildings);
            yield return StartCoroutine (App.Util.Global.SUser.RequestGet());
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
        public IEnumerator VersionCheck(MVersion versions)
        {
            Debug.LogError("VersionCheck");
            CScene scene = SceneManager.CurrentScene;
            SUser sUser = Global.SUser;
            CLoadingDialog.SetNextProgress(10f);
            yield return scene.StartCoroutine(sUser.Download(PromptMessageAsset.Url, versions.prompt_message, (AssetBundle assetbundle)=>{
                PromptMessageAsset.assetbundle = assetbundle;
            }));

            CLoadingDialog.SetNextProgress(20f);
            yield return scene.StartCoroutine(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetNextProgress(25f);
            yield return scene.StartCoroutine(sUser.Download(WorldAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                WorldAsset.assetbundle = assetbundle;
                List<App.Model.MTile> tileList = new List<MTile>();
                foreach(App.Model.Master.MWorld world in WorldAsset.Data.worlds){
                    tileList.Add(App.Model.MTile.Create(world.tile_id, world.x, world.y, world.level));
                }
                Global.worlds = tileList.ToArray();
            }));
            CLoadingDialog.SetNextProgress(30f);
            yield return scene.StartCoroutine(sUser.Download(BuildingAsset.Url, versions.building, (AssetBundle assetbundle)=>{
                BuildingAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetNextProgress(40f);
            yield return scene.StartCoroutine(sUser.Download(BaseMapAsset.Url, versions.top_map, (AssetBundle assetbundle)=>{
                BaseMapAsset.assetbundle = assetbundle;
            }));
            CLoadingDialog.SetNextProgress(50f);
            yield return scene.StartCoroutine(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            CLoadingDialog.SetNextProgress(60f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.characterUrl, versions.character, (AssetBundle assetbundle)=>{
                AssetBundleManager.character = assetbundle;
            }, false));
            CLoadingDialog.SetNextProgress(70f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.horseUrl, versions.horse, (AssetBundle assetbundle)=>{
                AssetBundleManager.horse = assetbundle;
            }, false));
            CLoadingDialog.SetNextProgress(80f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.hatUrl, versions.hat, (AssetBundle assetbundle)=>{
                AssetBundleManager.hat = assetbundle;
            }, false));
            CLoadingDialog.SetNextProgress(90f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                AssetBundleManager.map = assetbundle;
            }, false));
            CLoadingDialog.SetNextProgress(95);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.clothesUrl, versions.clothes, (AssetBundle assetbundle)=>{
                AssetBundleManager.clothes = assetbundle;
            }, false));
            CLoadingDialog.SetNextProgress(100f);
            yield return scene.StartCoroutine(sUser.Download(AssetBundleManager.weaponUrl, versions.weapon, (AssetBundle assetbundle)=>{
                AssetBundleManager.weapon = assetbundle;
            }, false));
            yield return 0;
        }
	}
}