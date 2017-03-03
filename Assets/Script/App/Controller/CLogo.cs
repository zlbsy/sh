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
            yield return StartCoroutine (App.Util.Global.SUser.RequestLogin("aaa", "bbb"));
            CConnectingDialog.ToClose();
            if (App.Util.Global.SUser.user == null)
            {
                yield break;
            }
            CLoadingDialog.ToShow();
            yield return StartCoroutine(VersionCheck( App.Util.Global.SUser.versions ));
            yield return StartCoroutine (App.Util.Global.SUser.RequestGet());
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
        public IEnumerator VersionCheck(MVersion versions)
        {
            CScene scene = SceneManager.CurrentScene;
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(PromptMessageAsset.Url, versions.prompt_message, (AssetBundle assetbundle)=>{
                PromptMessageAsset.assetbundle = assetbundle;
            }));
            list.Add(sUser.Download(LanguageAsset.WORD_URL, versions.word, (AssetBundle assetbundle)=>{
                LanguageAsset.assetbundle = assetbundle;
                Language.Reset(LanguageAsset.Data.words);
                LanguageAsset.Clear();
            }));
            list.Add(sUser.Download(LanguageAsset.CHARACTER_WORD_URL, versions.characterword, (AssetBundle assetbundle)=>{
                LanguageAsset.assetbundle = assetbundle;
                Language.ResetCharacterWord(LanguageAsset.Data.words);
                LanguageAsset.Clear();
            }));
            list.Add(sUser.Download(WorldAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                WorldAsset.assetbundle = assetbundle;
                Global.worlds = WorldAsset.Data.worlds;
            }));
            list.Add(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            list.Add(sUser.Download(HorseAsset.Url, versions.horse, (AssetBundle assetbundle)=>{
                HorseAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetHorse(HorseAsset.Data.equipments);
                HorseAsset.Clear();
            }));
            list.Add(sUser.Download(WeaponAsset.Url, versions.weapon, (AssetBundle assetbundle)=>{
                WeaponAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetWeapon(WeaponAsset.Data.equipments);
                WeaponAsset.Clear();
            }));
            list.Add(sUser.Download(ClothesAsset.Url, versions.clothes, (AssetBundle assetbundle)=>{
                ClothesAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetClothes(ClothesAsset.Data.equipments);
                ClothesAsset.Clear();
            }));
            list.Add(sUser.Download(AreaAsset.Url, versions.area, (AssetBundle assetbundle)=>{
                AreaAsset.assetbundle = assetbundle;
                AreaCacher.Instance.Reset(AreaAsset.Data.areas);
                AreaAsset.Clear();
            }));
            list.Add(sUser.Download(ItemAsset.Url, versions.item, (AssetBundle assetbundle)=>{
                ItemAsset.assetbundle = assetbundle;
                ItemCacher.Instance.Reset(ItemAsset.Data.items);
                ItemAsset.Clear();
            }));
            /*list.Add(sUser.Download(StageAsset.Url, versions.stage, (AssetBundle assetbundle)=>{
                StageAsset.assetbundle = assetbundle;
                StageCacher.Instance.Reset(StageAsset.Data.stages);
                StageAsset.Clear();
            }));*/
            list.Add(sUser.Download(BuildingAsset.Url, versions.building, (AssetBundle assetbundle)=>{
                BuildingAsset.assetbundle = assetbundle;
                BuildingCacher.Instance.Reset(BuildingAsset.Data.buildings);
                BuildingAsset.Clear();
            }));
            list.Add(sUser.Download(BaseMapAsset.Url, versions.top_map, (AssetBundle assetbundle)=>{
                BaseMapAsset.assetbundle = assetbundle;
                BaseMapCacher.Instance.Reset(BaseMapAsset.Data.baseMaps);
                BaseMapAsset.Clear();
            }));
            list.Add(sUser.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle)=>{
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sUser.Download(App.Model.Avatar.AvatarAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                App.Model.Avatar.AvatarAsset.assetbundle = assetbundle;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.avatarUrl, versions.avatar, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.avatar = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.horseUrl, versions.horse_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.horse = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.hatUrl, versions.hat, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.hat = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.clothesUrl, versions.clothes_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.clothes = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.weaponUrl, versions.weapon_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.weapon = assetbundle;
            }, false));
            list.Add(sUser.Download(ImageAssetBundleManager.equipmentIconUrl, versions.equipmenticon_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.equipmentIcon = assetbundle;
            }, false));
            float step = 100f / list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                CLoadingDialog.SetNextProgress((i + 1) * step);
                yield return scene.StartCoroutine(list[i]);
            }
            yield return 0;
        }
	}
}