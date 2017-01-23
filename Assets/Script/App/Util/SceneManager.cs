using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Util{
	public class SceneManager {

        public static void LoadScene(string name){
            UnityEngine.SceneManagement.SceneManager.LoadScene( name );
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
		}
	}
}