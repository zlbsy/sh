using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Character{
    public class VRawFace : VBase {
        private RawImage icon;
        public int CharacterId
        {
            set{
                this.StartCoroutine(LoadFaceIcon(value));
            }
        }
        public IEnumerator LoadFaceIcon(int characterId)
		{
			if (icon == null)
			{
				icon = this.GetComponent<RawImage>();
			}
            string url = string.Format(App.Model.Scriptable.FaceAsset.FaceUrl, characterId);
            yield return this.StartCoroutine(Global.SUser.Download(url, App.Util.Global.versions.face, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.FaceAsset.assetbundle = assetbundle;
                App.Model.Scriptable.MFace mFace = App.Model.Scriptable.FaceAsset.Data.face;
                icon.texture = mFace.image as Texture;
                icon.uvRect = mFace.rect;
            }));
        }

    }
}