using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using App.Controller.Battle;
using App.View.Character;

namespace App.Util.Battle{
    /// <summary>
    /// 战场武将操作相关
    /// </summary>
    public class CharacterAI{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private Belong belong;
        public CharacterAI(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void Execute(Belong belong){
            this.belong = belong;
            cBattlefield.StartCoroutine(Execute());
        }
        public IEnumerator Execute(){
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.Belong == this.belong && !_.ActionOver);
            int index = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY).Index;
            cBattlefield.manager.ClickNoneNode(index);
            yield break;
        }
        private void MoveRoadsShow(){
            

        }
    }
}