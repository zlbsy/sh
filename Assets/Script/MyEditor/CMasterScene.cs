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
            if (GUI.Button(new Rect(150, 50, 100, 30), "Create tiles"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterTileRun());
            }
            if (GUI.Button(new Rect(150, 100, 100, 30), "Create top_maps"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterTopMapRun());
            }
            if (GUI.Button(new Rect(150, 150, 100, 30), "Create buildings"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterBuildingRun());
            }
            if (GUI.Button(new Rect(150, 200, 100, 30), "Create Constant"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterConstantRun());
            }

            if (GUI.Button(new Rect(350, 50, 100, 30), "Create Prompt"))
            {
                var promptMessageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.PromptMessageAsset>();
                UnityEditor.AssetDatabase.CreateAsset(promptMessageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.PromptMessageAsset.Name));
                UnityEditor.AssetDatabase.Refresh();
            }
            if (GUI.Button(new Rect(350, 100, 100, 30), "Create Language"))
            {
                var languageAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.LanguageAsset>();
                UnityEditor.AssetDatabase.CreateAsset(languageAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.LanguageAsset.Name));
                UnityEditor.AssetDatabase.Refresh();
            }
        }
        IEnumerator CreateScriptableObjectMasterTopMapRun()
        {
            var topMapAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.TopMapAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("top_map"));
            topMapAsset.topMaps = sMaster.responseAll.top_maps;

            UnityEditor.AssetDatabase.CreateAsset(topMapAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.TopMapAsset.Name));
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