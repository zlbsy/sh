using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace App.Service{
	public class HttpClient {
		public HttpClient(){
        }
        public const string docmainBase = "http://d.lufylegend.com/";
        public static string docmain{
            get{
                string gameVersion = "";
                return docmainBase + gameVersion;
            }
        }
		string text;
        public bool isWaiting = false;
        public IEnumerator Send(string path, WWWForm form = null){
            string[] paths = path.Split('/');
            string url = string.Format("{0}sh.php?class={1}&method={2}", docmain, paths[0], paths[1]);
            Debug.Log("url : " + url);
            isWaiting = true;
            if (!string.IsNullOrEmpty(App.Util.Global.ssid))
            {
                if (form == null)
                {
                    form = new WWWForm();
                }
                form.AddField("ssid", App.Util.Global.ssid);
            }
            using (WWW www = (form == null ? new WWW(url) : new WWW (url, form))) {
                yield return www;
				if (!string.IsNullOrEmpty (www.error)) {
                    Debug.LogError("www Error:" + www.error + "\n" + path);
					yield break;
                }
                App.Controller.CLoadingDialog.UpdatePlusProgress(1f);
                Debug.Log("HttpClient : " + www.text);
                ResponseBase response = Deserialize<ResponseBase>(www.text);
                //Debug.LogError("response = " + response);
                //Debug.LogError("response.result = " + response.result + ", response.now = " + response.now);
                if (!response.result)
                {
                    Debug.LogError("Error");
                }
                if (response.user != null)
                {
                    if (App.Util.Global.SUser.user != null)
                    {
                        App.Util.Global.SUser.user.Update(response.user);
                    }
                }
                text = www.text;
                isWaiting = false;
                if (response.now > DateTime.MinValue) 
                {
                    lastReceivedServerTime = response.now;
                    lastReceivedClientTime = DateTime.Now;
                }
			}
        }
        public static string assetBandleURL{
            get{ 
                return docmain + "download/assetbundle/";
            }
        }
        public T Deserialize<T>(string text)
        {
            return (T)Deserialize(text, typeof(T));
        }
		public T Deserialize<T>()
		{
			return (T)Deserialize(text, typeof(T));
		}
		public object Deserialize(string json, Type type)
		{
			object result;

			try
			{
				// デシリアライズ実行
				result = JsonFx.JsonReader.Deserialize(json, type);
			}
			catch (Exception e)
			{
				Debug.Log(json);
				throw e;
			}

			// デコード結果
			return result;
        }
        static private DateTime lastReceivedServerTime;
        static private DateTime lastReceivedClientTime;

        static private TimeSpan TimeSpanClientAndServerTime
        {
            get { return lastReceivedClientTime - lastReceivedServerTime; }
        }
        static public DateTime Now
        {
            get { return DateTime.Now - TimeSpanClientAndServerTime; }
        }
		/*public T GetResult(){
			//T resultObject = JsonUtility.FromJson <T>(text);
			T resultObject = Deserialize <T>(text);
			return resultObject;
		}*/
	}
}