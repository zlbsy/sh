using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace App.Service{
	public class HttpClient {
		public HttpClient(){
		}
		const string docmain = "http://d.lufylegend.com/";
		string text;
        public IEnumerator Send(string path, WWWForm form = null){
            using (WWW www = (form == null ? new WWW(docmain + path) : new WWW (docmain + path, form))) {
				yield return www;
				if (!string.IsNullOrEmpty (www.error)) {
					Debug.LogError("www Error:" + www.error);
					yield break;
				}
                Debug.Log("HttpClient : " + www.text);
				text = www.text;
			}
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
		/*public T GetResult(){
			//T resultObject = JsonUtility.FromJson <T>(text);
			T resultObject = Deserialize <T>(text);
			return resultObject;
		}*/
	}
}