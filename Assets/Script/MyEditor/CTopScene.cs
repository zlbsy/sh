using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;

#if UNITY_EDITOR

namespace App.Controller{
    public class CTopScene : CScene {
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;
        [SerializeField] private VTile tileUnit;
        [SerializeField] private VTopMap topMap;
        [SerializeField] private VWorldMap worldMap;
        [SerializeField] private VBaseMap baseMap;
        private string prefabDir = "Prefab/"; //「Assets/Resources/」以下のprefabファイルの保存先
        public override IEnumerator OnLoad( Request req ) 
		{  
			yield return 0;
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(150, 50, 100, 30), "baseMap"))
            {
                CreateMap(baseMap);
            }
            if (GUI.Button(new Rect(150, 100, 100, 30), "topMap"))
            {
                CreateMap(topMap);
            }
            if (GUI.Button(new Rect(150, 150, 100, 30), "worldMap"))
            {
                CreateMap(worldMap);
            }
        }
        void CreateMap(VBaseMap vBaseMap)
        {
            List<VTile> tiles = new List<VTile>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                    obj.name = "Tile_"+(i + 1)+"_"+(j + 1);
                    obj.transform.parent = vBaseMap.transform;
                    obj.transform.localPosition = new Vector3(j * 0.69f + (i % 2) * 0.345f, -i * 0.617f, 0f);
                    VTile vTile = obj.GetComponent<VTile>();
                    vTile.tileSprite.sprite = null;
                    vTile.buildingSprite.sprite = null;
                    vTile.lineSprite.sprite = null;
                    tiles.Add(vTile);
                }
            }
            vBaseMap.mapWidth = width;
            vBaseMap.mapHeight = height;
            vBaseMap.tileUnits = tiles.ToArray();

            //savePrefab(vBaseMap.gameObject, vBaseMap.gameObject.name);
        }
        void savePrefab(GameObject gameobj, string name) {
            //prefabの保存フォルダパス
            string prefabDirPath = "Assets/" + prefabDir;
            if (!Directory.Exists(prefabDirPath)){
                //prefab保存用のフォルダがなければ作成する
                Directory.CreateDirectory(prefabDirPath);
            }

            //prefabの保存ファイルパス
            string prefabPath = prefabDirPath + name + ".prefab";
            if(!File.Exists(prefabPath)){
                //prefabファイルがなければ作成する
                File.Create(prefabPath);
            }

            //prefabの保存
            UnityEditor.PrefabUtility.CreatePrefab("Assets/" + prefabDir + name + ".prefab", gameobj);
            UnityEditor.AssetDatabase.SaveAssets ();
        }
	}
}
#endif