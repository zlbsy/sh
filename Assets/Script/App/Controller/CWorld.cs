using System.Collections;
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
    /// <summary>
    /// 大地图场景
    /// </summary>
    public class CWorld : CScene {
        [SerializeField]VWorldMap vWorldMap;
        private MWorldMap mWorldMap;
        public override IEnumerator OnLoad( Request request ) 
        {  
            InitMap();
            yield break;
        }
        private void InitMap(){
            mWorldMap = new MWorldMap();
            mWorldMap.MapId = App.Util.Global.Constant.world_map_id;
            mWorldMap.Tiles = App.Util.Global.worlds;
            vWorldMap.BindingContext = mWorldMap.ViewModel;
            vWorldMap.UpdateView();
            vWorldMap.transform.parent.localScale = Vector3.one;
            vWorldMap.MoveToCenter();
        }
        /// <summary>
        /// 点击州府县，进入州府县场景
        /// </summary>
        /// <param name="index">州府县索引</param>
        public void OnClickTile(int index){
            //地图信息
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mWorldMap.MapId);
            //根据索引获取所点击的州府县坐标
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            //根据州府县坐标获取州府县
            App.Model.Master.MWorld tile = System.Array.Find(mWorldMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y) as App.Model.Master.MWorld;
            if (tile != null)
            {
                Request req = Request.Create("worldId", tile.id, "nameKey", tile.Master.name);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Area.ToString(), req );
            }
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}