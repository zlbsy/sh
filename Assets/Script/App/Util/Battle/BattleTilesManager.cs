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
        public void ShowCharacterAttackArea(MCharacter mCharacter){
            movingTiles = cBattlefield.breadthFirst.Search(mCharacter);
            vBaseMap.SetTilesColor(movingTiles, Color.blue);
            //cBattlefield.battleMode = CBattlefield.BattleMode.show_move_tiles;
        }
        public bool IsMovingTile(int index){
            return IsMovingTile(cBattlefield.mapSearch.GetTile(index));
        }
        public bool IsMovingTile(VTile vTile){
            return movingTiles.Exists(_=>_.Index == vTile.Index);
        }
        public void ClearMovingTiles(){
            foreach (VTile tile in movingTiles)
            {
                vBaseMap.SetTilesColor(movingTiles, Color.white);
            }
        }
    }
}