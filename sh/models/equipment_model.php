<?php 
class Equipment_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function set_equipment($user_id, $equipment_id, $equipment_type){
		$values = array();
		$values["user_id"] = "{$user_id}";
		$values["equipment_id"] = "{$equipment_id}";
		$values["equipment_type"] = "'{$equipment_type}'";
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		return $this->user_db->insert($values, $this->user_db->equipment);
	}
	
	function get_list($user_id, $chara_id = null){
		$select = "id as Id,character_id,equipment_id as EquipmentId,equipment_type as EquipmentType";
		$table = $this->user_db->equipment;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($chara_id != null){
			$where[] = "character_id={$chara_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function get_equipment($user_id, $equipment_id){
		$this->user_db->where("id", $equipment_id);
		$this->user_db->where("user_id", $user_id);
		$query = $this->user_db->get(USER_EQUIPMENT);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function get_equipment_by_position($user_id, $character_id, $position){
		$this->user_db->where("user_id", $user_id);
		$this->user_db->where("character_id", $character_id);
		$this->user_db->where("position", $position);
		$query = $this->user_db->get(USER_EQUIPMENT);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function equip($user_id, $equipment, $equipment_master, $character_id, &$update_character_ids){
		$equiped = $this->get_equipment_by_position($user_id, $character_id, $equipment_master["position"]);
		$this->user_db->trans_begin();
		$update_character_ids[] = $character_id;
		if($equipment["character_id"] > 0){
			$update_character_ids[] = $equipment["character_id"];
			//如果物品已被英雄装备，则先取消装备
			$this->user_db->set("character_id",0);
			$this->user_db->where("id", $equipment["id"]);
			$this->user_db->where("user_id", $user_id);
			$result_remove = $this->user_db->update(USER_EQUIPMENT);
			if(!$this->transBool($result_remove)){
				return null;
			}
		}
		if(!empty($equiped)){
			$update_character_ids[] = $equiped["character_id"];
			//如果英雄已装备物品，则先取消装备
			$this->user_db->set("character_id",0);
			$this->user_db->where("user_id", $user_id);
			$this->user_db->where("character_id", $character_id);
			$this->user_db->where("position", $equipment_master["position"]);
			$result_remove = $this->user_db->update(USER_EQUIPMENT);
			if(!$this->transBool($result_remove)){
				return null;
			}
		}
		//装备
		$this->user_db->set("character_id",$character_id);
		$this->user_db->set("position", $equipment_master["position"]);
		$this->user_db->where("user_id", $user_id);
		$this->user_db->where("id", $equipment["id"]);
		$result = $this->user_db->update(USER_EQUIPMENT);
		if(!$this->transBool($result)){
			return null;
		}
		if (!$this->transStatus()){
			return null;
		}
		return $result;
	}
	function get_master($id){
		$this->master_db->select('id, arms, level, star, position, img, explanation, five1, five2, five3, five4, five5, hp, attack, magic, def, magicDef, speed, dodge, breakout, strength, force, strategy, command, intelligence, agility');
		$this->master_db->where("id", $id);
		$query = $this->master_db->get(MASTER_EQUIPMENT);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
}
