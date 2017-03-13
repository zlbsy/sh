using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
	public class MBattlefield : MBase {
		public MBattlefield(){
		}
        public string[] enemys;
        public int map_id;
        public List<string> script;
        public MTile[] Tiles;
	}
}