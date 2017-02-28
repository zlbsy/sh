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
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.Master.MTile tileMaster = topMapMaster.tiles[index];
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            if (tile != null)
            {
                Request req = Request.Create("areaId", tile.tile_id);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Stage.ToString(), req );
            }
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}