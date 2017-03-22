using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;
using App.Util.Cacher;
using App.Controller;

namespace App.Util.Search{
    public class BreadthFirst{
        private CBaseMap cBaseMap;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> tiles;
        public BreadthFirst(CBaseMap controller, MBaseMap model, VBaseMap view){
            cBaseMap = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public List<VTile> Search(MCharacter mCharacter){
            SearchInit();
            tiles = new List<VTile>();
            App.Model.Master.MCharacter characterMaster = mCharacter.Master;
            int movePower = characterMaster.moving_power;
            Debug.LogError("movePower = " + movePower);
            if (movePower == 0)
            {
                movePower = 3;
            }
            int i = mCharacter.CoordinateY * vBaseMap.mapWidth + mCharacter.CoordinateX;
            VTile tile = vBaseMap.tileUnits[i];
            tile.movingPower = movePower;
            LoopSearch(tile);
            //Vector2 start = new Vector2(vCharacter.ViewModel.CoordinateX.Value, vCharacter.ViewModel.CoordinateY.Value);

            return tiles;
        }
        private void LoopSearch(VTile vTile){
            if (vTile.movingPower <= 0)
            {
                return;
            }
            if (!vTile.isChecked)
            {
                vTile.isChecked = true;
                tiles.Add(vTile);
            }
            List<Vector2> coordinates = cBaseMap.tileMapManager.GetNeighboringCoordinates(baseMapMaster.GetCoordinateFromIndex(vTile.Index));
            foreach (Vector2 vec in coordinates)
            {
                VTile tile = vBaseMap.tileUnits[(int)vec.y * vBaseMap.mapWidth + (int)vec.x];
                if (tile.isChecked && tile.movingPower >= vTile.movingPower)
                {
                    continue;
                }
                int cost = 1;
                tile.movingPower = vTile.movingPower - cost;

                LoopSearch(tile);
            }
        }
        private void SearchInit(){
            foreach (VTile tile in vBaseMap.tileUnits)
            {
                tile.SearchInit();
            }
        }
    }
}