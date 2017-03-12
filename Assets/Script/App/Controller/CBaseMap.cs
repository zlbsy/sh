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
    public class CBaseMap : CScene {
        [SerializeField]protected VBaseMap vBaseMap;
        protected MBaseMap mBaseMap;
        public override IEnumerator OnLoad( Request request ) 
        {  
            InitMap();
            yield break;
        }
        protected virtual void InitMap(){
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="index">地图块儿索引</param>
        public virtual void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            OnClickTile(tile);
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="tile">地图块儿</param>
        public virtual void OnClickTile(App.Model.MTile tile){
        }
	}
}