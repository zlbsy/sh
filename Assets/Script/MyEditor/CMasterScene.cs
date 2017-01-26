using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;
using UnityEditor;


namespace App.Controller{
    public class CMasterScene : CScene {
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
        }

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
        }
        IEnumerator CreateScriptableObjectMasterTopMapRun()
        {
            var topMapAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.TopMapAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("top_map"));
            topMapAsset.topMaps = sMaster.responseAll.top_maps;
            topMapAsset.version = sMaster.responseAll.top_maps_v;

            UnityEditor.AssetDatabase.CreateAsset(topMapAsset, string.Format("Assets/Resources/{0}.asset", App.Model.Scriptable.TopMapAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        IEnumerator CreateScriptableObjectMasterTileRun()
        {
            var tileAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.TileAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("tile"));
            tileAsset.tiles = sMaster.responseAll.tiles;
            tileAsset.version = sMaster.responseAll.tiles_v;

            UnityEditor.AssetDatabase.CreateAsset(tileAsset, string.Format("Assets/Resources/{0}.asset", App.Model.Scriptable.TileAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
	}
}