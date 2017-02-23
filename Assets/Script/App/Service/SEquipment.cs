﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SEquipment : SBase {
        public MEquipment[] equipments;
        public SEquipment(){
        }
        public class ResponseList : ResponseBase
		{
            public MEquipment[] equipments;
		}
        public IEnumerator RequestList()
		{
            var url = "equipment/equipment_list";
            //WWWForm form = new WWWForm();
            //form.AddField("user_id", userId);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseList response = client.Deserialize<ResponseList>();
            this.equipments = response.equipments;
        }
	}
}