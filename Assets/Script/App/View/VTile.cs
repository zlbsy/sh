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
        [SerializeField]public SpriteRenderer lineSprite;
        [SerializeField]public TextMesh tileName;
        private int index = 0;
        private VBaseMap vBaseMap;
        void Start(){
            tileName.GetComponent<MeshRenderer>().sortingOrder = 5;
        }
        public int Index{
            get{ 
                return index;
            }
        }
        public void SetData(int index, int tileId, int subId = 0){
            this.index = index;
            tileSprite.sprite = App.Model.Master.MTile.GetIcon(tileId);
            tileName.gameObject.SetActive(false);
            if (lineSprite.sprite == null)
            {
                lineSprite.sprite = App.Model.Master.MTile.GetIcon(0);
            }
            if (subId > 0)
            {
                buildingSprite.gameObject.SetActive(true);
                buildingSprite.sprite = App.Model.Master.MTile.GetIcon(subId);
                if (subId > 2000)
                {
                    tileName.gameObject.SetActive(true);
                    string nameKey = TileCacher.Instance.Get(subId).name;
                    tileName.text = Language.Get(nameKey);
                }
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
            if (vBaseMap == null)
            {
                vBaseMap = this.GetComponentInParent<VBaseMap>();
            }
            if (!vBaseMap.Camera3DEnable || vBaseMap.IsDraging)
            {
                yield break;
            }
            this.Controller.SendMessage("OnClickTile", this.index);
        }
        public void SetColor(Color color){
            tileSprite.color = color;
            buildingSprite.color = color;
        }
    }
}