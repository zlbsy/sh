using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

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
            tileSprite.sprite = App.Model.Master.MTile.GetIcon(tileId);
            if (buildId > 0)
            {
                buildingSprite.gameObject.SetActive(true);
                buildingSprite.sprite = App.Model.Master.MTile.GetIcon(buildId);
            }
            else
            {
                buildingSprite.gameObject.SetActive(false);
            }
        }
        void OnMouseUp(){
            StartCoroutine (OnClickTile());
        }
        IEnumerator OnClickTile(){
            yield return 0;
            if (Global.SceneManager.DialogIsShow())
            {
                yield break;
            }
            if (vTopMap == null)
            {
                vTopMap = this.GetComponentInParent<VTopMap>();
            }
            if (!vTopMap.Camera3DEnable || vTopMap.IsDraging)
            {
                yield break;
            }
            if (this.Controller is CTop)
            {
                (this.Controller as CTop).OnClickTile(this.index);
            }
            else
            {
            }
        }

    }
}