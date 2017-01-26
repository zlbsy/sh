using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
    public class CMasterScene : CScene {
		public override IEnumerator OnLoad( ) 
		{  
			yield return 0;
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 50, 100, 30), "Create tiles"))
            {
                this.StartCoroutine(CreateScriptableObjectMasterTileRun());
            }
            if (GUI.Button(new Rect(100, 100, 100, 30), "TileAsset Load"))
            {
                Debug.LogError("TileAsset = " + App.Model.Scriptable.TileAsset.Data);
            }
        }
        IEnumerator CreateScriptableObjectMasterTileRun()
        {
            var tileAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.TileAsset>();

            SEditorMaster sMaster = new SEditorMaster();
            yield return StartCoroutine (sMaster.RequestAll("tile"));
            tileAsset.tiles = sMaster.responseAll.tiles;
            tileAsset.version = sMaster.responseAll.tiles_v;
            UnityEditor.AssetDatabase.CreateAsset(tileAsset, string.Format("Assets/Data/{0}.asset", App.Model.Scriptable.TileAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
	}
}