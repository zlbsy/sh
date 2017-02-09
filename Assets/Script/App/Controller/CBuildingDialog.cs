using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;


namespace App.Controller{
    public class CBuildingDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private int tileIndex;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            tileIndex = request.Get<int>("tile_index");
            App.Model.Master.MBuilding[] builds = BuildingCacher.Instance.GetAll(Global.SUser.user.Level);
            List<MBuilding> mBuildings = new List<MBuilding>();
            foreach(App.Model.Master.MBuilding build in builds){
                MBuilding mBuilding = new MBuilding();
                GameObject obj = Instantiate(childItem);
                obj.transform.parent = content;
                obj.transform.localScale = Vector3.one;
                VBuildingChild vBuildintChild = obj.GetComponent<VBuildingChild>();
                vBuildintChild.BindingContext = mBuilding.ViewModel;
                mBuilding.Id = build.id;
                mBuildings.Add(mBuilding);
            }
            BuildingCacher.Instance.SetBuildings(mBuildings);
			yield return 0;
		}
        public void ToBuild(int buildId){
            MBuilding mBuilding = BuildingCacher.Instance.GetBuilding(buildId);
            App.Model.Master.MBuilding buildingMaster = mBuilding.Master;
            VTopMap vTopMap = (App.Util.SceneManager.CurrentScene as CTop).GetVTopMap();
            App.Model.MTile[] tiles = vTopMap.ViewModel.Tiles.Value;
            int currentNum = System.Array.FindAll(tiles, _ => _.tile_id == buildingMaster.tile_id).Length;
            if (currentNum <= buildingMaster.sum)
            {
                if (BuyManager.CanBuy(buildingMaster.price, buildingMaster.price_type))
                {
                    App.Model.Master.MTopMap topMapMaster = TopMapCacher.Instance.Get(vTopMap.ViewModel.MapId.Value);
                    Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(tileIndex);
                    App.Model.MTile currentTile = App.Model.MTile.Create(buildingMaster.tile_id, (int)coordinate.x, (int)coordinate.y);
                    List<App.Model.MTile> tileList = vTopMap.ViewModel.Tiles.Value.ToList();
                    tileList.Add(currentTile);
                    vTopMap.ViewModel.Tiles.Value = tileList.ToArray();
                    this.Close();
                }
            }
            else
            {
                //Confirm dialog
            }
        }
	}
}