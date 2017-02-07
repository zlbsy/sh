using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;


namespace App.Controller{
    public class CBuildingDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
		public override IEnumerator OnLoad( ) 
		{  
            yield return StartCoroutine(base.OnLoad());
            App.Model.Master.MBuilding[] builds = BuildingCacher.Instance.GetAll();
            //List<MBuilding> mBuilding = new List<MBuilding>();
            foreach(App.Model.Master.MBuilding build in builds){
                MBuilding mBuilding = new MBuilding();
                GameObject obj = Instantiate(childItem);
                obj.transform.parent = content;
                obj.transform.localScale = Vector3.one;
                VBuildingChild vBuildintChild = obj.GetComponent<VBuildingChild>();
                vBuildintChild.BindingContext = mBuilding.ViewModel;
                mBuilding.Id = build.id;
            }
			yield return 0;
		}
	}
}