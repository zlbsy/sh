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
        private MCharacter mCharacter;
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
            //TODO::行动顺序
            mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.Belong == this.belong && !_.ActionOver);

            MSkill skill = mCharacter.CurrentSkill;
            int index = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY).Index;
            cBattlefield.manager.ClickNoneNode(index);
            yield return new WaitForEndOfFrame();
            bool needHeal = false;
            if (needHeal)
            {
                yield return cBattlefield.StartCoroutine(Heal());
            }
            else
            {
                yield return cBattlefield.StartCoroutine(Attack());
            }

        }
        private IEnumerator Attack(){
            yield return cBattlefield.StartCoroutine(MoveToAttackTarget());

        }
        private IEnumerator MoveToAttackTarget(){
            MCharacter attackTarget;
            VTile targetTile;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                MCharacter target;
                VTile vTile = GetNestNode(character, cBattlefield.tilesManager.CurrentMovingTiles);
                bool canAttack = cBattlefield.charactersManager.IsInAttackDistance(vTile.CoordinateX, vTile.CoordinateY, mCharacter.CoordinateX, mCharacter.CoordinateY, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (attackTarget == null)
                {
                    attackTarget = character;
                    targetTile = vTile;
                }
                else
                {
                    //选择
                }
            }
            if (attackTarget != null)
            {
                cBattlefield.manager.ClickMovingNode(targetTile.Index);
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        private VTile GetNestNode(MCharacter target, List<VTile> tiles, out MCharacter attackTarget){
            if (cBattlefield.tilesManager.CurrentMovingTiles.Count == 1)
            {
                return cBattlefield.tilesManager.CurrentMovingTiles[0];
            }
            tiles.Sort((a, b)=>{
                bool aCanAttack = cBattlefield.charactersManager.IsInAttackDistance(a.CoordinateX, a.CoordinateY, target.CoordinateX, target.CoordinateY, mCharacter);
                bool bCanAttack = cBattlefield.charactersManager.IsInAttackDistance(b.CoordinateX, b.CoordinateY, target.CoordinateX, target.CoordinateY, mCharacter);
                if(aCanAttack && !bCanAttack){
                    return -1;
                }else if(!aCanAttack && bCanAttack){
                    return 1;
                }else if(aCanAttack && bCanAttack){
                    bool aCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, target, a.CoordinateX, a.CoordinateY, target.CoordinateX, target.CoordinateY);
                    bool bCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, target, b.CoordinateX, b.CoordinateY, target.CoordinateX, target.CoordinateY);
                    if(aCanCounter && !bCanCounter){
                        return 1;
                    }else if(!aCanCounter && bCanCounter){
                        return -1;
                    }else if(aCanCounter && bCanCounter){
                        //TODO::地形优势
                        return 0;
                    }
                }
                int aDistance = cBattlefield.mapSearch.GetDistance(mCharacter.CoordinateX, mCharacter.CoordinateY, a.CoordinateX, a.CoordinateY);
                int bDistance = cBattlefield.mapSearch.GetDistance(mCharacter.CoordinateX, mCharacter.CoordinateY, b.CoordinateX, b.CoordinateY);
                return aDistance - bDistance;
            });
            return cBattlefield.tilesManager.CurrentMovingTiles[0];
        }
        private IEnumerator Heal(){
            yield break;
        }
    }
}