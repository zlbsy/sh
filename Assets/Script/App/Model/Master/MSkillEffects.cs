using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MSkillEffects : MBase {
        public MSkillEffects(){
        }
        public MSkillEffect enemy;
        public MSkillEffect self;
	}
}