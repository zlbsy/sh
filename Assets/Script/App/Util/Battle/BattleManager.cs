using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;
using Holoville.HOTween;

namespace App.Util.Battle{
    public class BattleManager{
        private CBattlefield cBattlefield;
        private MCharacter mCharacter;
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
                this.mCharacter = mCharacter;
                cBattlefield.tilesManager.ShowCharacterMovingArea(mCharacter);
                cBattlefield.OpenBattleCharacterPreviewDialog(this.mCharacter);
            }
        }
        public void ClickMovingNode(int index){
            MCharacter mCharacter = GetCharacter(index);
            if (mCharacter != null)
            {
                return;
            }
            if (cBattlefield.tilesManager.IsMovingTile(index))
            {
                Vector2 vec = new Vector2(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                VTile startTile = cBattlefield.mapSearch.GetTile(vec);
                VTile endTile = cBattlefield.mapSearch.GetTile(index);

                Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
                    {
                        this.mCharacter.Action = ActionType.stand;
                        cBattlefield.tilesManager.ClearMovingTiles();
                        cBattlefield.battleMode = CBattlefield.BattleMode.move_end;
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
                this.mCharacter = null;
                cBattlefield.tilesManager.ClearMovingTiles();
                cBattlefield.HideBattleCharacterPreviewDialog();
                cBattlefield.battleMode = CBattlefield.BattleMode.none;
            }

        }
        public MCharacter GetCharacter(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, _=>_.CoordinateX == coordinate.x && _.CoordinateY == coordinate.y);
            return mCharacter;
        }
    }
}