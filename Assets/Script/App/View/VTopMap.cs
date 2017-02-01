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
        [SerializeField]public int mapWidth;
        [SerializeField]public int mapHeight;
        [SerializeField]public VTile[] tileUnits;
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
            int widthCount = 0;
            int heightCount = 0;
            int i = 0;
            foreach(VTile tile in tileUnits){
                tile.gameObject.SetActive(false);
            }
            foreach(App.Model.Master.MTile tile in topMapMaster.tiles){
                i = heightCount * mapWidth + widthCount;
                VTile obj = tileUnits[i];
                obj.gameObject.SetActive (true);
                obj.SetData(widthCount, heightCount, "tile_" + tile.id);
                widthCount++;
                if (widthCount >= topMapMaster.width)
                {
                    widthCount = 0;
                    heightCount++;
                }
                if (heightCount >= topMapMaster.height)
                {
                    break;
                }
            }
        }

    }
}