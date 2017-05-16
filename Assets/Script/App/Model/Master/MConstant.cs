using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    /**
     * 静态变量类
     */
    [System.Serializable]
	public class MConstant:MBase {
        public MConstant(){
        }
        public int recover_ap_time;
        public int world_map_id = 1;
        public int tutorial_end = 5;
        public int[] female_heads;
        public float weak_hp = 0.2f;
	}
}