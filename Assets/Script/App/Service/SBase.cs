using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace App.Service{
	public class SBase {
        //public IEnumerator Download (string url, string path)
        public IEnumerator Download (string url, int ver, System.Action<AssetBundle> handle, bool destory = true)
        {
            var www = WWW.LoadFromCacheOrDownload(url, ver);
            while (!www.isDone)
            {
                App.Controller.CLoadingDialog.UpdatePlusProgress(www.progress);
                if(!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                    break;
                }
                yield return null;
            }
            //yield return www;
            if(!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            handle(www.assetBundle);
            if (destory)
            {
                www.assetBundle.Unload(false);
            }
            //www.assetBundle.Unload(false);
            /*using (WWW www = new WWW (url + "?time=" + System.DateTime.Now.GetHashCode())) {

                yield return www;

                File.Delete (path);
                File.WriteAllBytes (path, www.bytes);
            }
            yield return 0;*/
        }
	}
}