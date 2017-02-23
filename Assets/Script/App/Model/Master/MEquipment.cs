using System.Collections;
using System.Collections.Generic;
namespace App.Model.Master{
    [System.Serializable]
	public class MEquipment : MBase {
        public enum EquipmentType
        {
            weapon,
            horse,
            clothes
        }
        public MEquipment(){
		}
        public int name;//
        public int physical_attack;//物理攻击
        public int magic_attack;//魔法攻击
        public int power;//力量
        public int move_power;//轻功／移动力
        public int hp;//血量
        public int physical_defense;////物理防御
        public int magic_defense;////魔法防御
	}
}