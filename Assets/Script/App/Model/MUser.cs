﻿using System.Collections;
using System.Collections.Generic;
namespace App.Model{
	public class MUser : MBase {
        public MUser(){
        }
        public int id;
        public string name;
        public string nickname;
        public string password;
        public int level;
        public int gold;
        public int silver;
        public int junling;
        public int map_id;
        public MTile[] top_map;
	}
}