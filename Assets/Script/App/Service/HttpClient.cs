using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace App.Service{
    public class HttpResponse{
        public DateTime now;
        public bool result = false;
    }
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
            Debug.Log("Send : " + path);
            isWaiting = true;
            if (!string.IsNullOrEmpty(App.Util.Global.ssid))
            {
                if (form == null)
                {
                    form = new WWWForm();
                }
                form.AddField("ssid", App.Util.Global.ssid);
            }
            using (WWW www = (form == null ? new WWW(docmain + path) : new WWW (docmain + path, form))) {
				yield return www;
				if (!string.IsNullOrEmpty (www.error)) {
                    Debug.LogError("www Error:" + www.error + "\n" + path);
					yield break;
				}
                Debug.Log("HttpClient : " + www.text);
                HttpResponse response = Deserialize<HttpResponse>(www.text);
                Debug.LogError("response = " + response.result + ", " + response.now);
                if (!response.result)
                {
                    Debug.LogError("Error");
                }
                text = www.text;
                /*if (text.IndexOf("\"result\":0") >= 0)
                {
                    Debug.LogError("Error");
                }*/
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

        static public TimeSpan TimeSpanClientAndServerTime
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