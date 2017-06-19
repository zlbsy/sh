using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public enum AidType{
        none,
        physicalAttack,
        magicAttack,
        physicalDefense,
        magicDefense
    }
    public enum StrategyEffectType{
        aid,
        vampire
    }
    [System.Serializable]
    public class MStrategy : MBase {
        public MStrategy(){
        }
        public AidType aid_type;
        public StrategyEffectType effect_type;
        public float hert;
	}
}