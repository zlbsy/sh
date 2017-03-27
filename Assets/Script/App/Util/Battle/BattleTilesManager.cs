using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using App.Controller.Battle;

namespace App.Util.Battle{
    public class BattleTilesManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> currentTiles;
        private List<GameObject> attackIcons = new List<GameObject>();
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
            foreach(VTile tile in currentTiles){
                MCharacter character = cBattlefield.manager.GetCharacter(tile.Index);
                if (character == null || character.Belong == mCharacter.Belong)
                {
                    continue;
                }
                GameObject attackTween = cBattlefield.CreateAttackTween();
                attackTween.transform.SetParent(tile.transform);
                attackTween.transform.localPosition = Vector3.zero;
                attackTween.transform.localScale = Vector3.one;
                attackIcons.Add(attackTween);
            }
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
            foreach (GameObject obj in attackIcons)
            {
                GameObject.Destroy(obj);
            }
        }
    }
}