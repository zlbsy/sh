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
        private int x = 0;
        private int y = 0;
        private VTopMap vTopMap;
        #region VM处理

        #endregion
        public void SetData(int x, int y, string spriteName){
            this.x = x;
            this.y = y;
            tileSprite.sprite = AssetBundleManager.GetMapTile(spriteName);
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
            vTopMap.Camera3dToPosition(this.transform.position.x, this.transform.position.y - 9f);
        }

    }
}