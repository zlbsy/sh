using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace App.Controller{
    public class CMasterScene : CScene {
        public override IEnumerator OnLoad( Request req ) 
		{  
			yield return 0;
        }
        #if UNITY_EDITOR
        void OnGUI()
        {
            if (GUI.Button(new Rect(150, 50, 100, 30), "tiles"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterTileRun());
            }
            if (GUI.Button(new Rect(150, 100, 100, 30), "base_maps"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterBaseMapRun());
            }
            if (GUI.Button(new Rect(150, 150, 100, 30), "buildings"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterBuildingRun());
            }
            if (GUI.Button(new Rect(150, 200, 100, 30), "Constant"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterConstantRun());
            }
            if (GUI.Button(new Rect(150, 250, 100, 30), "world"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterWorldRun());
            }
            if (GUI.Button(new Rect(150, 300, 100, 30), "area"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterAreaRun());
            }
            if (GUI.Button(new Rect(150, 350, 100, 30), "character"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterCharacterRun());
            }
            if (GUI.Button(new Rect(150, 400, 100, 30), "face"))
            {
                var faceAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.FaceAsset>();
                UnityEditor.AssetDatabase.CreateAsset(faceAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.FaceAsset.Name));
                UnityEditor.AssetDatabase.Refresh();
            }
            if (GUI.Button(new Rect(150, 450, 100, 30), "horse"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterHorseRun());
            }
            if (GUI.Button(new Rect(150, 500, 100, 30), "clothes"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterClothesRun());
            }
            if (GUI.Button(new Rect(150, 550, 100, 30), "weapon"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterWeaponRun());
            }
            if (GUI.Button(new Rect(150, 600, 100, 30), "skill"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterSkillRun());
            }
            if (GUI.Button(new Rect(150, 650, 100, 30), "stage"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterStageRun());
            }

            if (GUI.Button(new Rect(350, 50, 100, 30), "Prompt"))
            {
                var promptMessageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.PromptMessageAsset>();
                UnityEditor.AssetDatabase.CreateAsset(promptMessageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.PromptMessageAsset.Name));
                UnityEditor.AssetDatabase.Refresh();
            }
            if (GUI.Button(new Rect(350, 100, 100, 30), "Language"))
            {
                var languageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.LanguageAsset>();
                UnityEditor.AssetDatabase.CreateAsset(languageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.LanguageAsset.Name));
                UnityEditor.AssetDatabase.Refresh();
            }
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
            yield return StartCoroutine (sMaster.RequestAll("character"));
            asset.equipments = sMaster.responseAll.horses;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.HorseAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterClothesRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.ClothesAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("character"));
            asset.equipments = sMaster.responseAll.clothes;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ClothesAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterWeaponRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.WeaponAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("character"));
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
        IEnumerator CreateScriptableObjectMasterStageRun()
        {
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.StageAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("stage"));
            asset.stages = sMaster.responseAll.stages;

            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.StageAsset.Name));
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