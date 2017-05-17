using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    /**
     * 
    */
    public class SSkill : SBase {
        public MSkill skill;
        public SSkill(){
        }
        public class ResponseLevelUp : ResponseBase
		{
            public MSkill skill;
		}
        public IEnumerator RequestLevelUp(int id)
        {
            var url = "skill/level_up";
            WWWForm form = new WWWForm();
            form.AddField("skill_id", id);
            Debug.Log("RequestLevelUp skill_id="+id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form ));
            ResponseLevelUp response = client.Deserialize<ResponseLevelUp>();
            this.skill = response.skill;
        }
	}
}