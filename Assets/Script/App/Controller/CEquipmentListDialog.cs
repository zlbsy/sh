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


namespace App.Controller{
    public class CEquipmentListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.user.equipments == null)
            {
                SEquipment sEquipment = new SEquipment();
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.user.equipments = sEquipment.equipments;
            }
            int id = request.Get<int>("id");
            App.Model.Master.MEquipment.EquipmentType equipmentType = request.Get<App.Model.Master.MEquipment.EquipmentType>("equipmentType");
            App.Model.MEquipment[] equipments = System.Array.FindAll(Global.SUser.user.equipments, 
                _=>_.EquipmentType == equipmentType && _.character_id == 0);
            foreach(App.Model.MEquipment equipment in equipments){
                GameObject obj = Instantiate(childItem);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                VEquipmentIcon vEquipmentIcon = obj.GetComponent<VEquipmentIcon>();
                vEquipmentIcon.BindingContext = equipment.ViewModel;
                vEquipmentIcon.ResetAll();
            }
			yield return 0;
        }
        public void EquipmentIconClick(int equipmentId){
            
        }
	}
}