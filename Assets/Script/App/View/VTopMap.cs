using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using Holoville.HOTween;

namespace App.View{
    public class VTopMap : VBase {
        [SerializeField]public int mapWidth;
        [SerializeField]public int mapHeight;
        [SerializeField]public VTile[] tileUnits;
        [SerializeField]Camera camera3d;
        private Vector2 camera3dPosition;
        private Vector2 mousePosition;
        private bool _isDraging;
        private const float tileWidth = 0.69f;
        private const float tileHeight = 0.6f;
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
                App.Model.MTile building = System.Array.Find(ViewModel.Tiles.Value, _=>_.x == widthCount && _.y == heightCount);
                obj.SetData(i, tile.id, building != null ? building.Master.id : 0);
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
            BoxCollider collider = this.GetComponent<BoxCollider>();
            collider.size = new Vector3(topMapMaster.width * tileWidth + 0.345f, topMapMaster.height * tileHeight + 0.2f, 0f);
            collider.center = new Vector3(collider.size.x * 0.5f - 0.345f, -collider.size.y * 0.5f + 0.4f, 0f);
        }
        public bool IsDraging{
            get{ 
                return _isDraging;
            }
        }
        public void MoveToCenter(){
            App.Model.Master.MTopMap topMapMaster = TopMapCacher.Instance.Get(ViewModel.MapId.Value);
            int widthCount = Mathf.FloorToInt(topMapMaster.width / 2f);
            int heightCount = Mathf.FloorToInt(topMapMaster.height / 2f);
            int i = heightCount * mapWidth + widthCount;
            VTile obj = tileUnits[i];
            Camera3dToPosition(obj.transform.position.x, obj.transform.position.y - 9f);
        }
        void OnMouseDrag(){
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z));
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            camera3d.transform.localPosition = new Vector3(camera3dPosition.x + (mousePosition.x - Input.mousePosition.x) * 0.01f, camera3dPosition.y + (mousePosition.y - Input.mousePosition.y)*0.01f, camera3d.transform.localPosition.z);
        }
        void OnMouseUp(){
            _isDraging = Input.mousePosition.x != mousePosition.x || Input.mousePosition.y != mousePosition.y;
        }
        void OnMouseDown(){
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            camera3dPosition = new Vector2(camera3d.transform.localPosition.x, camera3d.transform.localPosition.y);
        }
        public void Camera3dToPosition(float x, float y){
            HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition", new Vector3(x, y, camera3d.transform.localPosition.z)));
        }
    }
}