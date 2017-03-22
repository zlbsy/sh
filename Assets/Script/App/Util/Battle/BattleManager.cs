using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;

namespace App.Util.Battle{
    public class BattleManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public BattleManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void ClickNoneNode(int index){
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter != null)
            {
                cBattlefield.tilesManager.ShowCharacterMovingArea(mCharacter);
            }
            /*
            List<VCharacter> vCharacters = cBattlefield.GetVBaseMap().Characters;
            VCharacter vCharacter = cBattlefield.GetVBaseMap().Characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            if (vCharacter != null)
            {
                cBattlefield.tilesManager.ShowCharacterMovingArea(vCharacter, index, coordinate);
            }*/
        }
        public void ClickMovingNode(int index){
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter != null)
            {
                return;
            }
            if (cBattlefield.tilesManager.IsMovingTile(index))
            {
                
            }
            else
            {
                
            }

        }
        public MCharacter GetCharacter(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.CoordinateX == coordinate.x && _.CoordinateY == coordinate.y);
        }
    }
}