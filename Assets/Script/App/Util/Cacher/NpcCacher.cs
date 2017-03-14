﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher{
    public class NpcCacher: CacherBase<NpcCacher, App.Model.Master.MNpc> {
        public App.Model.MCharacter GetFromNpc(int npcId){
            App.Model.Master.MNpc npc = Get(npcId);
            return GetFromNpc(npc);
        }
        public App.Model.MCharacter GetFromBattleNpc(App.Model.Master.MBattleNpc mBattleNpc){
            App.Model.Master.MNpc npc = Get(mBattleNpc.id);
            App.Model.MCharacter mCharacter = GetFromNpc(npc);
            if (mBattleNpc.horse > 0)
            {
                mCharacter.Horse = mBattleNpc.horse;
            }
            if (mBattleNpc.weapon > 0)
            {
                mCharacter.Weapon = mBattleNpc.weapon;
            }
            if (mBattleNpc.clothes > 0)
            {
                mCharacter.Clothes = mBattleNpc.clothes;
            }
            if (mBattleNpc.star > 0)
            {
                mCharacter.Star = mBattleNpc.star;
            }
            mCharacter.X = mBattleNpc.x;
            mCharacter.Y = mBattleNpc.y;

            return mCharacter;
        }
        public App.Model.MCharacter GetFromNpc(App.Model.Master.MNpc npc){
            return App.Model.MCharacter.Create(npc);
        }
    }
}