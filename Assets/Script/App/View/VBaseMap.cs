﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using Holoville.HOTween;

namespace App.View{
    public class VBaseMap : VBase {
        [SerializeField]public int mapWidth;
        [SerializeField]public int mapHeight;
        [SerializeField]public VTile[] tileUnits;
        [SerializeField]protected Camera camera3d;
        protected Vector2 camera3dPosition;
        protected Vector2 mousePosition = Vector2.zero;
        protected bool _isDraging;
        protected bool _camera3DEnable = true;
        protected const float tileWidth = 0.69f;
        protected const float tileHeight = 0.6f;
        #region VM处理
        public VMBaseMap ViewModel { get { return (VMBaseMap)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMBaseMap oldVm = oldViewModel as VMBaseMap;
            if (oldVm != null)
            {
                ViewModel.MapId.OnValueChanged -= MapIdChanged;
                ViewModel.Tiles.OnValueChanged -= TilesChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.MapId.OnValueChanged += MapIdChanged;
                ViewModel.Tiles.OnValueChanged += TilesChanged;
            }
        }
        private void MapIdChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MBaseMap baseMapMaster = BaseMapCacher.Instance.Get(newvalue);
            ResetAll(baseMapMaster);
        }
        private void TilesChanged(App.Model.MTile[] oldvalue, App.Model.MTile[] newvalue)
        {
            ResetAll();
        }
        #endregion
        public void ResetAll(App.Model.Master.MBaseMap baseMapMaster = null){
            if (baseMapMaster == null)
            {
                baseMapMaster = BaseMapCacher.Instance.Get(ViewModel.MapId.Value);
            }
            int widthCount = 0;
            int heightCount = 0;
            int i = 0;
            foreach(VTile tile in tileUnits){
                tile.gameObject.SetActive(false);
            }
            foreach(App.Model.Master.MTile tile in baseMapMaster.tiles){
                i = heightCount * mapWidth + widthCount;
                VTile obj = tileUnits[i];
                obj.gameObject.SetActive (true);
                App.Model.MTile building = System.Array.Find(ViewModel.Tiles.Value, _=>_.x == widthCount && _.y == heightCount);
                //if(building != null)Debug.LogError(widthCount + ","+heightCount + ", " + building.Master);
                obj.SetData(heightCount * baseMapMaster.width + widthCount, tile.id, building != null ? building.Master.id : 0);
                widthCount++;
                if (widthCount >= baseMapMaster.width)
                {
                    widthCount = 0;
                    heightCount++;
                }
                if (heightCount >= baseMapMaster.height)
                {
                    break;
                }
            }
            BoxCollider collider = this.GetComponent<BoxCollider>();
            collider.size = new Vector3(baseMapMaster.width * tileWidth + 0.345f, baseMapMaster.height * tileHeight + 0.2f, 0f);
            collider.center = new Vector3(collider.size.x * 0.5f - 0.345f, -collider.size.y * 0.5f + 0.4f, 0f);
            mousePosition.x = int.MinValue;
        }
        public bool IsDraging{
            get{ 
                return _isDraging;
            }
        }
        public void MoveToCenter(){
            App.Model.Master.MBaseMap baseMapMaster = BaseMapCacher.Instance.Get(ViewModel.MapId.Value);
            int widthCount = Mathf.FloorToInt(baseMapMaster.width / 2f);
            int heightCount = Mathf.FloorToInt(baseMapMaster.height / 2f);
            int i = heightCount * mapWidth + widthCount;
            VTile obj = tileUnits[i];
            Camera3dToPosition(obj.transform.position.x, obj.transform.position.y - 9f);
        }
        void OnMouseDrag(){
            if (mousePosition.x == int.MinValue)
            {
                return;
            }
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z));
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            camera3d.transform.localPosition = new Vector3(camera3dPosition.x + (mousePosition.x - Input.mousePosition.x) * 0.01f, camera3dPosition.y + (mousePosition.y - Input.mousePosition.y)*0.01f, camera3d.transform.localPosition.z);
        }
        void OnMouseUp(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                return;
            }
            _isDraging = Input.mousePosition.x != mousePosition.x || Input.mousePosition.y != mousePosition.y;
            mousePosition.x = int.MinValue;
        }
        void OnMouseDown(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                return;
            }
            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;
            camera3dPosition = new Vector2(camera3d.transform.localPosition.x, camera3d.transform.localPosition.y);
        }
        public void Camera3dToPosition(float x, float y){
            HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition", new Vector3(x, y, camera3d.transform.localPosition.z)));
        }
        public bool Camera3DEnable{
            set{ 
                _camera3DEnable = value;
            }
            get{ 
                return _camera3DEnable;
            }
        }
    }
}