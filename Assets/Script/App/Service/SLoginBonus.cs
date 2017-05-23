using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SLoginBonus : SBase {
        public int count;
        public SLoginBonus(){
        }
        public class ResponseLog : ResponseBase
		{
            public int count;
		}
        public IEnumerator RequestLogCount()
		{
            var url = "loginbonus/log_count";
            //WWWForm form = new WWWForm();
            //form.AddField("user_id", userId);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseLog response = client.Deserialize<ResponseLog>();
            this.count = response.count;
        }
	}
}