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
using App.Util;
using App.View.Common;
using App.Controller.Common;
using App.Model.Scriptable;


namespace App.Controller{
	public class CTop : CScene {
        [SerializeField]VTopMap vTopMap;
        [SerializeField]VBuildingMenu buildingMenu;
        [SerializeField]VStrengthenMenu strengthenMenu;
        [SerializeField]GameObject menuBackground;
        [SerializeField]VHeaderFace headerFace;
        [SerializeField]VHeaderTop headerTop;
        private VBottomMenu currentMenu;
        private MTopMap mTopMap;
        public override IEnumerator OnLoad( Request request ) 
		{  
            InitHeader();
            InitMap();
            yield return this.StartCoroutine(OnLoadEnd());
        }
        private IEnumerator OnLoadEnd(){
            SUser sUser = Global.SUser;
            int tutorial = sUser.self.GetValue("tutorial");
            if (tutorial < Global.Constant.tutorial_end)
            {
                if (Global.tutorials == null)
                {
                    yield return StartCoroutine(sUser.Download(TutorialAsset.Url, Global.versions.tutorial, (AssetBundle assetbundle)=>{
                        TutorialAsset.assetbundle = assetbundle;
                        Global.tutorials = TutorialAsset.Data.tutorials;
                    }));
                }
                TutorialStart();
                yield break;
            }
            yield break;
        }
        private void TutorialStart(){
        }
        private void InitHeader(){
            MUser mUser = App.Util.Global.SUser.self;
            headerFace.BindingContext = mUser.ViewModel;
            headerFace.UpdateView();
            headerTop.BindingContext = mUser.ViewModel;
            headerTop.UpdateView();
        }
        private void InitMap(){
            MUser mUser = App.Util.Global.SUser.self;
            //地图需要判断是否变化，所以另准备一个Model
            mTopMap = new MTopMap();
            mTopMap.MapId = mUser.MapId;
            mTopMap.Tiles = mUser.TopMap.Clone() as App.Model.MTile[];
            vTopMap.BindingContext = mTopMap.ViewModel;
            vTopMap.UpdateView();
            vTopMap.transform.parent.localScale = Vector3.one;
            vTopMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mTopMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
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
        public void OpenMenu(VBottomMenu menu){
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
        public void OpenBattleList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleListDialog));
        }
        public void OpenCharacterList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.CharacterListDialog));
        }
        public void OpenItemList(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ItemListDialog));
        }
	}
}