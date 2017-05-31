﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;
using System.Linq;
using App.Controller.Common;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace App.Controller{
    public class CMasterScene : CScene {
        Dictionary<string, IEnumerator> apis;
        Dictionary<string, IEnumerator> assets;
        Dictionary<string, IEnumerator> scenarios;
        private string type = "";
        private int page = 0;
        private string[] apiKeys;
        private string[] assetsKeys;
        private string[] scenarioKeys;
        public override IEnumerator OnLoad( Request req ) 
		{  
            type = "";
            apis = new Dictionary<string, IEnumerator>();
            assets = new Dictionary<string, IEnumerator>();
            scenarios = new Dictionary<string, IEnumerator>();
            page = 0;
			yield return 0;
        }
        #if UNITY_EDITOR
        void OnGUI()
        {
            if (apis.Count == 0)
            {
                if (GUI.Button(new Rect(50, 100, 200, 30), "Init"))
                {
                    apis.Add("tutorial", CreateScriptableObjectMasterTutorialRun());
                    apis.Add("tiles", CreateScriptableObjectMasterTileRun());
                    apis.Add("base_maps", CreateScriptableObjectMasterBaseMapRun());
                    apis.Add("buildings", CreateScriptableObjectMasterBuildingRun());
                    apis.Add("Constant", CreateScriptableObjectMasterConstantRun());
                    apis.Add("world", CreateScriptableObjectMasterWorldRun());
                    apis.Add("area", CreateScriptableObjectMasterAreaRun());
                    apis.Add("character", CreateScriptableObjectMasterCharacterRun());
                    apis.Add("horse", CreateScriptableObjectMasterHorseRun());
                    apis.Add("clothes", CreateScriptableObjectMasterClothesRun());
                    apis.Add("weapon", CreateScriptableObjectMasterWeaponRun());
                    apis.Add("skill", CreateScriptableObjectMasterSkillRun());
                    apis.Add("battlefield", CreateScriptableObjectMasterBattleFieldRun());
                    apis.Add("item", CreateScriptableObjectMasterItemRun());
                    apis.Add("shop", CreateScriptableObjectMasterShopRun());
                    apis.Add("gacha", CreateScriptableObjectMasterGachaRun());
                    apis.Add("word", CreateScriptableObjectMasterWordRun());
                    apis.Add("npc", CreateScriptableObjectMasterNpcRun());
                    apis.Add("npc_equip", CreateScriptableObjectMasterNpcEquipmentRun());
                    apis.Add("avatar", CreateScriptableObjectMasterAvatarRun());
                    apis.Add("loginbonus", CreateScriptableObjectMasterLoginBonusRun());
                    apis.Add("exp", CreateScriptableObjectMasterExpRun());
                    apiKeys = apis.Keys.ToArray();

                    assets.Add("face", CreateScriptableObjectFaceAssetRun());
                    assets.Add("Prompt", CreateScriptableObjectPromptAssetRun());
                    assets.Add("Language", CreateScriptableObjectLanguageAssetRun());
                    assetsKeys = assets.Keys.ToArray();

                    scenarioKeys = scenarios.Keys.ToArray();
                }
                return;
            }
            if (string.IsNullOrEmpty(type))
            {
                if (GUI.Button(new Rect(50, 100, 200, 30), "apis"))
                {
                    type = "apis";
                    page = 0;
                }
                if (GUI.Button(new Rect(50, 150, 200, 30), "assets"))
                {
                    type = "assets";
                    page = 0;
                }
                if (GUI.Button(new Rect(50, 200, 200, 30), "scenarios"))
                {
                    type = "scenarios";
                    page = 0;
                }
            }
            else if (type == "apis")
            {
                SubGUI(apiKeys);
            }
            else if (type == "assets")
            {
                SubGUI(assetsKeys);
            }
        }
        void SubGUI(string[] keys)
        {
            if (GUI.Button(new Rect(10, 10, 200, 30), "前一页"))
            {
                page--;
            }
            if (GUI.Button(new Rect(220, 10, 200, 30), "下一页"))
            {
                page++;
            }
            if (GUI.Button(new Rect(430, 10, 200, 30), "返回"))
            {
                type = "";
            }
            for (int i = page * 10; i < (page + 1) * 10 && i < keys.Length; i++)
            {
                string key = keys[i];

                if (GUI.Button(new Rect(50, 100 + (i - page * 10) * 50, 200, 30), key))
                {
                    this.StartCoroutine(apis[key]);
                }
            }
        }
        IEnumerator CreateScriptableObjectMasterExpRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.ExpAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("exp"));
            asset.exps = sMaster.responseAll.exps;
            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ExpAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterLoginBonusRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.LoginBonusAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("loginbonus"));
            asset.loginbonuses = sMaster.responseAll.loginbonus;
            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.LoginBonusAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterAvatarRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Avatar.AvatarAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("avatar"));
            App.MyEditor.MAvatar avatar = sMaster.responseAll.avatar;
            asset.cavalry = avatar.cavalry;
            asset.infantry = avatar.infantry;
            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Avatar.AvatarAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectLanguageAssetRun()
        {
            var languageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.LanguageAsset>();
            UnityEditor.AssetDatabase.CreateAsset(languageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.LanguageAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
            yield break;
        }
        IEnumerator CreateScriptableObjectPromptAssetRun()
        {
            var promptMessageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.PromptMessageAsset>();
            UnityEditor.AssetDatabase.CreateAsset(promptMessageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.PromptMessageAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
            yield break;
        }
        IEnumerator CreateScriptableObjectFaceAssetRun()
        {
            var faceAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.FaceAsset>();
            UnityEditor.AssetDatabase.CreateAsset(faceAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.FaceAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
            yield break;
        }
        IEnumerator CreateScriptableObjectMasterTutorialRun()
        {
            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("tutorial"));
            int i = 0;
            foreach(List<string> tutorial in sMaster.responseAll.tutorials){
                var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.TutorialAsset>();
                asset.tutorial = tutorial;
                UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/tutorial/{0}.asset", i++));
                UnityEditor.AssetDatabase.Refresh();
            }

        }
        IEnumerator CreateScriptableObjectMasterNpcRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.NpcAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("npc"));
            asset.npcs = sMaster.responseAll.npcs;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.NpcAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterNpcEquipmentRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.NpcEquipmentAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("npc_equipment"));
            asset.npc_equipments = sMaster.responseAll.npc_equipments;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.NpcEquipmentAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterWordRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.LanguageAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("word"));
            asset.words = sMaster.responseAll.words;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.WordAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterGachaRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.GachaAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("gacha"));
            asset.gachas = sMaster.responseAll.gachas;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.GachaAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterShopRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.ShopAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("shop"));
            asset.shopItems = sMaster.responseAll.shop_items;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ShopAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterItemRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.ItemAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("item"));
            asset.items = sMaster.responseAll.items;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ItemAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterSkillRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.SkillAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("skill"));
            asset.skills = sMaster.responseAll.skills;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.SkillAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterHorseRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.HorseAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("horse"));
            asset.equipments = sMaster.responseAll.horses;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.HorseAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterClothesRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.ClothesAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("clothes"));
            asset.equipments = sMaster.responseAll.clothes;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ClothesAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterWeaponRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.WeaponAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("weapon"));
            asset.equipments = sMaster.responseAll.weapons;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.WeaponAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterCharacterRun()
        {
            var areaAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.CharacterAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("character"));
            areaAsset.characters = sMaster.responseAll.characters;

            UnityEditor.AssetDatabase.CreateAsset(areaAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.CharacterAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterBattleFieldRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.BattlefieldAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("battlefield"));
            asset.battlefields = sMaster.responseAll.battlefields;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.BattlefieldAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterAreaRun()
        {
            var areaAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.AreaAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("area"));
            areaAsset.areas = sMaster.responseAll.areas;

            UnityEditor.AssetDatabase.CreateAsset(areaAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.AreaAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterWorldRun()
        {
            var worldAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.WorldAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("world"));
            worldAsset.worlds = sMaster.responseAll.worlds;

            UnityEditor.AssetDatabase.CreateAsset(worldAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.WorldAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterBaseMapRun()
        {
            var baseMapAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.BaseMapAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("base_map"));
            baseMapAsset.baseMaps = sMaster.responseAll.base_maps;

            UnityEditor.AssetDatabase.CreateAsset(baseMapAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.BaseMapAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterTileRun()
        {
            var tileAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.TileAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("tile"));
            tileAsset.tiles = sMaster.responseAll.tiles;

            UnityEditor.AssetDatabase.CreateAsset(tileAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.TileAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterBuildingRun()
        {
            var buildingAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.BuildingAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("building"));
            buildingAsset.buildings = sMaster.responseAll.buildings;

            UnityEditor.AssetDatabase.CreateAsset(buildingAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.BuildingAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterConstantRun()
        {
            var constantAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.ConstantAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("constant"));
            constantAsset.constant = sMaster.responseAll.constant;

            UnityEditor.AssetDatabase.CreateAsset(constantAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ConstantAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        #endif
	}
}