using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MSkill : MBase {
        public enum SkillType
        {
            attack
        }
        public MSkill(){
        }
        public string name;//
        public SkillType type;//
        public int price;
	}
}