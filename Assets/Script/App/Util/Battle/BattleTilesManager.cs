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
        private List<VTile> currentTiles;
        public BattleTilesManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void ShowCharacterMovingArea(MCharacter mCharacter){
            currentTiles = cBattlefield.breadthFirst.Search(mCharacter);
            vBaseMap.SetTilesColor(currentTiles, Color.blue);
            cBattlefield.battleMode = CBattlefield.BattleMode.show_move_tiles;
        }
        public void ShowCharacterAttackArea(MCharacter mCharacter){
            currentTiles = cBattlefield.breadthFirst.Search(mCharacter, 1);
            currentTiles = currentTiles.FindAll(_=>_.CoordinateX != mCharacter.CoordinateX || _.CoordinateY != mCharacter.CoordinateY);
            vBaseMap.SetTilesColor(currentTiles, Color.red);
            //cBattlefield.battleMode = CBattlefield.BattleMode.show_move_tiles;
        }
        public bool IsInCurrentTiles(int index){
            return IsInCurrentTiles(cBattlefield.mapSearch.GetTile(index));
        }
        public bool IsInCurrentTiles(VTile vTile){
            return currentTiles.Exists(_=>_.Index == vTile.Index);
        }
        public void ClearCurrentTiles(){
            foreach (VTile tile in currentTiles)
            {
                vBaseMap.SetTilesColor(currentTiles, Color.white);
            }
        }
    }
}