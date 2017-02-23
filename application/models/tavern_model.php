<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Tavern_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function liqueur_list(){
		$this->master_db->select('id, name, gold, silver, from, to, free_time,free_count');
		$now = date("Y-m-d H:i:s",time());
		$this->master_db->where("(`from`='0000-00-00 00:00:00' OR `from`<'{$now}')");
		$this->master_db->where("(`to`='0000-00-00 00:00:00' OR `to`>'{$now}')");
		$query = $this->master_db->get(MASTER_LIQUEUR);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		$user = $this->getSessionData("user");
		foreach ($result as $key => $liqueur) {
			$log = $this->get_liqueur_free_log($user["id"],$liqueur["id"]);
			$free_cnt = 0;
			$last_log_time = null;
			if(!empty($log)){
				foreach ($log as $logchild) {
					if($logchild["register_time"] > DAY_START){
						$free_cnt++;
					}
				}
				$last_log_time = $log[0]["register_time"];
			}
			$liqueur["log_count"] = $free_cnt;
			$liqueur["last_log_time"] = $last_log_time;
			$result[$key] = $liqueur;
		}
		return $result;
	}
	function liqueur_buy($args){
		$liqueur = $this->get_liqueur_master($args["liqueur_id"]);
		if(!$liqueur){
			return null;
		}
		$now = date("Y-m-d H:i:s",time());
		if(($liqueur["from"] != "0000-00-00 00:00:00" && $liqueur["from"] > $now) || ($liqueur["to"] != "0000-00-00 00:00:00" && $liqueur["to"] < $now)){
			return null;
		}
		$user = $this->getSessionData("user");
		$gold = 0;
		$silver = 0;
		if($args["buy_type"] == "free"){
			$limit = $liqueur["free_count"];
			$log = $this->get_liqueur_free_log($user["id"],$liqueur["id"]);
			if(!empty($log)){
				$free_cnt = 0;
				foreach ($log as $logchild) {
					if($logchild["register_time"] > DAY_START){
						$free_cnt++;
					}
				}
				if($free_cnt >= $limit){
					return null;
				}
				$last_log = $log[0];
				if($last_log["register_time"] > date("Y-m-d H:i:s",strtotime("-".$liqueur["free_time"]." minute"))){
					return null;
				}
			}
		}else{
			$gold = $liqueur["gold"];
			$silver = $liqueur["silver"];
			if($user["gold"] < $gold || $user["silver"] < $silver){
				return null;
			}
		}
		
		$liqueur_gets = $this->get_liqueur_get($liqueur["id"]);
		$probability_sum = 0;
		foreach($liqueur_gets as $val){
			$probability_sum += $val["probability"];
		}
		$rand_index = rand(0,$probability_sum - 1);
		$liqueur_get = null;
		$probability_sum = 0;
		foreach($liqueur_gets as $val){
			$probability_sum += $val["probability"];
			if($rand_index <= $probability_sum){
				$liqueur_get = $val;
				break;
			}
		}
		$this->user_db->trans_begin();
		$error = false;
		//买酒记录(消费)
		if(!$error){
			$setlog = $this->set_liqueur_free_log($user["id"],$liqueur["id"],$gold,$silver);
			$error = ($setlog ? false : true);
		}
		//奖品
		if(!$error){
			$item_get = $this->item_model->set_item($user["id"], $liqueur_get["item_id"]);
			$error = ($item_get ? false : true);
		}
		if ($error || $this->user_db->trans_status() === FALSE)
		{
		    $this->user_db->trans_rollback();
			return null;
		}
		else
		{
		    $this->user_db->trans_commit();
		}
		$item = $this->item_model->get_master($liqueur_get["item_id"]);
		return $item;
	}
	function get_liqueur_master($id){
		$this->master_db->where('id', $id);
		$query = $this->master_db->get(MASTER_LIQUEUR);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function get_liqueur_get($liqueur_id){
		$this->master_db->where('liqueur_id', $liqueur_id);
		$query = $this->master_db->get(MASTER_LIQUEUR_GET);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function set_liqueur_free_log($user_id, $liqueur_id, $gold, $silver){
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('type', 'liqueur');
		$this->user_db->set('child_id', $liqueur_id);
		$this->user_db->set('gold', -$gold);
		$this->user_db->set('silver', -$silver);
		$this->user_db->set('register_time', date("Y-m-d H:i:s",time()));
		$res = $this->user_db->insert(USER_BANKBOOK);
		return $res;
	}
	function get_liqueur_free_log($user_id, $liqueur_id){
		$this->user_db->where('type', 'liqueur');
		$this->user_db->where('user_id', $user_id);
		$this->user_db->where('child_id', $liqueur_id);
		$this->user_db->where('gold', 0);
		$this->user_db->where('silver', 0);
		$this->user_db->where('register_time >', strtotime("-2 day"));
		//$this->user_db->where('register_time >', date("Y-m-d 00:00:00",time()));
		$this->user_db->order_by("id", "desc");
		$query = $this->user_db->get(USER_BANKBOOK);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
}
