using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util{
    public class Language{
        private static Dictionary<string, string> dictionaryDatas = new Dictionary<string, string>();
        public static void Reset(App.Model.Master.MWord[] datas){
            dictionaryDatas.Clear();
            foreach(App.Model.Master.MWord data in datas){
                dictionaryDatas.Add(data.key, data.value);   
            }
        }
        public static string Get(string key){
            string val;
            if (dictionaryDatas.TryGetValue (key, out val))
            {
                return val;
            }
            return key;
        }
    }
}