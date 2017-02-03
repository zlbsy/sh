using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using UnityEngine.UI;
using App.Util.Cacher;
using Holoville.HOTween;


namespace App.Controller{
	public class CTop : CScene {
        [SerializeField]VTopMap topMap;
        [SerializeField]GameObject buildingMenu;
        private GameObject currentMenu;
		public override IEnumerator OnLoad( ) 
		{  
            yield return StartCoroutine (App.Util.Global.SUser.RequestGet());
            Debug.LogError("App.Util.Global.SUser.user="+App.Util.Global.SUser.user);
            MTopMap mTopMap = new MTopMap();
            mTopMap.MapId = App.Util.Global.SUser.user.map_id;
            mTopMap.Tiles = App.Util.Global.SUser.user.top_map;
            topMap.BindingContext = mTopMap.ViewModel;
            topMap.ResetAll();
            topMap.MoveToCenter();
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
            App.Model.Master.MTopMap topMapMaster = TopMapCacher.Instance.Get(App.Util.Global.SUser.user.map_id);
            int x = index % topMapMaster.width;
            int y = Mathf.FloorToInt(index / topMapMaster.height);
            App.Model.Master.MTile tileMaster = topMapMaster.tiles[index];
            App.Model.MTile tile = System.Array.Find(App.Util.Global.SUser.user.top_map, _=>_.x == x && _.y == y);
            if (tile == null)
            {
                OpenMenu(buildingMenu);
            }
        }
        private void OpenMenu(GameObject menu){
            currentMenu = menu;
            HOTween.To(menu.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,100f)));
        }
        public void CloseMenu(){
            HOTween.To(currentMenu.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector3(0f,0f)));
        }
	}
}