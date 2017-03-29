<?php 
class Character_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_character_list($user_id, $chara_id = null){
		$select = "`id` as Id,`user_id` as UserId, `character_id` as `CharacterId`, `exp` as `Exp`, `star` as `Star`, `level` as `Level`, `weapon_type` as `WeaponType`, `move_type` as `MoveType`, `horse` as `Horse`, `clothes` as `Clothes`, `weapon` as `Weapon`";
		$table = $this->user_db->characters;
		$where = array();
		$where[] = "user_id = {$user_id}";
		if($chara_id != null){
			if(is_array($chara_id)){
				$where[] = "character_id in (".implode(", ", $chara_id).") ";
			}else{
				$where[] = "character_id = {$chara_id}";
			}
		}
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
}
