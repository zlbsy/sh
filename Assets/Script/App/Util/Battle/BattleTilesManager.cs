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
    /// <summary>
    /// 战场地图块操作相关
    /// </summary>
    public class BattleTilesManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> currentMovingTiles;
        private List<VTile> currentAttackTiles;
        private List<GameObject> attackIcons = new List<GameObject>();
        public List<VTile> CurrentMovingTiles{get{ return currentMovingTiles;}}
        public BattleTilesManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public void ShowCharacterMovingArea(MCharacter mCharacter){
            currentMovingTiles = cBattlefield.breadthFirst.Search(mCharacter, 0, true);
            vBaseMap.ShowMovingTiles(currentMovingTiles, mCharacter.Belong);
            cBattlefield.battleMode = CBattlefield.BattleMode.show_move_tiles;
        }
        public void ShowCharacterSkillArea(MCharacter mCharacter){
            int[] distance = mCharacter.CurrentSkill == null ? new int[]{0,0} : mCharacter.CurrentSkill.Master.distance;
            currentAttackTiles = cBattlefield.breadthFirst.Search(mCharacter, distance[1]);
            VTile characterTile = currentAttackTiles.Find(_=>_.CoordinateX == mCharacter.CoordinateX && _.CoordinateY == mCharacter.CoordinateY);
            currentAttackTiles = currentAttackTiles.FindAll(_=>cBattlefield.mapSearch.GetDistance(_, characterTile) >= distance[0]);
            if (mCharacter.CurrentSkill == null)
            {
                return;
            }
            vBaseMap.ShowAttackTiles(currentAttackTiles);
            if (mCharacter.Belong == Belong.self && !mCharacter.ActionOver)
            {
                ShowCharacterSkillTween(mCharacter, currentAttackTiles);
            }
        }
        public void ShowCharacterSkillTween(MCharacter mCharacter, List<VTile> tiles){
            foreach(VTile tile in tiles){
                if (tile.IsAttackTween)
                {
                    continue;
                }
                MCharacter character = cBattlefield.charactersManager.GetCharacter(tile.Index);
                if (character == null)
                {
                    continue;
                }
                bool sameBelong = cBattlefield.charactersManager.IsSameBelong(character.Belong, mCharacter.Belong);
                bool useToEnemy = mCharacter.CurrentSkill.UseToEnemy;
                Debug.LogError("useToEnemy="+useToEnemy + ", sameBelong="+sameBelong);
                if (useToEnemy ^ sameBelong)
                {
                    GameObject attackTween = cBattlefield.CreateAttackTween();
                    tile.SetAttackTween(attackTween);
                    attackIcons.Add(attackTween);
                }
                /*
                if (character == null || cBattlefield.charactersManager.IsSameBelong(character.Belong, mCharacter.Belong))
                {
                    continue;
                }*/
            }
        }
        public bool IsInMovingCurrentTiles(int index){
            return IsInMovingCurrentTiles(cBattlefield.mapSearch.GetTile(index));
        }
        public bool IsInMovingCurrentTiles(VTile vTile){
            return currentMovingTiles.Exists(_=>_.Index == vTile.Index);
        }
        public void ClearCurrentTiles(){
            if (currentMovingTiles != null)
            {
                vBaseMap.HideMovingTiles(currentMovingTiles);
                currentMovingTiles.Clear();
            }
            if (currentAttackTiles != null)
            {
                vBaseMap.HideAttackTiles(currentAttackTiles);
                currentAttackTiles.Clear();
            }
            foreach (GameObject obj in attackIcons)
            {
                GameObject.Destroy(obj);
            }
        }
    }
}