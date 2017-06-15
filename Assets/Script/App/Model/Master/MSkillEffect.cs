using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    public enum SkillEffectBegin{
        attack_start,
        attack_end
    }
    [System.Serializable]
	public class MSkillEffect : MBase {
        public MSkillEffect(){
        }
        public int[] aids;
        public int count;
        public SkillEffectBegin time;
	}
}