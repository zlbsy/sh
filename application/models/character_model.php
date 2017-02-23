<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Character_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_cost_from_star($star){
		$this->master_db->select("grade,cost");
		$this->master_db->where("star", $star);
		$query_character = $this->master_db->get(MASTER_CHARACTER_STAR);
		if ($query_character->num_rows() == 0){
			return null;
		}
		$result = $query_character->first_row('array');
		return $result;
	}
	function get_character_list($user_id, $chara_id = null){
		//$this->user_db->select("`id`,`character_id`,`exp`,`level`,`star`,concat('[',skill_1,',',skill_2,',',skill_3,',',skill_4,',',skill_5,']') AS skill_levels", FALSE);
		$this->user_db->select("`id`, `character_id` as `CharacterId`, `exp` as `Exp`, `star` as `Star`, `level` as `Level`, `weapon_type` as `WeaponType`, `move_type` as `MoveType`, `horse` as `Horse`, `clothes` as `Clothes`, `weapon` as `Weapon`", FALSE);
		$this->user_db->where("user_id", $user_id);
		if($chara_id != null){
			if(is_array($chara_id)){
				$this->user_db->where_in("character_id", $chara_id);
			}else{
				$this->user_db->where("character_id", $chara_id);
			}
		}
		$query_character = $this->user_db->get(USER_CHARACTERS);
		$user = $this->getSessionData("user");
		$result = $query_character->result_array();
		/*foreach ($result as $key=>$character) {
			$result[$key]["equipments"] = $this->equipment_model->get_list($user["id"],$character["character_id"]);
		}*/
		return $result;
	}
	function get_master_character($chara_id){
		$this->master_db->select("`id`,`arms`,`armsKind`,`cost`,`five`,`start_star`,`faceImg`,`minFace`,`actions`,`soldiers`,`lineups`,concat('[',skill_1,',',skill_2,',',skill_3,',',skill_4,',',skill_5,']') AS skills,`initialHp`,`initialAttack`,`initialMagic`,`initialDef`,`initialMagicDef`,`initialSpeed`,`initialDodge`,`initialBreakout`", FALSE);
		$this->master_db->where("id", $chara_id);
		$query_character = $this->master_db->get(MASTER_CHARACTER);
		if ($query_character->num_rows() == 0){
			return null;
		}
		$result = $query_character->first_row('array');
		return $result;
	}
	function set_character($character){
		$user = $this->getSessionData("user");
		$this->user_db->set("user_id",$user['id']);
		$this->user_db->set("character_id",$character['id']);
		$this->user_db->set("exp",0);
		$this->user_db->set("star",$character['start_star']);
		$this->user_db->set("level",1);
		$result = $this->user_db->insert(USER_CHARACTER);
		return $result;
	}
	function update($id, $params){
		$this->user_db->set($params);
		$this->user_db->where("id",$id);
		$result = $this->user_db->update(USER_CHARACTER);
		return $result;
	}
}
