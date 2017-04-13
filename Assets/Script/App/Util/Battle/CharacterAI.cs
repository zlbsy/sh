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
            if (mCharacter.Target == null)
            {
                //向最近武将移动
                yield return cBattlefield.StartCoroutine(MoveToNearestTarget());
            }
            else
            {
                //攻击
                VTile vTile = cBattlefield.mapSearch.GetTile(mCharacter.Target.CoordinateX, mCharacter.Target.CoordinateY);
                cBattlefield.manager.ClickAttackNode(vTile.Index);
            }
        }
        private IEnumerator MoveToNearestTarget(){
            yield return new WaitForEndOfFrame();
        }
        private IEnumerator MoveToAttackTarget(){
            MCharacter attackTarget = null;
            VTile targetTile = null;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                VTile vTile = GetNearestNode(character, cBattlefield.tilesManager.CurrentMovingTiles);
                bool canAttack = cBattlefield.charactersManager.IsInAttackDistance(character.CoordinateX, character.CoordinateY, vTile.CoordinateX, vTile.CoordinateY, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (attackTarget == null)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    continue;
                }
                bool aCanKill = cBattlefield.calculateManager.Hert(mCharacter, attackTarget) - attackTarget.Hp >= 0;
                bool bCanKill = cBattlefield.calculateManager.Hert(mCharacter, character) - character.Hp >= 0;
                if (!aCanKill && bCanKill)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    continue;
                }
                bool aCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, attackTarget, targetTile.CoordinateX, targetTile.CoordinateY, attackTarget.CoordinateX, attackTarget.CoordinateY);
                bool bCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, character, vTile.CoordinateX, vTile.CoordinateY, character.CoordinateX, character.CoordinateY);
                if (aCanCounter && !bCanCounter)
                {
                    attackTarget = character;
                    targetTile = vTile;
                }
                //TODO::地形优势
            }
            if (attackTarget != null)
            {
                mCharacter.Target = attackTarget;
                cBattlefield.manager.ClickMovingNode(targetTile.Index);
            }
            do{
                yield return new WaitForEndOfFrame();
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }
        private VTile GetNearestNode(MCharacter target, List<VTile> tiles){
            if (cBattlefield.tilesManager.CurrentMovingTiles.Count == 1)
            {
                return cBattlefield.tilesManager.CurrentMovingTiles[0];
            }
            tiles.Sort((a, b)=>{
                bool aCanAttack = cBattlefield.charactersManager.IsInAttackDistance(target.CoordinateX, target.CoordinateY, a.CoordinateX, a.CoordinateY, mCharacter);
                bool bCanAttack = cBattlefield.charactersManager.IsInAttackDistance(target.CoordinateX, target.CoordinateY, b.CoordinateX, b.CoordinateY, mCharacter);
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
                        //return 0;
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