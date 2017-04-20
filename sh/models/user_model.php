<?php 
class User_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	var $_select_clums = 'id, name, nickname as Nickname, face as Face, level as Level, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	var $_select_other_clums = 'id, name, nickname as Nickname, face as Face, level as Level, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	function register($args){
		$res = $this->login($args);
		if($res){
			return false;
		}
		$this->user_db->set('account',$args["account"]);
		$this->user_db->set('face',$args["face"]);
		$this->user_db->set('name',$args["name"]);
		$this->user_db->set('nickname',$args["nickname"]);
		$this->user_db->set('pass',$args["pass"]);
		$res = $this->user_db->insert(USER_PLAYER);
		if(!$res){
			return false;
		}
		return $this->user_db->insert_id();
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
