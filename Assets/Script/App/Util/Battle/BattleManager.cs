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
        private static CBattlefield cBattlefield;
        public static void Init(CBattlefield controller){
            cBattlefield = controller;
            BattleTilesManager.Init(controller);
        }
        public static void ClickNodeMode(int index){
            App.Model.Master.MBaseMap topMapMaster = BaseMapCacher.Instance.Get(cBattlefield.GetMBaseMap().MapId);
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            List<VCharacter> vCharacters = cBattlefield.GetVBaseMap().Characters;
            VCharacter vCharacter = cBattlefield.GetVBaseMap().Characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            if (vCharacter != null)
            {
                BattleTilesManager.ShowCharacterMovingArea(vCharacter);
            }
        }
        public static void Destory(){
            
        }
    }
}