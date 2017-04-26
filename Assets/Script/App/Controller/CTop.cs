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
    public class CTop : CBaseMap {
        [SerializeField]VBuildingMenu buildingMenu;
        [SerializeField]VStrengthenMenu strengthenMenu;
        [SerializeField]GameObject menuBackground;
        [SerializeField]VHeaderFace headerFace;
        [SerializeField]VHeaderTop headerTop;
        private VBottomMenu currentMenu;
        public override IEnumerator OnLoad( Request request ) 
		{  
            InitHeader();
            InitMap();
            yield return StartCoroutine(base.OnLoad(request));
            yield return StartCoroutine(OnLoadEnd());
        }
        private IEnumerator OnLoadEnd(){
            TutorialStart();
            yield break;
        }
        private void TutorialStart(){
            if (Global.SUser.self.GetValue("tutorial") >= Global.Constant.tutorial_end)
            {
                return;
            }
            SUser sUser = Global.SUser;
            int tutorial = sUser.self.GetValue("tutorial");
            StartCoroutine(sUser.Download(TutorialAsset.TutorialUrl(tutorial), Global.versions.tutorial, (AssetBundle assetbundle)=>{
                TutorialAsset.assetbundle = assetbundle;
                //App.Util.LSharp.LSharpScript.Instance.Analysis(TutorialAsset.Data.tutorial);
                List<string> script = new List<string>();
                //script.Add("Talk.set(1,0,少年，现在开始教学,true);");
                //script.Add("Var.setprogress(tutorial,1);");
                //script.Add("Tutorial.clickmask(SceneTop.UICamera.Canvas.LeftFooter.MapButton,0,0,96,96);");
                script.Add("Tutorial.call(SceneTop,GotoWorld);");
                script.Add("Tutorial.wait(SceneWorld);");
                script.Add("Tutorial.camerato(1);");
                script.Add("Wait.time(0.4);");
                script.Add("Tutorial.clickmask3d(SceneWorld.3DPanel.BaseMap.Tile_6_5,-50,-50,100,100);");
                script.Add("Tutorial.call(SceneWorld,OnClickTutorialTile);");
                script.Add("Tutorial.wait(SceneArea);");
                script.Add("Tutorial.camerato(1);");
                script.Add("Wait.time(0.4);");
                script.Add("Tutorial.clickmask3d(SceneArea.3DPanel.BaseMap.Tile_7_5,-50,-50,100,100);");
                script.Add("Tutorial.call(SceneArea,OnClickTutorialTile);");
                script.Add("Tutorial.wait(SceneStage);");
                //script.Add("Tutorial.call(Scene,OpenCharacterList);");
                ///script.Add("Tutorial.wait(CharacterListDialog(Clone));");
                //script.Add("Talk.set(1,0,武将一览打开了,true);");
                script.Add("Tutorial.close();");
                App.Util.LSharp.LSharpScript.Instance.Analysis(script);
            }));
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
            mBaseMap = new MTopMap();
            mBaseMap.MapId = mUser.MapId;
            mBaseMap.Tiles = mUser.TopMap.Clone() as App.Model.MTile[];
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToPosition();
        }
        public void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
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
            vBaseMap.Camera3DEnable = false;
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
                vBaseMap.Camera3DEnable = true;
                menuBackground.SetActive(false);
            });
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