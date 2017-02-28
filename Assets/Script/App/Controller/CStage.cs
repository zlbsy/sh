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
    public class CStage : CScene {
        [SerializeField]VBaseMap vBaseMap;
        private MBaseMap mBaseMap;
        private int areaId;
        public override IEnumerator OnLoad( Request request ) 
        {  
            areaId = request.Get<int>("areaId");
            InitMap();
            yield break;
        }
        private void InitMap(){
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = areaId;
            mBaseMap.Tiles = StageCacher.Instance.GetStages(areaId);
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToCenter();
        }
        public void OnClickTile(int index){
            
        }
        public void GotoTop(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}