<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Gacha_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function log($text){
		$fp = fopen('/var/www/d/test/test.txt','a+');
		fwrite( $fp, $text."\n" );
		fclose( $fp );
	}
	function get_free_logs(){
		$this->master_db->select('id, gold, silver, from_time, to_time, free_time,free_count');
		$now = date("Y-m-d H:i:s",time());
		$this->master_db->where("(`from_time`='0000-00-00 00:00:00' OR `from_time`<'{$now}')");
		$this->master_db->where("(`to_time`='0000-00-00 00:00:00' OR `to_time`>'{$now}')");
		$query = $this->master_db->get(MASTER_GACHA);
		if ($query->num_rows() == 0){
			return array();
		}
		$result = $query->result_array();
		$user = $this->getSessionData("user");
		$freelogs = array();
		foreach ($result as $key => $gacha) {
			$freelog = array();
			$log = $this->get_free_log($user["id"],$gacha["id"]);
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
			$freelog["GachaId"] = $gacha["id"];
			$freelog["LimitCount"] = $gacha["free_count"] - $free_cnt;
			$freelog["LastTime"] = $last_log_time;
			$freelogs[] = $freelog;
		}
		return $freelogs;
	}
	function get_free_log($user_id, $gacha_id){
		$this->user_db->where('type', 'gacha');
		$this->user_db->where('user_id', $user_id);
		$this->user_db->where('child_id', $gacha_id);
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
	function can_slot($args, $gacha){
		if(!$gacha){
			return false;
		}
		$now = date("Y-m-d H:i:s",time());
		if(($gacha["from_time"] != "0000-00-00 00:00:00" && $gacha["from_time"] > $now) || ($gacha["to_time"] != "0000-00-00 00:00:00" && $gacha["to_time"] < $now)){
			return false;
		}
		return true;
	}
	function slot($args){return null;
		$gacha = $this->get_gacha_master($args["gacha_id"]);
	/*	if(!$this->can_slot($args, $gacha)){
			$this->error("can not gacha");
		}
		$user = $this->getSessionData("user");
		$this->log("can gacha");
		$gold = 0;
		$silver = 0;
		if(isset($args["buy_type"]) && $args["buy_type"] == "free"){
			$limit = $gacha["free_count"];
			$log = $this->get_free_log($user["id"], $gacha["id"]);
		if($args["buy_type"] == "free"){
			$log = $this->get_free_log($user["id"],$gacha["id"]);
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
			$limit_count = $gacha["free_count"] - $free_cnt;
			if($limit_count <= 0){
				return false;
			}
		}
		return true;
	}
	function slot($args){
		$gacha = $this->get_gacha_master($args["gacha_id"]);
		if(!$gacha || !$this->can_gacha($args, $gacha)){
			return null;
		}
		$now = date("Y-m-d H:i:s",time());
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
				if($last_log["register_time"] > date("Y-m-d H:i:s",strtotime("-".$gacha["free_time"]." minute"))){
					return null;
				}
			}
		}else{
			$cnt = $args["cnt"];
			if($cnt == 10){
				$cnt = 9;
			}
			$gold = $gacha["Gold"] * $cnt;
			$silver = $gacha["Silver"] * $cnt;
			if($user["Gold"] < $gold || $user["Silver"] < $silver){
				$this->error("no money " . $user["Gold"] . " < " . $gold . ", " . $user["Silver"] . " < " . $silver);
			}
		}
	*/	
	//	$gacha_childs = $this->get_gacha_childs($gacha["id"]);
		$slot_list = array();
	/*	$i = 0;
		$this->log("gacha_childs=".count($gacha_childs));
		while(count($slot_list) < $args["cnt"]){
			$probability_sum = 0;
			foreach($gacha_childs as $val){
				$probability_sum += $val["probability"];
			}
			$rand_index = rand(0,$probability_sum - 1);
			$this->log("rand_index=".$rand_index.", probability_sum=".$probability_sum);
			$probability_sum = 0;
			foreach($gacha_childs as $val){
				$probability_sum += $val["probability"];
				$this->log($rand_index . "<" . $probability_sum);
				if($rand_index <= $probability_sum){
					$slot_list[] = $val;
					break;
				}
			}
			$this->log("slot_list_count=". count($slot_list));
		}
		$this->user_db->trans_begin();
		$error = false;
		//购买记录(消费)
		if(!$error){
			$setlog = $this->set_gacha_log($user["id"], $gacha["id"], $gold, $silver);
			$error = ($setlog ? false : true);
		}
		$this->log("set_gacha_log");
		//奖品
		if(!$error){
			for($i=0;!$error && $i<count($slot_list);$i++){
				$gacha_child = $slot_list[$i];
				$this->log("i=".$i.", ".$gacha_child["type"]);
				switch($gacha_child["type"]){
					case "item":
						$gacha_get = $this->item_model->set_item($user["id"], $gacha_child["child_id"]);
						break;
					case "horse":
					case "weapon":
					case "clothes":
						$gacha_get = $this->equipment_model->set_equipment($user["id"], $gacha_child["child_id"], $gacha_child["type"]);
						break;
				}
				$error = ($gacha_get ? false : true);
			}
		}*/
		$this->log("gacha get over");
=======
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
>>>>>>> f8bf56028c81302c743b1ced789a3322d4873d9c
		if ($error || $this->user_db->trans_status() === FALSE)
		{
		    $this->user_db->trans_rollback();
			return null;
		}
		else
		{
		    $this->user_db->trans_commit();
		}
<<<<<<< HEAD
		$contents = array();
		foreach ($slot_list as $slot) {
			$contents[] = array("type"=>$slot["type"], "content_id"=>$slot["child_id"]);
		}
		return $contents;
	}
	function get_gacha_master($id){
		$this->master_db->where('id', $id);
		$query = $this->master_db->get(MASTER_GACHA);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function get_gacha_childs($gacha_id){
		$this->master_db->where('gacha_id', $gacha_id);
		$query = $this->master_db->get(MASTER_GACHA_CHILD);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function set_gacha_log($user_id, $gacha_id, $gold, $silver){
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('type', 'gacha');
		$this->user_db->set('child_id', $gacha_id);
		$this->user_db->set('gold', -$gold);
		$this->user_db->set('silver', -$silver);
		$this->user_db->set('register_time', date("Y-m-d H:i:s",time()));
		$res = $this->user_db->insert(USER_BANKBOOK);
		return $res;
=======
		$item = $this->item_model->get_master($liqueur_get["item_id"]);
		return $item;
>>>>>>> f8bf56028c81302c743b1ced789a3322d4873d9c
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
