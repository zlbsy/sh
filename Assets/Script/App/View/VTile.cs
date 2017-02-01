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
        #region VM处理

        #endregion
        public void SetData(int x, int y, string spriteName){
            this.x = x;
            this.y = y;
            tileSprite.sprite = AssetBundleManager.GetMapTile(spriteName);
        }
        void OnMouseDown(){
            Debug.LogError("OnMouseDown : "+this.name);
        }

    }
}