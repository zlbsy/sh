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
	public class CTop : CScene {
        [SerializeField]VTopMap vTopMap;
        [SerializeField]VBuildingMenu buildingMenu;
        [SerializeField]VStrengthenMenu strengthenMenu;
        [SerializeField]GameObject menuBackground;
        [SerializeField]VHeaderFace headerFace;
        [SerializeField]VHeaderTop headerTop;
        private VTopMenu currentMenu;
        private MTopMap mTopMap;
        public override IEnumerator OnLoad( Request request ) 
		{  
            MUser mUser = App.Util.Global.SUser.user;
            InitHeader(mUser);
            InitMap(mUser);
        }
        private void InitHeader(MUser mUser){
            headerFace.BindingContext = mUser.ViewModel;
            headerFace.ResetAll();
            headerTop.BindingContext = mUser.ViewModel;
            headerTop.ResetAll();
        }
        private void InitMap(MUser mUser){
            //地图需要判断是否变化，所以另准备一个Model
            mTopMap = new MTopMap();
            mTopMap.MapId = mUser.MapId;
            mTopMap.Tiles = mUser.TopMap.Clone() as App.Model.MTile[];
            vTopMap.BindingContext = mTopMap.ViewModel;
            vTopMap.ResetAll();
            vTopMap.transform.parent.localScale = Vector3.one;
            vTopMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mTopMap.MapId);
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
            }
        }
        public void OpenMenu(VTopMenu menu){
            currentMenu = menu;
            currentMenu.gameObject.SetActive(true);
            vTopMap.Camera3DEnable = false;
            menuBackground.SetActive(true);
            menu.Open();
        }
        public void CloseMenu(){
            CloseMenu(null);
        }
        public void CloseMenu(System.Action complete){
            currentMenu.Close(()=>{
                if(complete != null){
                    complete();
                }
                //currentMenu.gameObject.SetActive(false);
                currentMenu = null;
                vTopMap.Camera3DEnable = true;
                menuBackground.SetActive(false);
            });
        }
        public VTopMap GetVTopMap(){
            return vTopMap;
        }
        public void GotoWorld(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.World.ToString() );
        }
	}
}