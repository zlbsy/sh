<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
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
		$this->user_db->set('name',$args["name"]);
		$this->user_db->set('nickname',$args["name"]);
		$this->user_db->set('pass',$args["pass"]);
		$res = $this->user_db->insert(USER_PLAYER);
		if(!$res){
			return false;
		}
		return $this->user_db->insert_id();
	}
	function login($args){
		$this->user_db->select($this->_select_clums);
		$this->user_db->where('name',$args["name"]);
		$this->user_db->where('pass',$args["pass"]);
		$query = $this->user_db->get(USER_PLAYER);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function update($args){
		$DB = $this->user_db;
		if(!$args || !is_array($args))return false;
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$DB->set($key, $value);
		}
		$user = $this->getSessionData("user");
		$DB->where('id',$user["id"]);
		$res = $DB->update(USER_PLAYER);
		return $res;
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
		$this->user_db->select($select);
		$this->user_db->where('user_id',$user["id"]);
		$query = $this->user_db->get(USER_BANKBOOK);
		if ($query->num_rows() == 0){
			return 0;
		}
		$result = $query->first_row('array');
		return $this->update($result);
	}
	function get($id, $is_all = false){
		$user = $this->getSessionData("user");
		$is_self = ($id == $user["id"]);
		$this->user_db->select($is_self ? $this->_select_clums : $this->_select_other_clums);
		$this->user_db->where('id', $id);
		$query = $this->user_db->get(USER_PLAYER);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		if($is_self && $is_all){
			$result["TopMap"] = $this->get_top_map($id);
		}
		return $result;
	}
	function get_top_map($user_id){
		$this->user_db->select('id, user_id, num, tile_id, x, y, level');
		$this->user_db->where('user_id', $user_id);
		$query = $this->user_db->get(USER_TOP_MAP);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
}
