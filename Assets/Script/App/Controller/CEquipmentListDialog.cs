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
            foreach(App.Model.MEquipment equipment in Global.SUser.user.equipments){
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