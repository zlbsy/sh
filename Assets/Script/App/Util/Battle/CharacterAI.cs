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
        private MCharacter attackTarget = null;
        private VTile targetTile = null;
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
            MSkill attackSkill = System.Array.Find(mCharacter.Skills, delegate(MSkill skill){
                App.Model.Master.MSkill skillMaster = skill.Master;
                return (skillMaster.type == SkillType.attack || skillMaster.type == SkillType.magic) 
                    && System.Array.IndexOf(skillMaster.weapon_types, mCharacter.WeaponType) >= 0;
            });
            mCharacter.CurrentSkill = attackSkill;
            int index = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY).Index;
            cBattlefield.manager.ClickNoneNode(index);
            yield return new WaitForEndOfFrame();
            FindAttackTarget();
            //yield return cBattlefield.StartCoroutine();
            bool canKill = false;
            if (targetTile != null)
            {
                canKill = cBattlefield.calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.Hp >= 0;
            }
            if (canKill)
            {
                yield return cBattlefield.StartCoroutine(Attack());
            }
            else
            {
                bool needHeal = false;
                MSkill healSkill = System.Array.Find(mCharacter.Skills, delegate(MSkill skill){
                    App.Model.Master.MSkill skillMaster = skill.Master;
                    return skillMaster.type == SkillType.heal && System.Array.IndexOf(skillMaster.weapon_types, mCharacter.WeaponType) >= 0;
                });
                if (healSkill != null)
                {
                    Debug.LogError("healSkill = " + healSkill);
                }
                if (needHeal)
                {
                    yield return cBattlefield.StartCoroutine(Heal());
                }
                else
                {
                    yield return cBattlefield.StartCoroutine(Attack());
                }
            }
            mCharacter.CurrentSkill = attackSkill;
        }

        private IEnumerator Attack(){
            yield return cBattlefield.StartCoroutine(WaitMoving());
            if (targetTile == null)
            {
                //向最近武将移动
                yield return cBattlefield.StartCoroutine(MoveToNearestTarget());
                cBattlefield.manager.ActionOver();
            }
            else
            {
                //攻击
                VTile vTile = cBattlefield.mapSearch.GetTile(attackTarget.CoordinateX, attackTarget.CoordinateY);
                cBattlefield.manager.ClickAttackNode(vTile.Index);
            }
        }
        private IEnumerator MoveToNearestTarget(){
            List<VTile> tileList = null;
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                VTile startTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile endTile = cBattlefield.mapSearch.GetTile(character.CoordinateX, character.CoordinateY);
                List<VTile> tiles = cBattlefield.aStar.Search(startTile, endTile);
                if (tileList == null || tileList.Count > tiles.Count)
                {
                    tileList = tiles;
                }
            }
            for (int i = tileList.Count - 1; i >= 0; i--)
            {
                VTile tile = tileList[i];
                if (!cBattlefield.tilesManager.IsInMovingCurrentTiles(tile))
                {
                    continue;
                }
                MCharacter character = System.Array.Find(mBaseMap.Characters, chara=>chara.CoordinateX == tile.CoordinateX && chara.CoordinateY == tile.CoordinateY);
                if (character != null)
                {
                    continue;
                }
                cBattlefield.manager.ClickMovingNode(tile.Index);
                break;
            }
            do{
                yield return new WaitForEndOfFrame();
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }
        private IEnumerator WaitMoving(){
            if (targetTile != null)
            {
                cBattlefield.manager.ClickMovingNode(targetTile.Index);
            }
            do{
                yield return new WaitForEndOfFrame();
            }
            while (cBattlefield.battleMode == CBattlefield.BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }
        private void FindAttackTarget(){
            attackTarget = null;
            targetTile = null;
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
                bool aCanKill = cBattlefield.calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.Hp >= 0;
                if (aCanKill)
                {
                    continue;
                }
                bool bCanKill = cBattlefield.calculateManager.Hert(mCharacter, character, targetTile) - character.Hp >= 0;
                if (!aCanKill && bCanKill)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    continue;
                }
                bool aCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, attackTarget, targetTile.CoordinateX, targetTile.CoordinateY, attackTarget.CoordinateX, attackTarget.CoordinateY);
                bool bCanCounter = cBattlefield.calculateManager.CanCounterAttack(mCharacter, character, vTile.CoordinateX, vTile.CoordinateY, character.CoordinateX, character.CoordinateY);
                if (!aCanCounter && bCanCounter)
                {
                    continue;
                }else if (aCanCounter && !bCanCounter)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    continue;
                }
                //TODO::地形优势
            }
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