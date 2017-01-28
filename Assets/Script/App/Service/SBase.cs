using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace App.Service{
	public class SBase {
        public IEnumerator Download (string url, string path)
        {
            using (WWW www = new WWW (url + "?time=" + System.DateTime.Now.GetHashCode())) {

                yield return www;

                File.Delete (path);
                File.WriteAllBytes (path, www.bytes);
            }
            yield return 0;
        }
	}
}