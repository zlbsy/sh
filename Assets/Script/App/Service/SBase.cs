using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace App.Service{
	public class SBase {
        public IEnumerator Download (string url, string path)
        //public IEnumerator Download (string url, int ver)
        {
            /*var www = WWW.LoadFromCacheOrDownload(url, ver);
            yield return www;
            www.assetBundle.Unload(false);
            Debug.LogError("www.assetBundle="+www.assetBundle);*/
            using (WWW www = new WWW (url + "?time=" + System.DateTime.Now.GetHashCode())) {

                yield return www;

                File.Delete (path);
                File.WriteAllBytes (path, www.bytes);
            }
            yield return 0;
        }
	}
}