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
        private static CBattlefield cBattlefield;
        public static void Init(CBattlefield controller){
            cBattlefield = controller;

        }
        public static void ShowCharacterMovingArea(VCharacter vCharacter){

        }
        public static void Destory(){
            
        }
    }
}