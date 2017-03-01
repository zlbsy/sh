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
using System.Linq;


namespace App.Controller{
    public class CStage : CScene {
        [SerializeField]VBaseMap vBaseMap;
        private MBaseMap mBaseMap;
        private App.Model.Master.MArea area;
        public override IEnumerator OnLoad( Request request ) 
        {  
            Debug.LogError("CStage OnLoad");
            area = request.Get<App.Model.Master.MArea>("area");
            InitMap();
            yield break;
        }
        private void InitMap(){
            Debug.LogError("CStage InitMap");
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = area.map_id;
            mBaseMap.Tiles = area.stages;
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            
        }
        public void GotoArea(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Area.ToString() );
        }
        public void addCharacter(int characterId, ActionType action, string direction, int x, int y){
            MCharacter mCharacter = new MCharacter();
            mCharacter.CharacterId = characterId;
            mCharacter.Action = action;
            mCharacter.X = x;
            mCharacter.Y = y;
            List<MCharacter> characters = mBaseMap.Characters.ToList();
            characters.Add(mCharacter);
            mBaseMap.Characters = characters.ToArray();
        }
	}
}