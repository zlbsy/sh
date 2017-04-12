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
        [SerializeField]public SpriteRenderer movingSprite;
        [SerializeField]public SpriteRenderer attackSprite;
        [SerializeField]public TextMesh tileName;

        public int MovingPower{ get; set;}
        public bool IsChecked{ get; set;}
        public int Index{ get; set;}
        public int CoordinateX{ get; set;}
        public int CoordinateY{ get; set;}
        public int G{ get; set;}
        public int H{ get; set;}
        public int F{ get; set;}
        public int NodeIndex{ get; set;}
        public bool IsOpen{ get; set;}
        public VTile ParentNode{ get; set;}
        public bool IsRoad{ get; set;}
        public bool IsAllCost{ get; set;}

        private GameObject attackTween;
        private VBaseMap vBaseMap;
        public void SearchInit(){
            MovingPower = 0;
            IsChecked = false;
            IsOpen = false;
            IsRoad = true;
            IsAllCost = false;
            ParentNode = null;
        }
        void Start(){
            lineSprite.sprite = App.Model.Master.MTile.GetIcon(0);
            tileName.GetComponent<MeshRenderer>().sortingOrder = 5;
        }
        public void SetData(int index, int cx, int cy, int tileId, int subId = 0){
            this.Index = index;
            this.CoordinateX = cx;
            this.CoordinateY = cy;
            tileSprite.sprite = App.Model.Master.MTile.GetIcon(tileId);
            tileName.gameObject.SetActive(false);
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
            this.Controller.SendMessage("OnClickTile", this.Index);
        }
        public void ShowMoving(App.Model.Belong belong){
            this.movingSprite.gameObject.SetActive(true);
            this.movingSprite.sprite = App.Model.Master.MTile.GetIcon(string.Format("moving_{0}", belong.ToString()));
        }
        public void ShowAttack(){
            this.attackSprite.gameObject.SetActive(true);
            this.attackSprite.sprite = App.Model.Master.MTile.GetIcon("attack");
        }
        public void HideMoving(){
            this.movingSprite.gameObject.SetActive(false);
        }
        public void HideAttack(){
            this.attackSprite.gameObject.SetActive(false);
        }
        public void SetColor(Color color){
            tileSprite.color = color;
            buildingSprite.color = color;
        }
        public void SetAttackTween(GameObject attackTween){
            attackTween.transform.SetParent(this.transform);
            attackTween.transform.localPosition = Vector3.zero;
            attackTween.transform.localScale = Vector3.one;
            this.attackTween = attackTween;
        }
        public bool IsAttackTween{
            get{ 
                return attackTween != null;
            }
        }
    }
}