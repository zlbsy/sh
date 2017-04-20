<?php 
class User_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	var $_select_clums = 'id, name, nickname as Nickname, face as Face, level as Level, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	var $_select_other_clums = 'id, name, nickname as Nickname, face as Face, level as Level, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	function register($account, $password, $name, $character_id){
		$character_model = new Character_model();
		$character = $character_model->get_character_list(0, $character_id);
		if(is_null($character)){
			return false;
		}
		$this->user_db->trans_begin();
		$values = array();
		$values['account'] = "'{$account}'";
		$values['pass'] = "'{$password}'";
		$values['name'] = "'{$name}'";
		$values['face'] = $character_id;
		$now = date("Y-m-d H:i:s");
		$values['last_ap_date'] = "'{$now}'";
		$values['register_time'] = "'{$now}'";
		$res_player = $this->user_db->insert($values, $this->user_db->player);
		if(!$res_player){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		$user = $this->login(array("account"=>$account, "pass"=>$password));
		if(is_null($user)){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		$user_id = $user["Id"];
		$character_values = array();
		$character_values['user_id'] = $user_id;
		$character_values['character_id'] = $character_id;
		$character_values['star'] = $character["Star"];
		$character_values['weapon_type'] = "'".$character["WeaponType"]."'";
		$character_values['move_type'] = "'".$character["MoveType"]."'";
		$character_values['horse'] = $character["Horse"];
		$character_values['clothes'] = $character["Clothes"];
		$character_values['weapon'] = $character["Weapon"];
		$character_values['register_time'] = "'{$now}'";
		$res_character = $character_model->character_insert($character_values);
		if(!$res_character){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		$character_skills = array();
		$character_values['user_id'] = $user_id;
		$character_values['character_id'] = $character_id;
		$Skills = $character["Skills"];
		$character_skills['skill_id'] = $Skills["SkillId"];
		$character_skills['level'] = $Skills["Level"];
		$character_skills['register_time'] = "'{$now}'";
		$res_character = $character_model->character_skill_insert($character_skills);
		if(!$res_character){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		if($character["Horse"] > 0){
			$rs_horse = $equipment_mmodel->set_equipment($user_id, $character["Horse"], "horse", $character_id);
			if(!$rs_horse){
				$this->user_db->trans_rollback();
				$this->error("register fail");
			}
		}
		$rs_weapon = $equipment_mmodel->set_equipment($user_id, $character["Weapon"], "weapon", $character_id);
		if(!$rs_weapon){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		$rs_clothes = $equipment_mmodel->set_equipment($user_id, $character["Clothes"], "clothes", $character_id);
		if(!$rs_clothes){
			$this->user_db->trans_rollback();
			$this->error("register fail");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function login($args){
		$table = $this->user_db->player;
		$where = array();
		$where[] = "account='{$args["account"]}'";
		$where[] = "pass='{$args["pass"]}'";
		$result = $this->user_db->select($this->_select_clums, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function update($args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$user = $this->getSessionData("user");
		$where = array("id={$user["id"]}");
		$table = $this->user_db->player;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function update_silver(){
		return $this->update_bankbook('sum(silver) as silver');
	}
	function update_gold(){
		return $this->update_bankbook('sum(gold) as gold');
	}
	function update_money(){
		return $this->update_bankbook('sum(silver) as silver, sum(gold) as gold');
	}
	function update_bankbook($select){
		$user = $this->getSessionData("user");
		$table = $this->user_db->bankbook;
		$where = array("user_id={$user["id"]}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return false;
		}
		return $this->update($result);
	}
	function get($id, $is_all = false){
		$user = $this->getSessionData("user");
		$is_self = (isset($id) && $id == $user["id"]);
		$select = $is_self ? $this->_select_clums : $this->_select_other_clums;
		$table = $this->user_db->player;
		$where = array("id={$user["id"]}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return null;
		}
		if($is_self && $is_all){
			$result["TopMap"] = $this->get_top_map($id);
			$result["progress"] = $this->get_story_progress($id);
		}
		return $result;
	}
	function get_top_map($user_id){
		$select = "id, user_id, num, tile_id, x, y, level";
		$table = $this->user_db->top_map;
		$where = array("user_id={$user_id}");
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function get_story_progress($user_id){
		$select = "k, v";
		$table = $this->user_db->story_progress;
		$where = array("user_id={$user_id}");
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
}
