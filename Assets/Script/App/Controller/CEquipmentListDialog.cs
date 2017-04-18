using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Equipment;
using App.Controller.Common;


namespace App.Controller{
    public class CEquipmentListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.equipments == null)
            {
                SEquipment sEquipment = new SEquipment();
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.self.equipments = sEquipment.equipments;
            }
            int id = request.Get<int>("id");
            App.Model.Master.MEquipment.EquipmentType equipmentType = request.Get<App.Model.Master.MEquipment.EquipmentType>("equipmentType");
            App.Model.MEquipment[] equipments = System.Array.FindAll(Global.SUser.self.equipments, 
                _=>_.EquipmentType == equipmentType && _.character_id == 0);
            foreach(App.Model.MEquipment equipment in equipments){
                if (equipment.Id == id)
                {
                    continue;
                }
                ScrollViewSetChild(content, childItem, equipment);
            }
			yield return 0;
        }
        public void EquipmentIconClick(int equipmentId){
            
        }
	}
}