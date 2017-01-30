using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;

namespace App.View{
    public class VTopMap : VBase {
        [SerializeField]Image tileUnit;
        #region VM处理
        public VMTopMap ViewModel { get { return (VMTopMap)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMTopMap oldVm = oldViewModel as VMTopMap;
            if (oldVm != null)
            {
                ViewModel.MapId.OnValueChanged -= MapIdChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.MapId.OnValueChanged += MapIdChanged;
            }
        }
        private void MapIdChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MTopMap topMapMaster = TopMapCacher.Instance.Get(newvalue);
            ResetAll(topMapMaster);
        }
        #endregion
        public void ResetAll(App.Model.Master.MTopMap topMapMaster = null){
            if (topMapMaster == null)
            {
                topMapMaster = TopMapCacher.Instance.Get(ViewModel.MapId.Value);
            }
            this.ClearChild();
            int widthCount = 0;
            int heightCount = 0;
            int i = 0;
            foreach(App.Model.Master.MTile tile in topMapMaster.tiles){
                GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                //obj.name = tile.id.ToString();
                obj.name = (i++).ToString();
                obj.transform.parent = this.transform;
                RectTransform rectTrans = obj.GetComponent<RectTransform>();
                rectTrans.anchoredPosition = new Vector2(widthCount * 69f, heightCount * 80f);
                obj.SetActive (true);
                widthCount++;
                if (widthCount >= topMapMaster.width)
                {
                    widthCount = 0;
                    heightCount++;
                }
            }
        }

    }
}