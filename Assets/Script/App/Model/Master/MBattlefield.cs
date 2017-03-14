using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
	public class MBattlefield : MBase {
		public MBattlefield(){
        }
        public string name;
        public MBattleNpc[] enemys;
        public int[][] own_positions;
        public int map_id;
        public List<string> script;
        public App.Model.MTile[] tiles;
	}
}