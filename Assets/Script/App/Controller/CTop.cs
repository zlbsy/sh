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
                /*script.Add("Talk.set(4100,0,@player_name，起床了，太阳都晒到屁股了！,false);");
                script.Add("Talk.setplayer(@player_id,0,......,true);");
                script.Add("Talk.setplayer(@player_id,0,哇！！你是谁啊？怎么随便跑到别人家里来了？,true);");
                script.Add("Talk.set(4100,0,哈哈！你果然连我都不认识了啊！,false);");
                script.Add("Talk.setplayer(@player_id,0,果然？什么啊？我认识你？,true);");
                script.Add("Talk.set(4100,0,我问你！你知道自己的身世吗？,false);");
                script.Add("Talk.setplayer(@player_id,0,我......这个......,true);");
                script.Add("Talk.set(4100,0,你竖起耳朵听好了，我接下来说的话，关系到你的命运。,false);");
                script.Add("Talk.setplayer(@player_id,0,怪里怪气的，暂且听一听吧,true);");
                script.Add("Talk.set(4100,0,你是星宿下凡，原本是九天玄女娘娘的大弟子——青衣。,false);");
                script.Add("Talk.setplayer(@player_id,0,等等！说什么鬼话？那我不就是神仙了？,true);");
                script.Add("Talk.set(4100,0,不许插嘴！好好听我说！,false);");
                script.Add("Talk.setplayer(@player_id,0,哦，但是九天玄女是谁啊？,true);");
                script.Add("Talk.set(4100,0,闭嘴！,false);");
                script.Add("Talk.setplayer(@player_id,0,......,true);");
                script.Add("Talk.set(4100,0,九天玄女娘娘是上古神仙，娘娘命你看守镇魔塔，你因贪睡，放走了镇魔塔里的数百只妖星魔星，娘娘才罚你下界，让你抓捕放走的妖星魔星！,false);");
                script.Add("Talk.setplayer(@player_id,0,......,true);");
                script.Add("Talk.set(4100,0,你发什么呆啊，我说完了！,false);");
                script.Add("Talk.setplayer(@player_id,0,继续编啊！,true);");
                script.Add("Talk.set(4100,0,谁编了！我说的都是事实！,false);");
                script.Add("Talk.setplayer(@player_id,0,我既然是神仙，怎么不会法术呢？,true);");
                script.Add("Talk.set(4100,0,那是因为你是被贬下界，所以法力都被封起来了！,false);");
                script.Add("Talk.setplayer(@player_id,0,那我怎么抓那些妖星魔星啊？,true);");
                script.Add("Talk.set(4100,0,这我帮不了你，你要自己想办法，现在给你第一个任务！,false);");
                script.Add("Talk.setplayer(@player_id,0,还有其他任务？,true);");*/
                script.Add("Talk.set(4100,0,别废话，跟我来，点一下这里！,false);");
                script.Add("Tutorial.clickmask(SceneTop.UICamera.Canvas.LeftFooter.MapButton,0,0,96,96);");
                script.Add("Tutorial.call(SceneTop,GotoWorld);");
                script.Add("Tutorial.wait(SceneWorld);");
                script.Add("Tutorial.camerato(1);");
                script.Add("Wait.time(0.4);");
                script.Add("Talk.setplayer(@player_id,0,你这是要带我去哪儿？,true);");
                script.Add("Talk.set(4100,0,现在八十万禁军教头王进遇到了些麻烦，你必须赶过去帮忙，快点儿！,false);");
                script.Add("Tutorial.clickmask3d(SceneWorld.3DPanel.BaseMap.Tile_6_5,-50,-50,100,100);");
                script.Add("Tutorial.call(SceneWorld,OnClickTutorialTile);");
                script.Add("Tutorial.wait(SceneArea);");
                script.Add("Character.add(1,stand,right,4,3,true);");
                script.Add("Character.add(2,stand,left,5,3,true);");
                script.Add("Tutorial.camerato(1);");
                script.Add("Wait.time(0.4);");
                /*script.Add("Talk.set(109,0,（可恶的山贼！我倒是不怕他们，但是他们一起上的话，恐怕保护不了母亲大人，怎么办......？）,false);");
                script.Add("Talk.set(4100,0,看到前面了吗？王教头母子正被山贼围攻，你快去救他们！,false);");
                script.Add("Talk.setplayer(@player_id,0,开什么玩笑？我又不会武功，上去不是多送一条命啊！,true);");
                script.Add("Talk.set(4100,0,打架的本能总是有的吧。王教头！我来帮你！,false);");
                script.Add("Talk.setplayer(@player_id,0,啊——！别推我啊！,true);");
                script.Add("Talk.set(5200,0,来了个不怕死的黄毛小子！把他一起干掉！,false);");*/
                script.Add("Talk.setplayer(@player_id,0,可恶！只能硬着头皮上了！,true);");
                script.Add("Battle.start(1);");
                script.Add("Tutorial.wait(ReadyBattleDialog(Clone));");
                script.Add("Wait.time(0.4);");
                script.Add("Talk.set(4100,0,选择出战的武将吧，没错就是你自己了！,false);");
                script.Add("Tutorial.clickmask(ReadyBattleDialog(Clone).Panel.Scroll View.Viewport.Content.CharacterIcon(Clone),0,0,120,120);");
                script.Add("Tutorial.call(ReadyBattleDialog(Clone).Panel.Scroll View.Viewport.Content.CharacterIcon(Clone),ClickChild);");
                script.Add("Tutorial.wait(ReadyBattleDialog(Clone).Panel.SelectCharacterContent.shadowChild(Clone).CharacterIcon(Clone));");
                script.Add("Tutorial.clickmask(ReadyBattleDialog(Clone).Panel.Battle,0,0,96,96);");
                script.Add("Tutorial.call(ReadyBattleDialog(Clone),BattleStart);");
                script.Add("Tutorial.wait(SceneBattlefield);");

                /*
                //script.Add("Talk.set(1,0,少年，现在开始教学,true);");
                //script.Add("Var.setprogress(tutorial,1);");
                script.Add("Tutorial.clickmask(SceneTop.UICamera.Canvas.LeftFooter.MapButton,0,0,96,96);");
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
                */
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
        protected override void InitMap(){
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
        public override void OnClickTile(int index){
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