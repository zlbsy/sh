using System.Collections;
using System.Collections.Generic;
namespace App.Model.Master{
    [System.Serializable]
	public class MEquipment : MBase {
        public enum EquipmentType
        {
            /// <summary>
            /// 武器
            /// </summary>
            weapon,
            /// <summary>
            /// 马
            /// </summary>
            horse,
            /// <summary>
            /// 衣服
            /// </summary>
            clothes
        }
        public enum ClothesType
        {
            /// <summary>
            /// 铠甲
            /// </summary>
            armor,
            /// <summary>
            /// 布衣
            /// </summary>
            commoner
        }
        public MEquipment(){
		}
        public string name;//
        public WeaponType weapon_type;//武器类型
        public MoveType move_type;//移动类型
        public ClothesType clothes_type;//衣服类型
        public int physical_attack;//物理攻击
        public int magic_attack;//魔法攻击
        public int power;//力量
        public int move_power;//轻功／移动力
        public int hp;//血量
        public int speed;//速度
        public int physical_defense;//物理防御
        public int magic_defense;//魔法防御

        public int image_index;//马匹或鞋
        public int saddle;//马铠
	}
}