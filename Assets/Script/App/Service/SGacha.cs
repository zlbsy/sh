using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using System.Linq;


namespace App.Service{
    public class SGacha : SBase {
        public App.Model.MGacha[] gachas;
        public SGacha(){
        }
        public class ResponseFreeLog : ResponseBase
        {
            public App.Model.MGacha[] gachas;
        }
        public IEnumerator RequestFreeLog()
        {
            var url = "gacha/freelog";
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url ));
            ResponseFreeLog response = client.Deserialize<ResponseFreeLog>();
            this.gachas = response.gachas;
        }
        public IEnumerator RequestGacha(int gacha_id, int cnt)
        {
            var url = "shop/buy";
            WWWForm form = new WWWForm();
            form.AddField("gacha_id", gacha_id);
            form.AddField("cnt", cnt);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));

        }
    }
}