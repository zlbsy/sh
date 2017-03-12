﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    /**
     * Master数据更新用API
    */
	public class SEditorMaster : SBase {
        public ResponseAll responseAll;
        public SEditorMaster(){
		}
        public class ResponseAll : ResponseBase
        {
            public App.Model.Master.MCharacter[] characters;
            public App.Model.Master.MTile[] tiles;
            public App.Model.Master.MBaseMap[] base_maps;
            public App.Model.Master.MBuilding[] buildings;
            public App.Model.Master.MWorld[] worlds;
            public App.Model.Master.MArea[] areas;
            public App.Model.Master.MStage[] stages;
            public App.Model.Master.MEquipment[] weapons;
            public App.Model.Master.MEquipment[] horses;
            public App.Model.Master.MEquipment[] clothes;
            public App.Model.Master.MSkill[] skills;
            public App.Model.Master.MItem[] items;
            public App.Model.Master.MGacha[] gachas;
            public App.Model.Master.MConstant constant;
		}
        public IEnumerator RequestAll(string type = "")
		{
            var url = "master/alldata";
            WWWForm form = new WWWForm();
            if (string.IsNullOrEmpty(type))
            {
                form.AddField("character", 1);
                form.AddField("tile", 1);
            }
            else
            {
                form.AddField(type, 1);
            }
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            responseAll = client.Deserialize<ResponseAll>();
		}
	}
}