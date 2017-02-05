﻿using System.Collections;
using System.Collections.Generic;
namespace App.Model{
    [System.Serializable]
	public class MTile : MBase {
        public MTile(){
		}
        public int user_id;//
        public int num;//
        public int tile_id;//
        public int x;//
        public int y;//
        public int level;//
        public App.Model.Master.MTile Master{
            get{ 
                return App.Util.Cacher.TileCacher.Instance.Get(tile_id);
            }
        }
	}
}