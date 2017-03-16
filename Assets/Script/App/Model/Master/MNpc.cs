using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MNpc : MBase {
        public int character_id;
        public int horse;//NpcEquipmentCacher id
        public int clothes;//NpcEquipmentCacher id
        public int weapon;//NpcEquipmentCacher id
        public int star;
	}
}