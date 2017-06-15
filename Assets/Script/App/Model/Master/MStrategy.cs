using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public enum StrategyType{
        physicalAttack,
        magicAttack,
        physicalDefense,
        magicDefense
    }
    public enum StrategyEffectType{
        aid
    }
    [System.Serializable]
    public class MStrategy : MBase {
        public MStrategy(){
        }
        public StrategyType type;
        public StrategyEffectType effect_type;
        public float hert;
	}
}