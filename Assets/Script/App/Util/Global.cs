using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;

namespace App.Util{
	public class Global {
        public static SUser SUser;
        public static string ssid;
        public static SceneManager SceneManager { get; private set; }
        public static void Initialize()
        {
            App.Model.Scriptable.LanguageAsset languageAsset = Resources.Load("Language/Japanese/languageasset") as App.Model.Scriptable.LanguageAsset;
            Language.Reset(languageAsset.words);
            SceneManager = new SceneManager();
            SUser = new SUser();
        }
        public static void ClearChild(GameObject obj)
        {
            var t = obj.transform;
            for ( int i = 0; i< t.childCount; i++) {
                GameObject.Destroy(t.GetChild(i).gameObject);
            }
            t.DetachChildren();    //子要素情報が残っているとテーブルが崩れるため解除処理
        }
	}
}