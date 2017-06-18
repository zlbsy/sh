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
using System.Linq;

namespace App.Util.Battle{
    /// <summary>
    /// 战场总操作相关
    /// </summary>
    public class BattleManager{
        private CBattlefield cBattlefield;
        private MCharacter mCharacter;
        private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        //private App.Model.Master.MBaseMap baseMapMaster;
        private System.Action returnAction;
        private List<MCharacter> actionCharacterList = new List<MCharacter>();
        public BattleManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            //vBaseMap = view;
            //baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
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
                cBattlefield.tilesManager.ShowCharacterSkillArea(mCharacter);
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
        public void ClickSkillNode(int index){
            MCharacter mCharacter = cBattlefield.charactersManager.GetCharacter(index);
            if (mCharacter == null || !cBattlefield.charactersManager.IsInSkillDistance(mCharacter, this.mCharacter))
            {
                CharacterReturnNone();
                return;
            }
            bool sameBelong = cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, this.mCharacter.Belong);
            bool useToEnemy = this.mCharacter.CurrentSkill.UseToEnemy;
            if (!(useToEnemy ^ sameBelong))
            {
                CAlertDialog.Show("belong不对");
                return;
            }
            this.mCharacter.Target = mCharacter;
            mCharacter.Target = this.mCharacter;
            //attackCharacterList.Clear();
            if (useToEnemy)
            {
                bool forceFirst = false;
                if (forceFirst && cBattlefield.charactersManager.IsInSkillDistance(this.mCharacter, mCharacter))
                {
                    //先手攻击
                    SetActionCharacterList(mCharacter, this.mCharacter, true);
                }
                else
                {
                    SetActionCharacterList(this.mCharacter, mCharacter, true);
                }
            }
            else
            {
                SetActionCharacterList(this.mCharacter, mCharacter, false);
            }
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.actioning;
            cBattlefield.ActionEndHandler += OnActionComplete;
            OnActionComplete();
        }
        private void SetActionCharacterList(MCharacter actionCharacter, MCharacter targetCharacter, bool canCounter){
            int count = cBattlefield.calculateManager.SkillCount(actionCharacter, targetCharacter);
            int countBack = count;
            while(count-- > 0){
                actionCharacterList.Add(actionCharacter);
            }
            if (!canCounter || !cBattlefield.calculateManager.CanCounterAttack(actionCharacter, targetCharacter, actionCharacter.CoordinateX, actionCharacter.CoordinateY, targetCharacter.CoordinateX, targetCharacter.CoordinateY))
            {
                return;
            }
            count = cBattlefield.calculateManager.CounterAttackCount(actionCharacter, targetCharacter);
            while (count-- > 0)
            {
                actionCharacterList.Add(targetCharacter);
            }
            //反击后反击
            if (actionCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.attack_back_attack)
            {
                while(countBack-- > 0){
                    actionCharacterList.Add(actionCharacter);
                }
            }
        }
        public void OnActionComplete(){
            if (actionCharacterList.Count > 0)
            {
                MCharacter currentCharacter = actionCharacterList[0];
                actionCharacterList.RemoveAt(0);
                bool isContinue = ActionStart(currentCharacter);
                if (isContinue)
                {
                    return;
                }
            }
            cBattlefield.ActionEndHandler -= OnActionComplete;
            ActionOver();
        }
        private bool ActionStart(MCharacter currentCharacter){
            if (currentCharacter.Hp > 0)
            {
                currentCharacter.Direction = (currentCharacter.X > currentCharacter.Target.X ? Direction.left : Direction.right);
                currentCharacter.Action = ActionType.attack;
                return true;
            }
            actionCharacterList.Clear();
            if (cBattlefield.charactersManager.IsSameCharacter(currentCharacter, this.mCharacter))
            {
                return true;
            }
            //是否引导攻击
            bool continueAttack = (this.mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.continue_attack);
            if (continueAttack)
            {
                VTile vTile = cBattlefield.mapSearch.GetTile(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                MCharacter mCharacter = System.Array.Find(mBaseMap.Characters, (c)=>{
                    if(c.Hp == 0){
                        return false;
                    }
                    if (cBattlefield.charactersManager.IsSameBelong(this.mCharacter.Belong, c.Belong))
                    {
                        return false;
                    }
                    bool canAttack = cBattlefield.charactersManager.IsInSkillDistance(c.CoordinateX, c.CoordinateY, vTile.CoordinateX, vTile.CoordinateY, this.mCharacter);
                    return canAttack;
                });
                if (mCharacter != null)
                {
                    cBattlefield.ActionEndHandler -= OnActionComplete;
                    VTile tile = cBattlefield.mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
                    ClickSkillNode(tile.Index);
                    return true;
                }
            }
            return false;
        }
        public void ActionOver(){
            MSkill skill = this.mCharacter.CurrentSkill;
            List<App.Model.Master.MSkillEffect> skillEffects = new List<App.Model.Master.MSkillEffect>();
            if (skill.Master.effect.enemy.count > 0 && skill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.attack_end)
            {
                //skillEffects.Add(skill.Master.effect.enemy);
                List<int> aids = skill.Master.effect.enemy.aids.ToList();
                int index = 0;
                List<App.Model.Master.MStrategy> strategys = new List<App.Model.Master.MStrategy>();
                while (index++ < skill.Master.effect.enemy.count)
                {
                    int i = Random.Range(0, aids.Count - 1);
                    App.Model.Master.MStrategy strategy = StrategyCacher.Instance.Get(aids[i]);
                    aids.RemoveAt(i);
                    strategys.Add(strategy);
                }
                foreach (App.Model.Master.MStrategy strategy in strategys)
                {
                    //TODO::特效
                    if(strategy.effect_type == App.Model.Master.StrategyEffectType.aid){
                        this.mCharacter.Target.AddAid(strategy);
                        VTile vTile = this.cBattlefield.mapSearch.GetTile(this.mCharacter.Target.CoordinateX, this.mCharacter.Target.CoordinateY);
                        this.cBattlefield.CreateEffect(strategy.hert > 0 ? "effect_up" : "effect_down", vTile.transform);
                    }
                }
            }else if (skill.Master.effect.self.count > 0 && skill.Master.effect.self.time == App.Model.Master.SkillEffectBegin.attack_end)
            {
                skillEffects.Add(skill.Master.effect.self);
            }
            if (skillEffects.Count > 0)
            {
                List<App.Model.Master.MStrategy> strategys = new List<App.Model.Master.MStrategy>();
                foreach (App.Model.Master.MSkillEffect skillEffect in skillEffects)
                {
                    List<int> aids = skillEffect.aids.ToList();
                    int index = 0;
                    while (index++ < skillEffect.count)
                    {
                        int i = Random.Range(0, aids.Count - 1);
                        App.Model.Master.MStrategy strategy = StrategyCacher.Instance.Get(aids[i]);
                        aids.RemoveAt(i);
                        strategys.Add(strategy);
                    }
                }
            }
            if (this.mCharacter.Target != null)
            {
                this.mCharacter.Target.Target = null;
                this.mCharacter.Target = null;
            }
            this.mCharacter.ActionOver = true;
            cBattlefield.tilesManager.ClearCurrentTiles();
            cBattlefield.CloseOperatingMenu();
            cBattlefield.HideBattleCharacterPreviewDialog();
            cBattlefield.battleMode = CBattlefield.BattleMode.none;
            Belong belong = this.mCharacter.Belong;
            this.mCharacter = null;
            if (!System.Array.Exists(mBaseMap.Characters, _ => _.Hp > 0 && _.Belong == Belong.enemy))
            {
                //敌军全灭
                Debug.LogError("敌军全灭");
                cBattlefield.BattleWin();
                return;
            }else if (!System.Array.Exists(mBaseMap.Characters, _ => _.Hp > 0 && _.Belong == Belong.self))
            {
                //我军全灭
                Debug.LogError("我军全灭");
                cBattlefield.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleFailDialog));
                return;
            }
            if (!System.Array.Exists(mBaseMap.Characters, _ => _.Hp > 0 && _.Belong == belong && !_.ActionOver))
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
                if (System.Array.Exists(mBaseMap.Characters, _ => _.Hp > 0 && _.Belong == Belong.friend && !_.ActionOver))
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
                bool sameBelong = cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, this.mCharacter.Belong);
                bool useToEnemy = this.mCharacter.CurrentSkill.UseToEnemy;
                if (useToEnemy ^ sameBelong)
                {
                    ClickSkillNode(index);
                }
                /*if (!cBattlefield.charactersManager.IsSameBelong(this.mCharacter.Belong, mCharacter.Belong))
                {
                    ClickSkillNode(index);
                }*/
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
                        this.mCharacter.Action = ActionType.idle;
                        cBattlefield.tilesManager.ClearCurrentTiles();
                        cBattlefield.battleMode = CBattlefield.BattleMode.move_end;
                        this.mCharacter.CoordinateY = endTile.CoordinateY;
                        this.mCharacter.CoordinateX = endTile.CoordinateX;
                        cBattlefield.tilesManager.ShowCharacterSkillArea(this.mCharacter);
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