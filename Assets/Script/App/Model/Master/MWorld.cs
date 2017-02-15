using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    /**
     * 州府
     */
    [System.Serializable]
	public class MWorld {
        public MWorld(){
        }
        public int id;
        public int tile_id;
        public int x;
        public int y;
        public int level;
	}
}