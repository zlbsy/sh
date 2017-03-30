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
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter != null && !cBattlefield.GetCharacterView(mCharacter).Gray)
            {
                this.mCharacter = mCharacter;
                cBattlefield.tilesManager.ShowCharacterMovingArea(mCharacter);
                cBattlefield.OpenBattleCharacterPreviewDialog(this.mCharacter);
                int cx = mCharacter.CoordinateX;
                int cy = mCharacter.CoordinateY;
                Direction direction = mCharacter.Direction;
                returnAction = () =>
                    {
                        this.mCharacter.CoordinateY = cy;
                        this.mCharacter.CoordinateX = cx;
                        this.mCharacter.Direction = direction;
                };
            }
        }
        public void ClickAttackNode(int index){
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter == null)
            {
                CharacterReturnNone();
                return;
            }
            if (mCharacter.Belong == this.mCharacter.Belong)
            {
                CAlertDialog.Show("不能打自己人");
                return;
            }
            this.mCharacter.Target = mCharacter;
            mCharacter.Target = this.mCharacter;

            attackCharacterList.Add(this.mCharacter);
            attackCharacterList.Add(this.mCharacter);
            attackCharacterList.Add(mCharacter);

            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.attacking;
            cBattlefield.ActionEndHandler += OnAttackComplete;
            OnAttackComplete();
        }
        public void OnAttackComplete(){
            if (attackCharacterList.Count > 0)
            {
                MCharacter currentCharacter = attackCharacterList[0];
                currentCharacter.Direction = (currentCharacter.X > currentCharacter.Target.X ? Direction.left : Direction.right);
                currentCharacter.Action = ActionType.attack;
                attackCharacterList.RemoveAt(0);
                return;
            }
            cBattlefield.ActionEndHandler -= OnAttackComplete;
            App.View.Character.VCharacter character = cBattlefield.GetCharacterView(this.mCharacter);
            character.Gray = true;
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
        }
        public void ClickMovingNode(int index){
            if (this.mCharacter.Belong != Belong.self)
            {
                CharacterReturnNone();
                return;
            }
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter != null)
            {
                return;
            }
            if (cBattlefield.tilesManager.IsInCurrentTiles(index))
            {
                Vector2 vec = new Vector2(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile startTile = cBattlefield.mapSearch.GetTile(vec);
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
        public MCharacter GetCharacter(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.CoordinateX == coordinate.x && _.CoordinateY == coordinate.y);
            return mCharacter;
        }
        public void CharacterReturnNone(){
            returnAction();
            this.mCharacter = null;
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
        }
    }
}