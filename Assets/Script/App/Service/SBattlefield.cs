using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
	public class SBattlefield : SBase {
        public MBattleChild[] battlelist;
		public SBattlefield(){
			
		}
		public class ResponseBattleList
		{
            public MBattleChild[] battlelist;
        }
        public IEnumerator RequestBattlelist()
        {
            var url = "battle/battle_list";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseBattleList response = client.Deserialize<ResponseBattleList>();
            battlelist = response.battlelist;
        }
	}
}