using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MBattleNpc : MBase {
		public int level;
		/// <summary>
		/// NpcEquipmentCacher id
		/// 0表示使用MNpc的默认装备
		/// </summary>
		public int horse;
		/// <summary>
		/// NpcEquipmentCacher id
		/// 0表示使用MNpc的默认装备
		/// </summary>
		public int clothes;
		/// <summary>
		/// NpcEquipmentCacher id
		/// 0表示使用MNpc的默认装备
		/// </summary>
        public int weapon;
        public string move_type;
        public string weapon_type;
        public int star;
		public int x;
		public int y;
	}
}