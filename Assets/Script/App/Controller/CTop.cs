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
        private VTopMenu currentMenu;
        private MTopMap mTopMap;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine (App.Util.Global.SUser.RequestGet());
            mTopMap = new MTopMap();
            mTopMap.MapId = App.Util.Global.SUser.user.map_id;
            vTopMap.BindingContext = mTopMap.ViewModel;
            mTopMap.Tiles = App.Util.Global.SUser.user.top_map.Clone() as App.Model.MTile[];
            //vTopMap.ResetAll();
            vTopMap.transform.parent.localScale = Vector3.one;
            vTopMap.MoveToCenter();
            /*SMaster sMaster = new SMaster();
            yield return StartCoroutine (sMaster.RequestAll());
            App.Model.Master.MCharacter[] characters = sMaster.characters;
            App.Util.Cacher.CharacterCacher.Instance.Reset(sMaster.characters);
            yield break;
            foreach (App.Model.Master.MCharacter character in characters)
            {
                Debug.Log("character.id=" + App.Util.Cacher.CharacterCacher.Instance.Get(character.id));
            }
            */
			yield return 0;
        }
        public void OnClickTile(int index){
            App.Model.Master.MTopMap topMapMaster = TopMapCacher.Instance.Get(mTopMap.MapId);
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
	}
}