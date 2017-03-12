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
using App.View.Common;
using Holoville.HOTween;


namespace App.Controller{
    public class CGachaResultDialog : CDialog {
        [SerializeField]private Transform[] positions;
        [SerializeField]private GameObject contentsChild;
        [SerializeField]private Transform contentPanel;
        public override IEnumerator OnLoad( Request request ) 
        {  
            StartCoroutine(base.OnLoad(request));
            App.Model.MContent[] contents = request.Get<App.Model.MContent[]>("contents");
            if (contents.Length == 1)
            {
                App.Model.MContent content = contents[0];
                CoroutineShowContent(content, Vector3.zero);
            }
            else
            {
                StartCoroutine(CoroutineShowContents(contents));
            }
            yield break;
		}
        private IEnumerator CoroutineShowContents(App.Model.MContent[] contents){
            for (int i = 0; i < contents.Length; i++)
            {
                App.Model.MContent content = contents[i];
                VContentsChild vContentsChild = CoroutineShowContent(content, positions[i].localPosition);
                while (!vContentsChild.showComplete)
                {
                    yield return new WaitForEndOfFrame();
                }
            }   
        }
        private VContentsChild CoroutineShowContent(App.Model.MContent content, Vector3 position){
            GameObject obj = Instantiate(contentsChild);
            obj.transform.SetParent(contentPanel);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.zero;
            obj.transform.eulerAngles = new Vector3(0, 0, 0f);
            VContentsChild vContentsChild = obj.GetComponent<VContentsChild>();
            vContentsChild.showComplete = false;
            vContentsChild.UpdateView(content);
            HOTween.To(obj.transform, 0.6f, new TweenParms()
                .Prop("eulerAngles", new Vector3(0, 0, 360f))
                .Prop("localPosition", position)
                .Prop("localScale", Vector3.one)
                .OnComplete(()=>{
                    vContentsChild.showComplete = true;
            }));
            return vContentsChild;
        }
        public void BackgroundClick(){
            this.Close();
        }
        public void EquipmentIconClick(int equipmentId){

        }
	}
}