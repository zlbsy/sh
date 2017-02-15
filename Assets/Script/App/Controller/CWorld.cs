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
            vWorldMap.ResetAll();
            vWorldMap.transform.parent.localScale = Vector3.one;
            //vWorldMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            /*App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mTopMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.Master.MTile tileMaster = topMapMaster.tiles[index];
            App.Model.MTile tile = System.Array.Find(mTopMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            if (tile == null)
            {
                buildingMenu.currentIndex = index;
                OpenMenu(buildingMenu);
            }
            else
            {
                OpenMenu(strengthenMenu);
            }*/
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}