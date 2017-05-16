<?php 
class Character_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_character_list($user_id, $chara_id = null){
		$select = "`id` as Id,`user_id` as UserId, `character_id` as `CharacterId`, `exp` as `Exp`, `star` as `Star`, `level` as `Level`, `move_type` as `MoveType`, `horse` as `Horse`, `clothes` as `Clothes`, `weapon` as `Weapon`";
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
		
		$result_select = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["Skills"] = $this->get_character_skills($user_id, $row["CharacterId"]);
			$result[] = $row;
		}
		return $result;
	}
	function get_character_skills($user_id, $character_id){
		$select = "skill_id as SkillId, level as Level";
		$table = $this->user_db->character_skill;
		$where = array();
		$where[] = "user_id = {$user_id}";
		$where[] = "character_id = {$character_id}";
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function character_insert($values){
		$res = $this->user_db->insert($values, $this->user_db->characters);
		return $res;
	}
	function character_skill_insert($values){
		$res = $this->user_db->insert($values, $this->user_db->character_skill);
		return $res;
	}
}
