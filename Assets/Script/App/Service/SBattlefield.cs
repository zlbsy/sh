using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
	public class SBattlefield : SBase {
		public MBattlefield battlefield;
		public SBattlefield(){
			
		}
		public class ResponseBattle
		{
			public int id;
			public string id2;
			public MBattlefield battlefield;
		}
		public IEnumerator Request(MonoBehaviour obj)
		{
			var url = "battlefield.php";
			//HttpClient<ResponseBattle> client = new HttpClient<ResponseBattle>();
			HttpClient client = new HttpClient();
			yield return obj.StartCoroutine(client.Send( url));
			battlefield = client.Deserialize<ResponseBattle>().battlefield;
		}
	}
}