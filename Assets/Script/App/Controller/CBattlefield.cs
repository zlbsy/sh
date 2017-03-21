using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Util.Battle;


namespace App.Controller{
    public class CBattlefield : CBaseMap {
        [SerializeField]Text title;
        private int battlefieldId;
        private MCharacter[] owns;
        List<int> characterIds;
        public enum BattleMode
        {
            none,
            show_move_tiles,

        }
        private BattleMode _battleMode;
        public BattleMode battleMode{ get; set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            battlefieldId = request.Get<int>("battlefieldId");
            characterIds = request.Get<List<int>>("characterIds");
            owns = System.Array.FindAll(App.Util.Global.SUser.self.characters, _=>characterIds.IndexOf(_.Id) >= 0);
            yield return this.StartCoroutine(base.OnLoad(request));
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
                App.Model.Master.MBattleOwn mBattleOwn = battlefieldMaster.owns[i];
                mCharacter.CoordinateX = mBattleOwn.x;
                mCharacter.CoordinateY = mBattleOwn.y;
                characters.Add(mCharacter);
            }
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys){
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                characters.Add(mCharacter);
            }
            //MCharacter[] characters = new MCharacter[owns.Length + enemys.Count];

            //owns.CopyTo(characters, 0);
            //enemys.CopyTo(characters, owns.Length);
            mBaseMap.Characters = characters.ToArray();
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
            BattleManager.Init(this);
            App.Util.LSharp.LSharpScript.Instance.Analysis(battlefieldMaster.script);
        }

        public override void OnClickTile(int index){
            switch (battleMode)
            {
                case BattleMode.none:
                    BattleManager.ClickNodeMode(index);
                    break;
            }
            if (battleMode == BattleMode.none)
            {
            }
            //vBaseMap.SetTilesColor(index);
            return;
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            List<VCharacter> vCharacters = vBaseMap.Characters;
            VCharacter vCharacter = vBaseMap.Characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            Debug.LogError("OnClickTile vCharacter="+vCharacter);
            if (vCharacter != null)
            {
                base.OnClickTile(index);
            }
        }
        public override void OnClickTile(App.Model.MTile tile){
            
        }
        public void OnDestroy(){
            BattleManager.Destory();
        }
	}
}