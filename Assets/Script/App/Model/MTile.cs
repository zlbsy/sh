using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model{
    [System.Serializable]
	public class MTile : MBase {
        public MTile(){
		}
        public int id;//
        public int tile_id;//
        public App.Model.Master.MTile Master{
            get{ 
                return null;
            }
        }
	}
}