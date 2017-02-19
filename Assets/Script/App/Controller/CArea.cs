﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;
using App.View.Top;


namespace App.Controller{
    public class CArea : CScene {
        [SerializeField]VBaseMap vBaseMap;
        private MBaseMap mBaseMap;
        private int worldId;
        public override IEnumerator OnLoad( Request request ) 
        {  
            worldId = request.Get<int>("worldId");
            InitMap();
            yield break;
        }
        private void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = worldId;
            mBaseMap.Tiles = AreaCacher.Instance.GetAreas(worldId);
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.ResetAll();
            vBaseMap.transform.parent.localScale = Vector3.one;
            //vWorldMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            /*App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mWorldMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.Master.MTile tileMaster = topMapMaster.tiles[index];
            App.Model.MTile tile = System.Array.Find(mWorldMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            if (tile != null)
            {
                Request req = Request.Create("worldId", tile.tile_id);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Area.ToString() );
            }*/
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}