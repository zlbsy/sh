using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;


namespace App.Controller{
    public class CBattlefield : CBaseMap {
		//[SerializeField]GameObject mapLayer;
		//[SerializeField]GameObject characterLayer;
		//[SerializeField]GameObject characterPrefab;
        private int battlefieldId;
        private MCharacter[] owns;
        public override IEnumerator OnLoad( Request request ) 
        {  
            battlefieldId = request.Get<int>("battlefieldId");
            List<int> characterIds = request.Get<List<int>>("characterIds");
            owns = System.Array.FindAll(App.Util.Global.SUser.self.characters, _=>characterIds.IndexOf(_.Id) >= 0);
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected override void InitMap(){
            App.Model.Master.MBattlefield battlefieldMaster = App.Util.Cacher.BattlefieldCacher.Instance.Get(battlefieldId);
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = battlefieldMaster.map_id;
            mBaseMap.Tiles = battlefieldMaster.tiles.Clone() as App.Model.MTile[];
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys){
                
            }
            /*MCharacter[] enemys = App.Service.HttpClient.Deserialize<MCharacter[]>(battlefieldMaster.enemys);
            MCharacter[] characters = new MCharacter[owns.Length + enemys.Length];
            owns.CopyTo(characters, 0);
            enemys.CopyTo(characters, owns.Length);
            mBaseMap.Characters = characters;*/
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
            App.Util.LSharp.LSharpScript.Instance.Analysis(battlefieldMaster.script);
        }
	}
}