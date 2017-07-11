<?php 
class MissionStatus{
	const init = "init";
	const clear = "clear";
	const complete = "complete";
}
class Mission_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_master_missions(){
		$select = "`id`,`mission_type`,`start_time`, `end_time`, `parent_id`, `battle_id`, 
		`story_progress`, `level`, `character_count`, `battle_count`, `rewards`";
		$table =  $this->master_db->mission;
		$result = $this->master_db->select($select, $table);
		return $result;
	}
	function get_mission_list($user_id){
		$select = "`id` as Id, `user_id` as UserId, `mission_id` as MissionId, `status` as Status, `counts` as Counts, `update_time`";
		$table = $this->user_db->mission;
		$where = array("`user_id`={$user_id}");
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_mission($masters, $mission_id){
		foreach ($masters as $mission) {
			if($mission["id"] == $mission_id){
				return $mission;
			}
		}
		return null;
	}
	function complete($user, $mission){
		$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
		$contents = array();
		$rewards = json_decode($mission_master["rewards"],true);
		foreach ($rewards as $content) {
			$content = json_decode($battlefield_reward["content"],true);
			$content["get_type"] = "mission";
			$contents[] = $content;
		}
		$rewards = json_decode($mission_master["rewards"]);
		$this->user_db->trans_begin();
		$user_model = new User_model();
		$contents_res = $user_model->set_contents($user["id"],$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$update_res = $this->update($mission["MissionId"], array("status"=>MissionStatus::complete));
		if(!$update_res){
			$this->user_db->trans_rollback();
			$this->error("mission complete error ");
		}
		$user = $this->getSessionData("user");
		$user["missions"]=$mission_model->get_mission_list($user["id"]);
		$this->setSessionData("user",$user);
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
		$table = $this->user_db->equipment;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function battle_mission_change($user, $BattlingId, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $user["mission_masters"];
		$mission_change = false;
		foreach ($missions as $key=>$mission) {
			if($mission["status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["battle_id"] == $BattlingId){
				$change = $this->update($mission["Id"], array("status"=>MissionStatus::clear));
				if(!$change){
					return false;
				}
				$mission_change  = true;
			}else if($mission_master["battle_count"] > 0){
				$args = array("counts"=>$mission["counts"] + 1);
				if($mission["counts"] + 1 >= $mission_master["battle_count"]){
					$args["status"] = MissionStatus::clear;
				}
				$change = $this->update($mission["Id"], $args);
				if(!$change){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function character_mission_change($user, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $user["mission_masters"];
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $key=>$mission) {
			if($mission["status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["character_count"] > 0 && $mission["counts"] != $character_count){
				$args = array("counts"=>$mission["counts"] + 1);
				if($mission["counts"] + 1 >= $mission_master["character_count"]){
					$args["status"] = MissionStatus::clear;
				}
				$change = $this->update($mission["Id"], $args);
				if(!$change){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function level_mission_change($user, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $user["mission_masters"];
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $key=>$mission) {
			if($mission["status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["level"] > 0 && $user["Level"] >= $mission_master["level"]){
				$change = $this->update($mission["Id"], array("status"=>MissionStatus::clear));
				if(!$change){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function progress_mission_change($user, $key, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $user["mission_masters"];
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $key=>$mission) {
			if($mission["status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if(!empty($mission_master["story_progress"]) && $mission_master["story_progress"] == $key){
				$change = $this->update($mission["Id"], array("status"=>MissionStatus::clear));
				if(!$change){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
}
