<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Battle_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function win($stage_id, $star){
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('item_id', $item_id);
		$this->user_db->set('register_time', date("Y-m-d 00:00:00",time()));
		$res = $this->user_db->insert(USER_ITEM);
		return $res;
	}
	function get_master($item_id){
		$this->master_db->where('id', $item_id);
		$query = $this->master_db->get(MASTER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function get_master_items(){
		$query = $this->master_db->get(MASTER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
}