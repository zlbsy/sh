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
    public class CArea : CBaseMap {
        [SerializeField]Text title;
        private int worldId;
        public override IEnumerator OnLoad( Request request ) 
        {  
            worldId = request.Get<int>("worldId");
            string nameKey = request.Get<string>("nameKey");
            title.text = App.Util.Language.Get(nameKey);
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected override void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = worldId;
            mBaseMap.Tiles = AreaCacher.Instance.GetAreas(worldId);
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
        }
        public override void OnClickTile(App.Model.MTile tile){
            App.Model.Master.MArea area = tile as App.Model.Master.MArea;
            if (area != null)
            {
                Request req = Request.Create("area", area);
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Stage.ToString(), req );
            }
        }
        public void GotoWorld(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.World.ToString() );
        }
	}
}