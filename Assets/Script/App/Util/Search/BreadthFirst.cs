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
        public List<VTile> Search(MCharacter mCharacter, int movePower = 0){
            SearchInit();
            tiles = new List<VTile>();
            if (movePower == 0)
            {
                App.Model.Master.MCharacter characterMaster = mCharacter.Master;
                movePower = characterMaster.moving_power;
                Debug.LogError("movePower = " + movePower);
                if (movePower == 0)
                {
                    movePower = 2;
                }
            }
            int i = mCharacter.CoordinateY * vBaseMap.mapWidth + mCharacter.CoordinateX;
            VTile tile = vBaseMap.tileUnits[i];
            tile.MovingPower = movePower;
            LoopSearch(tile);
            return tiles;
        }
        private void LoopSearch(VTile vTile){
            if (!vTile.IsChecked)
            {
                vTile.IsChecked = true;
                tiles.Add(vTile);
            }
            if (vTile.MovingPower <= 0)
            {
                return;
            }
            List<Vector2> coordinates = cBaseMap.mapSearch.GetNeighboringCoordinates(baseMapMaster.GetCoordinateFromIndex(vTile.Index));
            foreach (Vector2 vec in coordinates)
            {
                VTile tile = vBaseMap.tileUnits[(int)vec.y * vBaseMap.mapWidth + (int)vec.x];
                if (tile.IsChecked && tile.MovingPower >= vTile.MovingPower)
                {
                    continue;
                }
                int cost = 1;
                tile.MovingPower = vTile.MovingPower - cost;

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