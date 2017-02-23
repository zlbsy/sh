<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Master_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_master_character(){
		//$this->master_db->select("`id`,`name`,`head`,`hat`,`qualification`,`force`,`intelligence`,`command`,`agility`,`luck`,`power`,concat('[',skill_1,',',skill_2,',',skill_3,',',skill_4,',',skill_5,']') AS skills,`initialHp`,`initialAttack`,`initialMagic`,`initialDef`,`initialMagicDef`,`initialSpeed`,`initialDodge`,`initialBreakout`", FALSE);
		$this->master_db->select("`id`,`name`,`nickname`,`head`,`hat`,`qualification`,`force`,`intelligence`,
			`command`,`agility`,`luck`,`power`,`moving_power`,`riding`,`walker`,`pike`,`sword`,
			`long_knife`,`knife`,`long_ax`,`ax`,`sticks`,`fist`,`archery`,`hidden_weapons`,`dual_wield`, `start_star`, `face_rect`", FALSE);
		$this->master_db->order_by("id", "asc");
		$query_character = $this->master_db->get(MASTER_BASE_CHARACTER);
		$result = $query_character->result_array();
		foreach($result as $k=>$value){
			$result[$k]['face_rect'] = explode(',', $value['face_rect']);
		}
		return $result;
	}
	function get_master_base_map(){
		$this->master_db->select("`id`,`width`,`height`,`tile_ids`", FALSE);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_BASE_MAP);
		$res = $query->result_array();
		$result = array();
		foreach ($res as $value) {
			$value["tile_ids"] = explode(",", $value["tile_ids"]);
			$result[] = $value;
		}
		return $result;
	}
	function get_master_tile(){
		$this->master_db->select("`id`,`name`,`strategy`,`heal`,`boat`", FALSE);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_TILE);
		$result = $query->result_array();
		return $result;
	}
	function get_master_building($level = null, $building_id = null){
		$this->master_db->select("`id`,`from_level`,`to_level`,`tile_id`,`sum`,`price`,`price_type`", FALSE);
		if(!is_null($level)){
			$this->master_db->where('from_level <=',$level);
			$this->master_db->where('to_level >=',$level);
			$this->master_db->where('tile_id',$building_id);
		}
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_BUILDING);
		if(!is_null($level)){
			$result = $query->first_row('array');
		}else{
			$result = $query->result_array();
		}
		return $result;
	}
	function get_master_world(){
		$this->master_db->select("`id`,`tile_id`,`x`,`y`,`level`,`map_id`", FALSE);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_WORLD);
		$result = $query->result_array();
		return $result;
	}
	function get_master_area(){
		$this->master_db->select("`id`,`world_id`,`tile_id`,`x`,`y`,`level`,`map_id`", FALSE);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_AREA);
		$result = $query->result_array();
		return $result;
	}
	function get_master_constant(){
		$this->master_db->select("`name`,`val`", FALSE);
		$query = $this->master_db->get(MASTER_CONSTANT);
		if ($query->num_rows() == 0){
			return null;
		}
		$constants = $query->result_array();
		$result = array();
		foreach($constants as $value){
			$result[$value["name"]] = $value["val"];
		}
		return $result;
	}
	function get_master_character_star(){
		$this->master_db->select("grade,star,cost");
		$this->master_db->order_by("grade", "asc");
		$query_character = $this->master_db->get(MASTER_CHARACTER_STAR);
		$result = $query_character->result_array();
		return $result;
	}
	function get_master_items(){
		$this->master_db->select('id, name, type, child_id, price, explanation');
		$query = $this->master_db->get(MASTER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_master_exps(){
		$this->master_db->select('id, value');
		$query = $this->master_db->get(MASTER_EXP);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_master_growing(){
		$this->master_db->select('id, character_id, star, strength, force, strategy, command, intelligence, agility');
		$query = $this->master_db->get(MASTER_GROWING);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_master_equipment(){
		$this->master_db->select('id, arms, level, star, position, img, explanation, five1, five2, five3, five4, five5, hp, attack, magic, def, magicDef, speed, dodge, breakout, strength, force, strategy, command, intelligence, agility');
		$query = $this->master_db->get(MASTER_EQUIPMENT);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_master_skill(){
		$this->master_db->select('id, five');
		$query = $this->master_db->get(MASTER_SKILL);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
}
