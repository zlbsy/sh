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
using App.View.Character;
using App.Controller.Common;


namespace App.Controller{
    public class CStage : CBaseMap {
        [SerializeField]Text title;
        private App.Model.Master.MArea area;
        public Request saveRequest{ get;set;}
        public override IEnumerator OnLoad( Request request ) 
        {  
            area = request.Get<App.Model.Master.MArea>("area");
            this.saveRequest = request;
            title.text = App.Util.Language.Get(area.Master.name);
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected override void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = area.map_id;
            mBaseMap.Characters = new MCharacter[]{};
            mBaseMap.Tiles = area.stages;
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToPosition();
            base.InitMap();
            App.Util.LSharp.LSharpScript.Instance.Analysis(new List<string>{string.Format("Load.script({0})", area.tile_id)});
        }
        public override void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            List<VCharacter> vCharacters = vBaseMap.Characters;
            VCharacter vCharacter = vBaseMap.Characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            Debug.LogError("OnClickTile vCharacter="+vCharacter);
            if (vCharacter != null)
            {
                App.Util.LSharp.LSharpScript.Instance.Analysis(new List<string>{string.Format("Call.characterclick_{0}();", vCharacter.ViewModel.CharacterId.Value)});
            }
        }
        public void GotoArea(){
            Request req = Request.Create("worldId", saveRequest.Get<int>("worldId"), "nameKey", saveRequest.Get<string>("nameKey"));
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Area.ToString(), req );
        }
	}
}