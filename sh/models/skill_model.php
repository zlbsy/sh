<?php 
class Skill_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_skill($id){
		$select = 'id, skill_id, level';
		$table = $this->user_db->character_skill;
		$where = array();
		$where[] = "id={$id}";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_master($skill_id, $level){
		$select = "`id`,`level`,`character_level`,`price`";
		$table = $this->master_db->skill;
		$where = array();
		$where[] = "id={$skill_id}";
		$where[] = "level={$level}";
		$result = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function set_levelup_log($user_id, $child_id, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'skill_levelup'";
		$values['child_id'] = $child_id;
		$values['gold'] = 0;
		$values['silver'] = -$silver;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
	function level_up($user_id, $id){
		$skill = $this->get_skill($id);
		if($skill == null){
			$this->error("get_skill error".$this->user_db->last_sql);
			return false;
		}
		$skillMaster = $this->get_master($skill["skill_id"], $skill["level"]);
		if($skillMaster == null){
			$this->error("get_master error".$this->master_db->last_sql);
			return false;
		}
		$user = $this->getSessionData("user");
		if($user["Silver"] < $skillMaster["price"]){
			$this->error("Silver error");
			return false;
		}
		$this->user_db->trans_begin();
		$use_money = $this->set_levelup_log($user_id, 0, $skillMaster["price"]);
		if($use_money == null){
			$this->user_db->trans_rollback();
			$this->error("use money error".$this->user_db->last_sql);
		}
		$user_model = new User_model();
		$user_silver_update = $user_model->update_silver();
		if($user_silver_update == null){
			$this->user_db->trans_rollback();
			$this->error("silver update error".$this->user_db->last_sql);
		}
		$skill_up = $this->update($id,array("level"=>$skill["level"] + 1));
		$this->user_db->trans_commit();
		return true;
	}
	function update($id, $args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$where = array("id={$id}");
		$table = $this->user_db->character_skill;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
}
