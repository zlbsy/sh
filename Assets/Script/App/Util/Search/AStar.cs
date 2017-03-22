using System.Collections;
using System.Collections.Generic;
using App.Controller;
using App.Model;
using App.View;
using App.Util.Cacher;
using UnityEngine;

namespace App.Util.Search{
    public class AStar{
        private CBaseMap cBaseMap;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public AStar(CBaseMap controller, MBaseMap model, VBaseMap view){
            cBaseMap = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public List<VTile> Search(VCharacter vCharacter){
            Vector2 start = new Vector2(vCharacter.ViewModel.CoordinateX.Value, vCharacter.ViewModel.CoordinateY.Value);

            return null;
        }
    }
}