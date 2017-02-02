using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;

namespace App.View{
    public class VTile : VBase {
        [SerializeField]public SpriteRenderer tileSprite;
        [SerializeField]public SpriteRenderer buildingSprite;
        private int index = 0;
        private VTopMap vTopMap;
        #region VM处理

        #endregion
        public void SetData(int index, int tileId, int buildId = 0){
            this.index = index;
            tileSprite.sprite = AssetBundleManager.GetMapTile(string.Format("tile_{0}", tileId));
            if (buildId > 0)
            {
                buildingSprite.gameObject.SetActive(true);
                buildingSprite.sprite = AssetBundleManager.GetMapTile(string.Format("tile_{0}", buildId));
            }
            else
            {
                buildingSprite.gameObject.SetActive(false);
            }
        }
        void OnMouseUp(){
            if (vTopMap == null)
            {
                vTopMap = this.GetComponentInParent<VTopMap>();
            }
            if (vTopMap.IsDraging)
            {
                return;
            }
            Debug.Log("VTile OnMouseUp");
            //vTopMap.Camera3dToPosition(this.transform.position.x, this.transform.position.y - 9f);
        }

    }
}