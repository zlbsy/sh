using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;

namespace App.Util.Battle{
    public class BattleTilesManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> movingTiles;
        public BattleTilesManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void ShowCharacterMovingArea(MCharacter mCharacter){
            movingTiles = cBattlefield.breadthFirst.Search(mCharacter);
            vBaseMap.SetTilesColor(movingTiles, Color.blue);
            cBattlefield.battleMode = CBattlefield.BattleMode.show_move_tiles;
        }
        /*public List<VTile> GetNeighboringTiles(Vector2 coordinate){
            List<Vector2> coordinates = cBattlefield.tileMapManager.GetNeighboringCoordinates(coordinate);
            List<VTile> tiles = new List<VTile>();
            foreach(Vector2 vec in coordinates){
                int i = (int)vec.y * vBaseMap.mapWidth + (int)vec.x;
                VTile tile = vBaseMap.tileUnits[i];
                tiles.Add(tile);
            }
            return tiles;
        }*/
        public VTile GetTile(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            return GetTile(coordinate);
        }
        public VTile GetTile(Vector2 coordinate){
            int i = (int)coordinate.y * vBaseMap.mapWidth + (int)coordinate.x;
            return vBaseMap.tileUnits[i];
        }
        public bool IsMovingTile(int index){
            return IsMovingTile(GetTile(index));
        }
        public bool IsMovingTile(VTile vTile){
            return movingTiles.Exists(_=>_.Index == vTile.Index);
        }
    }
}