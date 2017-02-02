using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Util{
    public class SceneManager {
        public enum Prefabs{
            BuildingDialog
        }
        public static App.Controller.CScene CurrentScene;
        public static void LoadScene(string name){
            UnityEngine.SceneManagement.SceneManager.LoadScene( name );
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
		}

        public IEnumerator ShowDialog(Prefabs prefab)
        {

            GameObject instance = LoadPrefab (prefab.ToString());
            //LoadDialog( instance.GetComponent<DialogController>(), request , useBackground);
            //yield return instance;
            yield return 0;
        }
        public GameObject LoadPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load(string.Format("Prefabs/{0}", prefabName)) as GameObject;
            return CurrentScene.GetObject( prefab );
        }
	}
}