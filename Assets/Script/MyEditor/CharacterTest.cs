using UnityEngine;
using System.Collections;
using App.Model;
using App.Service;
using App.View;
using App.Model.Avatar;
using System.Collections.Generic;
using App.Model.Scriptable;
using App.Util;
using App.Util.Cacher;
using App.View.Character;

namespace MyEditor
{
    public class CharacterTest : App.Controller.Common.CBase
    {
        [SerializeField]GameObject characterPrefab;
        [SerializeField]Canvas layer;
        private bool loadComplete = false;
        // Use this for initialization
        // Update is called once per frame
        void Update()
        {
	
        }

        VCharacter view;
        MCharacter model;

        void OnGUI()
        {
            if (!loadComplete)
            {
                GUI.TextField(new Rect(100, 50, 100, 30), "Loading");
                return;
            }
            if (GUI.Button(new Rect(100, 50, 100, 30), "Create"))
            {
                GameObject obj = GameObject.Instantiate(characterPrefab);
                obj.transform.SetParent(layer.transform);
                //obj.transform.localPosition = new Vector3(100f, 100f, 0f);
                obj.SetActive(true);
                //obj.GetComponent<RectTransform> ().localScale = new Vector3(2f,2f,1f);
                model = new MCharacter();
                model.MoveType = MoveType.cavalry;
                model.WeaponType = WeaponType.sword;
                model.Weapon = 4;
                model.Clothes = 5;
                //model.Action = ActionType.attack;
                model.Horse = 1;
                model.Head = 1;
                model.Hat = 1;
                model.StatusInit();
                model.Hp = 100;
                view = obj.GetComponent<VCharacter>();
                view.BindingContext = model.ViewModel;
                model.Action = ActionType.stand;
                characterPrefab.SetActive(false);
            }
            if (GUI.Button(new Rect(50, 100, 50, 30), "Stand"))
            {
                model.Action = ActionType.stand;
            }
            if (GUI.Button(new Rect(100, 100, 50, 30), "Move"))
            {
                model.Action = ActionType.move;
            }
            if (GUI.Button(new Rect(150, 100, 50, 30), "Block"))
            {
                model.Action = ActionType.block;
            }
            if (GUI.Button(new Rect(200, 100, 50, 30), "Hert"))
            {
                this.StartCoroutine(ChangeAction(ActionType.hert));
            }
            if (GUI.Button(new Rect(250, 100, 50, 30), "Attack"))
            {
                this.StartCoroutine(ChangeAction(ActionType.attack));
            }
            if (GUI.Button(new Rect(100, 140, 100, 30), "ChangeHead"))
            {
                if (model.Head == 3)
                {
                    model.Head = 1;
                }
                else
                {
                    model.Head += 1;
                }
            }
            if (GUI.Button(new Rect(100, 180, 100, 30), "ChangeHat"))
            {
                if (model.Hat == 5)
                {
                    model.Hat = 1;
                }
                else
                {
                    model.Hat += 1;
                }
            }
            if (GUI.Button(new Rect(200, 180, 100, 30), "ChangeHorse"))
            {
                if (model.Horse == 2)
                {
                    model.Horse = 1;
                }
                else
                {
                    model.Horse += 1;
                }
            }
            if (GUI.Button(new Rect(100, 220, 100, 30), "ChangeWeapon"))
            {
                if (model.Weapon == 2)
                {
                    model.Weapon = 1;
                }
                else
                {
                    model.Weapon += 1;
                }
            }
            if (GUI.Button(new Rect(200, 220, 100, 30), "ChangeClothes"))
            {
                if (model.Clothes == 2)
                {
                    model.Clothes = 1;
                }
                else
                {
                    model.Clothes += 1;
                }
            }

            if (GUI.Button(new Rect(100, 260, 100, 30), "Damage"))
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-20);
                view.SendMessage(App.Controller.Battle.CharacterEvent.OnDamage.ToString(), arg);
            }
        }

        IEnumerator ChangeAction(ActionType at)
        {
            yield return new WaitForSeconds(0.5f);
            model.Action = at;
        }

        IEnumerator Start()
        {
            Caching.CleanCache();
            SEditorMaster sMaster = new SEditorMaster();
            MVersion versions = new MVersion();
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sMaster.Download(PromptMessageAsset.Url, versions.prompt_message, (AssetBundle assetbundle)=>{
                PromptMessageAsset.assetbundle = assetbundle;
            }));
            list.Add(sMaster.Download(LanguageAsset.WORD_URL, versions.word, (AssetBundle assetbundle)=>{
                LanguageAsset.assetbundle = assetbundle;
                Language.Reset(LanguageAsset.Data.words);
                LanguageAsset.Clear();
            }));
            /*list.Add(sMaster.Download(LanguageAsset.CHARACTER_WORD_URL, versions.characterword, (AssetBundle assetbundle)=>{
                LanguageAsset.assetbundle = assetbundle;
                Language.ResetCharacterWord(LanguageAsset.Data.words);
                LanguageAsset.Clear();
            }));*/
            list.Add(sMaster.Download(WorldAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                WorldAsset.assetbundle = assetbundle;
                Global.worlds = WorldAsset.Data.worlds;
            }));
            list.Add(sMaster.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            list.Add(sMaster.Download(NpcAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                NpcAsset.assetbundle = assetbundle;
                NpcCacher.Instance.Reset(NpcAsset.Data.npcs);
                NpcAsset.Clear();
            }));
            list.Add(sMaster.Download(NpcEquipmentAsset.Url, versions.world, (AssetBundle assetbundle)=>{
                NpcEquipmentAsset.assetbundle = assetbundle;
                NpcEquipmentCacher.Instance.Reset(NpcEquipmentAsset.Data.npc_equipments);
                NpcEquipmentAsset.Clear();
            }));
            list.Add(sMaster.Download(HorseAsset.Url, versions.horse, (AssetBundle assetbundle)=>{
                HorseAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetHorse(HorseAsset.Data.equipments);
                HorseAsset.Clear();
            }));
            list.Add(sMaster.Download(WeaponAsset.Url, versions.weapon, (AssetBundle assetbundle)=>{
                WeaponAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetWeapon(WeaponAsset.Data.equipments);
                WeaponAsset.Clear();
            }));
            list.Add(sMaster.Download(ClothesAsset.Url, versions.clothes, (AssetBundle assetbundle)=>{
                ClothesAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetClothes(ClothesAsset.Data.equipments);
                ClothesAsset.Clear();
            }));
            list.Add(sMaster.Download(AreaAsset.Url, versions.area, (AssetBundle assetbundle)=>{
                AreaAsset.assetbundle = assetbundle;
                AreaCacher.Instance.Reset(AreaAsset.Data.areas);
                AreaAsset.Clear();
            }));
            list.Add(sMaster.Download(ItemAsset.Url, versions.item, (AssetBundle assetbundle)=>{
                ItemAsset.assetbundle = assetbundle;
                ItemCacher.Instance.Reset(ItemAsset.Data.items);
                ItemAsset.Clear();
            }));
            list.Add(sMaster.Download(BattlefieldAsset.Url, versions.stage, (AssetBundle assetbundle)=>{
                BattlefieldAsset.assetbundle = assetbundle;
                BattlefieldCacher.Instance.Reset(BattlefieldAsset.Data.battlefields);
                BattlefieldAsset.Clear();
            }));
            list.Add(sMaster.Download(BuildingAsset.Url, versions.building, (AssetBundle assetbundle)=>{
                BuildingAsset.assetbundle = assetbundle;
                BuildingCacher.Instance.Reset(BuildingAsset.Data.buildings);
                BuildingAsset.Clear();
            }));
            list.Add(sMaster.Download(BaseMapAsset.Url, versions.top_map, (AssetBundle assetbundle)=>{
                BaseMapAsset.assetbundle = assetbundle;
                BaseMapCacher.Instance.Reset(BaseMapAsset.Data.baseMaps);
                BaseMapAsset.Clear();
            }));
            list.Add(sMaster.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle)=>{
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sMaster.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sMaster.Download(App.Model.Avatar.AvatarAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                App.Model.Avatar.AvatarAsset.assetbundle = assetbundle;
            }));
            list.Add(sMaster.Download(ImageAssetBundleManager.avatarUrl, versions.avatar, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.avatar = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.horseUrl, versions.horse_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.horse = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.hatUrl, versions.hat, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.hat = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.clothesUrl, versions.clothes_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.clothes = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.weaponUrl, versions.weapon_img, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.weapon = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.equipmentIconUrl, versions.equipmenticon_icon, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.equipmentIcon = assetbundle;
            }, false));
            list.Add(sMaster.Download(ImageAssetBundleManager.itemIconUrl, versions.item_icon, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.itemIcon = assetbundle;
            }, false));
            Debug.Log("Start");
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(i+"/"+list.Count);
                yield return this.StartCoroutine(list[i]);
            }
            Debug.Log("Start Over");
            loadComplete = true;
        }

    }
}