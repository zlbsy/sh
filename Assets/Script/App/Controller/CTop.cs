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
            yield return StartCoroutine(base.OnLoad(request));
            yield return StartCoroutine(OnLoadEnd());
        }
        private IEnumerator OnLoadEnd(){
            if (Global.SUser.self.GetValue("tutorial") < Global.Constant.tutorial_end)
            {
                TutorialStart();
                yield break;
            }
            yield break;
        }
        private void TutorialStart(){
            SUser sUser = Global.SUser;
            int tutorial = sUser.self.GetValue("tutorial");
            StartCoroutine(sUser.Download(TutorialAsset.TutorialUrl(tutorial), Global.versions.tutorial, (AssetBundle assetbundle)=>{
                TutorialAsset.assetbundle = assetbundle;
                //App.Util.LSharp.LSharpScript.Instance.Analysis(TutorialAsset.Data.tutorial);
                List<string> script = new List<string>();
                //script.Add("Talk.set(1,0,少年，现在开始教学,true);");
                //script.Add("Var.setprogress(tutorial,1);");
                //script.Add("Tutorial.clickmask(SceneTop.UICamera.Canvas.LeftFooter.MapButton,96,96);");
                script.Add("Tutorial.call(SceneTop,GotoWorld);");
                script.Add("Tutorial.wait(SceneWorld);");
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
            mTopMap = new MTopMap();
            mTopMap.MapId = mUser.MapId;
            mTopMap.Tiles = mUser.TopMap.Clone() as App.Model.MTile[];
            vTopMap.BindingContext = mTopMap.ViewModel;
            vTopMap.UpdateView();
            vTopMap.transform.parent.localScale = Vector3.one;
            vTopMap.MoveToPosition();
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