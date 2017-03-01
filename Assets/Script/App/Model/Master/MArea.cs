using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    /**
     * 州府
     */
    [System.Serializable]
    public class MArea : App.Model.MTile {
        public MArea(){
        }
        public int world_id;
        public int map_id;
        public App.Model.MTile[] stages;
	}
}