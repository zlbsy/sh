using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;
using App.Controller.Common;
using App.Model.Scriptable;
using App.Util.Cacher;
using App.Util;

#if UNITY_EDITOR

namespace App.Controller{
    public class CTopScene : CBaseMap {
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;
        [SerializeField] private VTile tileUnit;
        private bool loadComplete = false;
        private App.Model.Master.MBaseMap baseMapMaster;

        private bool createMapOk = false;
        private App.Model.Master.MTile currentTile = null;
        private bool isBuild = false;
        public override IEnumerator OnLoad( Request req ) 
		{  
			yield return 0;
        }
        void OnGUI()
        {
            if (!loadComplete)
            {
                GUI.Label(new Rect(100, 50, 100, 30), "Loading");
                return;
            }
            if (createMapOk)
            {
                GUI.Label(new Rect(100, 50, 100, 30), currentTile.name);
                if (GUI.Button(new Rect(250, 50, 100, 30), "isBuild:"+isBuild))
                {
                    isBuild = !isBuild;
                }
                ChangeCurrentTile();
            }
            else
            {
                width = int.Parse(GUI.TextField(new Rect(50, 10, 50, 30),width.ToString()));
                height = int.Parse(GUI.TextField(new Rect(150, 10, 50, 30),height.ToString()));
                if (GUI.Button(new Rect(50, 50, 100, 30), "createMap"))
                {
                    CreateMap(vBaseMap);
                    createMapOk = true;
                    currentTile = TileCacher.Instance.Get(1);
                }
            }

        }
        private void ChangeCurrentTile(){
            App.Model.Master.MTile[] tiles = TileCacher.Instance.GetAll();
            int i = 0;
            int j = 0;
            foreach(App.Model.Master.MTile tile in tiles){
                if (GUI.Button(new Rect(50 + i*110, 100 + j * 40, 100, 30), tile.name))
                {
                    currentTile = tile;
                }
                j++;
                if (j >= 15)
                {
                    j = 0;
                    i++;
                }
            }
        }
        public override void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = baseMapMaster;
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            //App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            App.View.VTile vTile = this.mapSearch.GetTile(coordinate);
            if (isBuild)
            {
                vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, vTile.TileId, currentTile.id);
            }
            else
            {
                vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, currentTile.id, vTile.BuildingId);
            }
            //OnClickTile(tile);
        }
        /*public override void OnClickTile(App.Model.MTile tile){
            Debug.LogError("OnClickTile");
        }*/
        void CreateMap(VBaseMap vBaseMap)
        {
            List<VTile> tiles = new List<VTile>();
            List<int> tileIds = new List<int>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                    obj.SetActive(true);
                    obj.name = "Tile_"+(i + 1)+"_"+(j + 1);
                    obj.transform.parent = vBaseMap.transform;
                    obj.transform.localPosition = new Vector3(j * 0.69f + (i % 2) * 0.345f, -i * 0.617f, 0f);
                    VTile vTile = obj.GetComponent<VTile>();
                    vTile.tileSprite.sprite = null;
                    vTile.buildingSprite.sprite = null;
                    vTile.lineSprite.sprite = null;
                    //vTile.SetData(1,0,0,1,0);
                    tiles.Add(vTile);
                    tileIds.Add(1);
                }
            }
            vBaseMap.mapWidth = width;
            vBaseMap.mapHeight = height;
            vBaseMap.tileUnits = tiles.ToArray();


            mBaseMap = new MBaseMap();
            mBaseMap.Tiles = new MTile[]{};
            vBaseMap.BindingContext = mBaseMap.ViewModel;

            baseMapMaster = new App.Model.Master.MBaseMap();
            baseMapMaster.width = width;
            baseMapMaster.height = height;
            baseMapMaster.tile_ids = tileIds.ToArray();
            vBaseMap.ResetAll(baseMapMaster);
            vBaseMap.MoveToPosition(0,0, baseMapMaster);
            //savePrefab(vBaseMap.gameObject, vBaseMap.gameObject.name);
            mapSearch = new App.Util.Search.TileMap(mBaseMap, vBaseMap);
        }
        /*void savePrefab(GameObject gameobj, string name) {
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
        }*/

        public override IEnumerator Start()
        {
            Caching.CleanCache();
            Global.Initialize();
            yield return StartCoroutine (base.Start());
            //SEditorMaster sMaster = new SEditorMaster();
            MVersion versions = new MVersion();
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
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
#endif