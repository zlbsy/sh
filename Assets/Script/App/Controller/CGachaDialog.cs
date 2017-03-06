using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.Model.Scriptable;


namespace App.Controller{
    public class CGachaDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private SGacha sGacha = new SGacha();
        public override IEnumerator OnLoad( Request request ) 
		{  
            StartCoroutine(base.OnLoad(request));
            yield return StartCoroutine(sGacha.Download(ImageAssetBundleManager.gachaIconUrl, App.Util.Global.SUser.versions.gacha, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.gachaIcon = assetbundle;
            }, false));
            yield return StartCoroutine(sGacha.Download(GachaAsset.Url, App.Util.Global.SUser.versions.gacha, (AssetBundle assetbundle)=>{
                GachaAsset.assetbundle = assetbundle;
                GachaCacher.Instance.Reset(GachaAsset.Data.gachas);
                GachaAsset.Clear();
            }));
            //yield return StartCoroutine (sGacha.RequestFreeLog());
            sGacha.gachas = new MGacha[]{};
            //int gachaId = request.Get<int>("gachaId");
            App.Model.Master.MGacha[] gachas = GachaCacher.Instance.GetAllOpen();
            foreach(App.Model.Master.MGacha gacha in gachas){
                App.Model.MGacha mGacha = System.Array.Find(sGacha.gachas, _=>_.GachaId == gacha.id);
                if (mGacha == null)
                {
                    mGacha = new MGacha();
                    mGacha.GachaId = gacha.id;
                }
                GameObject obj = Instantiate(childItem);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                VGachaChild vGachaChild = obj.GetComponent<VGachaChild>();
                vGachaChild.BindingContext = mGacha.ViewModel;
                vGachaChild.UpdateView();
            }
		}

        public void OnClickGacha(int gachaId, int cnt = 1){
            
        }
	}
}