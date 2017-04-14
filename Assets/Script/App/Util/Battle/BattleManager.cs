using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using Holoville.HOTween;
using App.Controller.Battle;

namespace App.Util.Battle{
    /// <summary>
    /// 战场总操作相关
    /// </summary>
    public class BattleManager{
        private CBattlefield cBattlefield;
        private MCharacter mCharacter;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private System.Action returnAction;
        private List<MCharacter> attackCharacterList = new List<MCharacter>();
        public BattleManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public MCharacter CurrentCharacter{
            get{ 
                return this.mCharacter;
            }
        }
        public void ClickNoneNode(int index){
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter != null)
            {
                this.mCharacter = mCharacter;
                cBattlefield.tilesManager.ShowCharacterMovingArea(mCharacter);
                cBattlefield.tilesManager.ShowCharacterAttackArea(mCharacter);
                cBattlefield.OpenBattleCharacterPreviewDialog(mCharacter);
                int cx = mCharacter.CoordinateX;
                int cy = mCharacter.CoordinateY;
                ActionType action = mCharacter.Action;
                float x = mCharacter.X;
                Direction direction = mCharacter.Direction;
                if (mCharacter.Belong == Belong.self)
                {
                    cBattlefield.OpenOperatingMenu();
                }
                returnAction = () =>
                    {
                        this.mCharacter.CoordinateY = cy;
                        this.mCharacter.CoordinateX = cx;
                        this.mCharacter.X = x;
                        this.mCharacter.Direction = direction;
                        this.mCharacter.Action = action;
                };
            }
            //Debug.LogError("this.mCharacter = " + this.mCharacter + ", mCharacter = " + mCharacter);
        }
        public void ClickAttackNode(int index){
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter == null || !cBattlefield.charactersManager.IsInAttackDistance(mCharacter, this.mCharacter))
            {
                CharacterReturnNone();
                return;
            }
            bool sameBelong = cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, this.mCharacter.Belong);
            bool useToEnemy = this.mCharacter.CurrentSkill.UseToEnemy;
            if (useToEnemy ^ sameBelong)
            {
                CAlertDialog.Show("不能打自己人");
                return;
            }
            this.mCharacter.Target = mCharacter;
            mCharacter.Target = this.mCharacter;
            //attackCharacterList.Clear();
            if (useToEnemy)
            {
                if (false && cBattlefield.charactersManager.IsInAttackDistance(this.mCharacter, mCharacter))
                {
                    //先手攻击
                    SetAttackCharacterList(mCharacter, this.mCharacter, true);
                }
                else
                {
                    SetAttackCharacterList(this.mCharacter, mCharacter, true);
                }
            }
            else
            {
                SetAttackCharacterList(this.mCharacter, mCharacter, false);
            }

            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.attacking;
            cBattlefield.ActionEndHandler += OnAttackComplete;
            OnAttackComplete();
        }
        private void SetAttackCharacterList(MCharacter attackCharacter, MCharacter targetCharacter, bool canCounter){
            //Debug.LogError("attackCharacter="+attackCharacter.Belong+", "+attackCharacter.Id);
            //Debug.LogError("targetCharacter="+targetCharacter.Belong+", "+targetCharacter.Id);
            int attackCount = cBattlefield.calculateManager.AttackCount(attackCharacter, targetCharacter);
            while(attackCount-- > 0){
                attackCharacterList.Add(attackCharacter);
            }
            if (canCounter && cBattlefield.calculateManager.CanCounterAttack(attackCharacter, targetCharacter, attackCharacter.CoordinateX, attackCharacter.CoordinateY, targetCharacter.CoordinateX, targetCharacter.CoordinateY))
            {
                attackCount = cBattlefield.calculateManager.CounterAttackCount(attackCharacter, targetCharacter);
                while (attackCount-- > 0)
                {
                    attackCharacterList.Add(targetCharacter);
                }
            }
            //Debug.LogError("attackCharacterList.Count="+attackCharacterList.Count);
        }
        public void OnAttackComplete(){
            if (attackCharacterList.Count > 0)
            {
                MCharacter currentCharacter = attackCharacterList[0];
                attackCharacterList.RemoveAt(0);
                if (currentCharacter.Hp > 0)
                {
                    currentCharacter.Direction = (currentCharacter.X > currentCharacter.Target.X ? Direction.left : Direction.right);
                    currentCharacter.Action = ActionType.attack;
                    return;
                }
                attackCharacterList.Clear();
                bool continueAttack = false;
                //TODO::是否引导攻击
                if (continueAttack)
                {
                    MCharacter mCharacter = null;
                    if (mCharacter != null)
                    {
                        cBattlefield.ActionEndHandler -= OnAttackComplete;
                        VTile tile = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
                        ClickAttackNode(tile.Index);
                        return;
                    }
                }
            }
            cBattlefield.ActionEndHandler -= OnAttackComplete;
            ActionOver();
        }
        public void ActionOver(){
            this.mCharacter.ActionOver = true;
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
            Belong belong = this.mCharacter.Belong;
            this.mCharacter = null;
            if (!System.Array.Exists(mBaseMap.Characters, _ => _.Belong == belong && !_.ActionOver))
            {
                ChangeBelong(belong);
            }
            else
            {
                cBattlefield.CloseOperatingMenu();
            }
        }
        public void ChangeBelong(Belong belong){
            if (belong == Belong.self)
            {
                if (System.Array.Exists(mBaseMap.Characters, _ => _.Belong == Belong.friend && !_.ActionOver))
                {
                    cBattlefield.BoutWave(Belong.friend);
                }
                else
                {
                    cBattlefield.BoutWave(Belong.enemy);
                }
            }else if (belong == Belong.friend)
            {
                cBattlefield.BoutWave(Belong.enemy);
            }else if (belong == Belong.enemy)
            {
                cBattlefield.BoutWave(Belong.self);
            }
        }
        public void ClickMovingNode(int index){
            if (this.mCharacter.Belong != cBattlefield.currentBelong || this.mCharacter.ActionOver)
            {
                CharacterReturnNone();
                return;
            }
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter != null)
            {
                if (!cBattlefield.charactersManager.IsSameBelong(this.mCharacter.Belong, mCharacter.Belong))
                {
                    ClickAttackNode(index);
                }
                return;
            }
            //Debug.LogError("ClickMovingNode="+index+", " + cBattlefield.tilesManager.IsInMovingCurrentTiles(index));
            if (cBattlefield.tilesManager.IsInMovingCurrentTiles(index))
            {
                //Vector2 vec = new Vector2(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile startTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile endTile = cBattlefield.mapSearch.GetTile(index);

                Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
                    {
                        this.mCharacter.Action = ActionType.stand;
                        cBattlefield.tilesManager.ClearCurrentTiles();
                        cBattlefield.battleMode = CBattlefield.BattleMode.move_end;
                        this.mCharacter.CoordinateY = endTile.CoordinateY;
                        this.mCharacter.CoordinateX = endTile.CoordinateX;
                        cBattlefield.tilesManager.ShowCharacterAttackArea(this.mCharacter);
                        cBattlefield.OpenOperatingMenu();
                    };
                
                List<VTile> tiles = cBattlefield.aStar.Search(startTile, endTile);
                if (tiles.Count > 0)
                {
                    cBattlefield.CloseOperatingMenu();
                    cBattlefield.tilesManager.ClearCurrentTiles();
                    this.mCharacter.Action = ActionType.move;
                    cBattlefield.battleMode = CBattlefield.BattleMode.moving;
                    Sequence sequence = new Sequence();
                    foreach (VTile tile in tiles)
                    {
                        TweenParms tweenParms = new TweenParms().Prop("X", tile.transform.localPosition.x, false).Prop("Y", tile.transform.localPosition.y, false).Ease(EaseType.Linear);
                        if (tile.Index == endTile.Index)
                        {
                            tweenParms.OnComplete(moveComplete);
                        }
                        sequence.Append(HOTween.To(this.mCharacter, 0.5f, tweenParms));
                    }
                    sequence.Play();
                }
                else
                {
                    moveComplete();
                }
            }
            else
            {
                CharacterReturnNone();
            }

        }
        public void CharacterReturnNone(){
            returnAction();
            this.mCharacter = null;
            Debug.LogError("this.mCharacter = null;");
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
        }
    }
}