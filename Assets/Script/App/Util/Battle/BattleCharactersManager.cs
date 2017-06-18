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
    public class BattleCharactersManager{
        private CBattlefield cBattlefield;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public BattleCharactersManager(CBattlefield controller, MBaseMap model, VBaseMap view){
            cBattlefield = controller;
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(MCharacter checkCharacter, MCharacter distanceCharacter){
            Debug.LogError("checkCharacter = " + checkCharacter);
            Debug.LogError("distanceCharacter = " + distanceCharacter);
            return IsInSkillDistance(checkCharacter.CoordinateX, checkCharacter.CoordinateY, distanceCharacter.CoordinateX, distanceCharacter.CoordinateY, distanceCharacter);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(int CoordinateX, int CoordinateY, int targetX, int targetY, MCharacter distanceCharacter){
            MSkill targetSkill = distanceCharacter.CurrentSkill;
            Debug.LogError("targetSkill="+targetSkill);
            App.Model.Master.MSkill targetSkillMaster = targetSkill.Master;
            int distance = cBattlefield.mapSearch.GetDistance(CoordinateX, CoordinateY, targetX, targetY);
            return distance >= targetSkillMaster.distance[0] && distance <= targetSkillMaster.distance[1];
        }
        /// <summary>
        /// 获取攻击到的所有敌人
        /// </summary>
        /// <returns>The damage characters.</returns>
        /// <param name="vCharacter">攻击方</param>
        /// <param name="targetView">攻击目标</param>
        /// <param name="skill">Skill.</param>
        public List<VCharacter> GetTargetCharacters(VCharacter vCharacter, VCharacter targetView, App.Model.Master.MSkill skill){
            List<VCharacter> result = new List<VCharacter>(){ targetView };
            if (skill.radius_type == RadiusType.point)
            {
                return result;
            }
            List<VCharacter> characters;
            if (System.Array.Exists(skill.types, s => s == SkillType.heal))
            {
                characters = vBaseMap.Characters.FindAll(_=>this.IsSameBelong(_.ViewModel.Belong.Value, vCharacter.ViewModel.Belong.Value));
            }
            else/* if (System.Array.Exists(skill.types, s => s == SkillType.attack))*/
            {
                characters = vBaseMap.Characters.FindAll(_=>this.IsSameBelong(_.ViewModel.Belong.Value, targetView.ViewModel.Belong.Value));
            }
            VTile targetTile = cBattlefield.mapSearch.GetTile(targetView.ViewModel.CoordinateX.Value, targetView.ViewModel.CoordinateY.Value);
            if (skill.radius_type == RadiusType.range)
            {
                foreach (VCharacter child in characters)
                {
                    VTile tile = cBattlefield.mapSearch.GetTile(child.ViewModel.CoordinateX.Value, child.ViewModel.CoordinateY.Value);
                    if (cBattlefield.mapSearch.GetDistance(targetTile, tile) <= skill.radius)
                    {
                        result.Add(child);
                    }
                }
            }else if (skill.radius_type == RadiusType.direction)
            {
                VTile tile = cBattlefield.mapSearch.GetTile(vCharacter.ViewModel.CoordinateX.Value, vCharacter.ViewModel.CoordinateY.Value);
                int distance = cBattlefield.mapSearch.GetDistance(targetTile, tile);
                if (distance > 1)
                {
                    return result;
                }
                var direction = cBattlefield.mapSearch.GetDirection(tile, targetTile);
                var radius = skill.radius;
                while (radius-- > 0)
                {
                    tile = cBattlefield.mapSearch.GetTile(targetTile, direction);
                    VCharacter child = GetCharacter(tile.Index, characters);
                    if (child == null)
                    {
                        break;
                    }
                    result.Add(child);
                    targetTile = tile;
                }
            }
            return result;
        }
        public bool IsSameCharacter(MCharacter character1, MCharacter character2){
            return character1.Belong == character2.Belong && character1.Id == character2.Id;
        }
        public bool IsSameBelong(Belong belong1, Belong belong2){
            if (belong1 == Belong.enemy)
            {
                return belong2 == Belong.enemy;
            }
            return belong2 == Belong.self || belong2 == Belong.friend;
        }
        public void ActionRestore(){
            foreach (MCharacter character in this.mBaseMap.Characters)
            {
                if (character.ActionOver)
                {
                    character.ActionOver = false;
                    character.Action = ActionType.idle;
                }
            }
        }
        public MCharacter GetCharacter(int index, MCharacter[] characters = null){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            MCharacter mCharacter = System.Array.Find(characters == null ? mBaseMap.Characters : characters, _=>_.CoordinateX == coordinate.x && _.CoordinateY == coordinate.y);
            return mCharacter;
        }
        public App.View.Character.VCharacter GetCharacter(int index, List<App.View.Character.VCharacter> characters){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            App.View.Character.VCharacter vCharacter = characters.Find(_=>_.ViewModel.CoordinateX.Value == coordinate.x && _.ViewModel.CoordinateY.Value == coordinate.y);
            return vCharacter;
        }
    }
}