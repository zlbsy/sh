<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Quest_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function chapter_list(){
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_CHAPTER);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function area_list($chapter_id){
		$this->master_db->select('id, name, chapter_id, img, x, y');
		$this->master_db->where("chapter_id", $chapter_id);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_AREA);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		$user = $this->getSessionData("user");
		$user_areas = $this->user_area_list($user, $chapter_id);
		if(empty($user_areas)){
			$insert_result = $this->set_user_area_list($user, $chapter_id, $result);
			if (!$insert_result){
				return null;
			}
			foreach ($result as $key=>$area) {
				$result[$key]["star"] = 0;
			}
		}else{
			foreach ($result as $key=>$area) {
				$k = multidimensional_search($user_areas, array("area_id"=>$area["id"]));
				$user_area = $user_areas[$k];
				$result[$key]["star"] = $user_area["star"];
			}
		}
		
		return $result;
	}
	function user_area_list($user, $chapter_id){
		$this->user_db->where("user_id", $user["id"]);
		$this->user_db->where("chapter_id", $chapter_id);
		$query = $this->user_db->get(USER_AREA);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function set_user_area_list($user, $chapter_id, $areas){
		$data = array();
		foreach ($areas as $area) {
			$data[] = array("user_id"=>$user["id"], "chapter_id"=>$chapter_id, "area_id"=>$area["id"], "register_time"=>date("Y-m-d H:i:s"));
		}
		return $this->user_db->insert_batch(USER_AREA, $data);
	}
	
	function stage_list($area_id){
		$this->master_db->select('id, name, area_id');
		$this->master_db->where("area_id", $area_id);
		$this->master_db->order_by("id", "asc");
		$query = $this->master_db->get(MASTER_STAGE);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		$user = $this->getSessionData("user");
		$user_stages = $this->user_stage_list($user, $area_id);
		if(empty($user_stages)){
			$insert_result = $this->set_user_stage_list($user, $area_id, $result);
			if (!$insert_result){
				return null;
			}
			foreach ($result as $key=>$stage) {
				$result[$key]["star"] = 0;
			}
		}else{
			foreach ($result as $key=>$stage) {
				$k = multidimensional_search($user_stages, array("stage_id"=>$stage["id"]));
				$user_stage = $user_stages[$k];
				$result[$key]["star"] = $user_stage["star"];
			}
		}
		foreach ($result as $key=>$stage) {
			$result[$key]["characters"] = $this->get_stage_characters($stage["id"]);
			$result[$key]["items"] = $this->get_stage_items($stage["id"]);
		}
		
		return $result;
	}
	function get_stage_characters($stage_id){
		$this->master_db->select('id,character_id, index, star, level');
		//$this->master_db->order_by("id", "asc");
		$this->master_db->order_by("index", "asc");
		$this->master_db->where("stage_id", $stage_id);
		$query = $this->master_db->get(MASTER_STAGE_CHARACTER);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_stage_items($stage_id, $select = null){
		$this->master_db->select($select ? $select : 'item_id');
		$this->master_db->order_by("id", "asc");
		//$this->master_db->order_by("item_id", "asc");
		$this->master_db->where("stage_id", $stage_id);
		$query = $this->master_db->get(MASTER_STAGE_GET);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_area_items($area_id, $select = null){
		$this->master_db->select($select ? $select : 'item_id');
		$this->master_db->order_by("id", "asc");
		//$this->master_db->order_by("item_id", "asc");
		$this->master_db->where("area_id", $area_id);
		$query = $this->master_db->get(MASTER_AREA_GET);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function user_stage_list($user, $area_id){
		$this->user_db->where("user_id", $user["id"]);
		$this->user_db->where("area_id", $area_id);
		$query = $this->user_db->get(USER_STAGE);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function set_user_stage_list($user, $area_id, $stages){
		$data = array();
		foreach ($stages as $stage) {
			$data[] = array("user_id"=>$user["id"], "area_id"=>$area_id, "stage_id"=>$stage["id"], "register_time"=>date("Y-m-d H:i:s"));
		}
		return $this->user_db->insert_batch(USER_STAGE, $data);
	}
}