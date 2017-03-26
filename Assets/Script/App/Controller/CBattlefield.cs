﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Util.Battle;
using App.View.Common;


namespace App.Controller{
    public class CBattlefield : CBaseMap {
        [SerializeField]Text title;
        [SerializeField]VBottomMenu operatingMenu;
        [SerializeField]CBattleCharacterPreviewDialog battleCharacterPreview;
        [SerializeField]GameObject attackTween;
        private int battlefieldId;
        List<int> characterIds;
        public enum BattleMode
        {
            none,
            show_move_tiles,
            moving,
            move_end,
            attacking

        }
        public BattleMode battleMode{ get; set;}
        public BattleManager manager{ get; set;}
        public BattleTilesManager tilesManager{ get; set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            battleCharacterPreview.gameObject.SetActive(false);
            battlefieldId = request.Get<int>("battlefieldId");
            characterIds = request.Get<List<int>>("characterIds");
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public GameObject CreateAttackTween(){
            GameObject obj = GameObject.Instantiate(attackTween);
            obj.SetActive(true);
            return obj;
        }
        protected override void InitMap(){
            App.Model.Master.MBattlefield battlefieldMaster = App.Util.Cacher.BattlefieldCacher.Instance.Get(battlefieldId);
            title.text = App.Util.Language.Get(battlefieldMaster.name);
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = battlefieldMaster.map_id;
            mBaseMap.Tiles = battlefieldMaster.tiles.Clone() as App.Model.MTile[];
            List<MCharacter> characters = new List<MCharacter>();
            for (int i = 0; i < characterIds.Count; i++)
            {
                MCharacter mCharacter = System.Array.Find(App.Util.Global.SUser.self.characters, _=>_.Id==characterIds[i]);
                mCharacter.Belong = Belong.self;
                App.Model.Master.MBattleOwn mBattleOwn = battlefieldMaster.owns[i];
                mCharacter.CoordinateX = mBattleOwn.x;
                mCharacter.CoordinateY = mBattleOwn.y;
                characters.Add(mCharacter);
            }
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys){
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.Belong = Belong.enemy;
                characters.Add(mCharacter);
            }
            mBaseMap.Characters = characters.ToArray();
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
            base.InitMap();
            App.Util.LSharp.LSharpScript.Instance.Analysis(battlefieldMaster.script);
        }

        public override void OnClickTile(int index){
            switch (battleMode)
            {
                case BattleMode.none:
                    manager.ClickNoneNode(index);
                    break;
                case BattleMode.show_move_tiles:
                    manager.ClickMovingNode(index);
                    break;
                case BattleMode.move_end:
                    manager.ClickPhysicalAttackNode(index);
                    break;
            }
        }
        public void OpenOperatingMenu(){
            operatingMenu.Open();
        }
        public void CloseOperatingMenu(){
            operatingMenu.Close(null);
        }
        public void OpenBattleCharacterPreviewDialog(MCharacter mCharacter){
            battleCharacterPreview.gameObject.SetActive(true);
            Request req = Request.Create("character", mCharacter);
            this.StartCoroutine(battleCharacterPreview.OnLoad(req));
        }
        public void HideBattleCharacterPreviewDialog(){
            battleCharacterPreview.Close();
        }
        protected override void InitManager(){
            base.InitManager();
            manager = new BattleManager(this, mBaseMap, vBaseMap);
            tilesManager = new BattleTilesManager(this, mBaseMap, vBaseMap);
        }
        public void AttackToHert(MCharacter mCharacter){
            mCharacter.Target.Action = App.Model.ActionType.hert;
        }
        public void OpenSkillList(){
            
        }
	}
}