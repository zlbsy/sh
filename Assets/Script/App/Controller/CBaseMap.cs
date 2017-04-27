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
using App.Controller.Common;
using System.Linq;


namespace App.Controller{
    public class CBaseMap : CScene {
        [SerializeField]protected VBaseMap vBaseMap;
        protected MBaseMap mBaseMap;
        public App.Util.Search.TileMap mapSearch{ get; set;}
        public App.Util.Search.AStar aStar{ get; set;}
        public App.Util.Search.BreadthFirst breadthFirst{ get; set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            InitMap();
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected virtual void InitManager(){
            mapSearch = new App.Util.Search.TileMap(mBaseMap, vBaseMap);
            aStar = new App.Util.Search.AStar(this, mBaseMap, vBaseMap);
            breadthFirst = new App.Util.Search.BreadthFirst(this, mBaseMap, vBaseMap);
        }
        protected virtual void InitMap(){
            InitManager();
        }
        public virtual void CameraTo(int id){
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, w=>w.id==id);
            vBaseMap.MoveToPosition(tile.x, tile.y);
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="index">地图块儿索引</param>
        public virtual void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.Model.MTile tile = System.Array.Find(mBaseMap.Tiles, _=>_.x == coordinate.x && _.y == coordinate.y);
            OnClickTile(tile);
        }
        /// <summary>
        /// 点击地图块儿
        /// </summary>
        /// <param name="tile">地图块儿</param>
        public virtual void OnClickTile(App.Model.MTile tile){
        }
        public VBaseMap GetVBaseMap(){
            return vBaseMap;
        }
        public MBaseMap GetMBaseMap(){
            return mBaseMap;
        }
        public virtual void OnDestroy(){
        }
        public App.View.Character.VCharacter GetCharacterView(MCharacter mCharacter){
            App.View.Character.VCharacter vCharacter = this.vBaseMap.Characters.Find(_=>_.ViewModel.CharacterId.Value == mCharacter.CharacterId && _.ViewModel.Belong.Value == mCharacter.Belong);
            return vCharacter;
        }
        public MCharacter GetCharacterModel(App.View.Character.VCharacter vCharacter){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.CharacterId == vCharacter.ViewModel.CharacterId.Value && _.Belong == vCharacter.ViewModel.Belong.Value);
            return mCharacter;
        }
        public void addCharacter(int npcId, ActionType action, string direction, int x, int y){
            MCharacter mCharacter = NpcCacher.Instance.GetFromNpc(npcId);
            mCharacter.Action = action;
            mCharacter.CoordinateX = x;
            mCharacter.CoordinateY = y;
            mCharacter.Direction = (Direction)System.Enum.Parse(typeof(Direction), direction, true);
            /*
            mCharacter.CharacterId = characterId;
            mCharacter.MoveType = MoveType.cavalry;
            mCharacter.WeaponType = WeaponType.longKnife;
            mCharacter.Weapon = 1;
            mCharacter.Clothes = 1;
            mCharacter.Horse = 1;
            mCharacter.Head = 1;
            mCharacter.Hat = 1;
            */
            List<MCharacter> characters = mBaseMap.Characters == null ? new List<MCharacter>() : mBaseMap.Characters.ToList();
            characters.Add(mCharacter);
            mBaseMap.Characters = characters.ToArray();
        }
	}
}