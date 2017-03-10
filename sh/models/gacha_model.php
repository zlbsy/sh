<?php 
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
		$gachas = $this->get_gacha_masters(null, date("Y-m-d H:i:s",time()));
		$user = $this->getSessionData("user");
		$freelogs = array();
		foreach ($gachas as $key => $gacha) {
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
		$select = "id, user_id, gacha_id, register_time";
		$table = $this->user_db->gacha_free_log;
		$where = array();
		$where[] = "`user_id`={$user_id}";
		$where[] = "`gacha_id`={$gacha_id}";
		$where[] = "`register_time`>".strtotime("-2 day");
		$order_by = "id desc";
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function set_gacha_free_log($user_id, $gacha_id){
		$values = array();
		$values["user_id"] = $user_id;
		$values["gacha_id"] = $gacha_id;
		$values["register_time"] = date("Y-m-d H:i:s",time());
		$table = $this->user_db->gacha_free_log;
		$res = $this->user_db->insert($values, $table);
		return $res;
	}
	function slot($args){
		$gachas = $this->get_gacha_masters($args["gacha_id"]);
		if(count($gachas) == 0){
			$this->error("can not gacha");
		}
		$gacha = $gachas[0];
		$user = $this->getSessionData("user");
		$this->log("can gacha");
		$gold = 0;
		$silver = 0;
		if(isset($args["buy_type"]) && $args["buy_type"] == "free"){
			$limit = $gacha["free_count"];
			$log = $this->get_free_log($user["id"], $gacha["id"]);
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
	
		$gacha_childs = $this->get_gacha_childs($gacha["id"]);
		$slot_list = array();
		$i = 0;
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
		//购买记录(消费)
		if(isset($args["buy_type"]) && $args["buy_type"] == "free"){
			$setlog = $this->set_gacha_free_log($user["id"], $gacha["id"]);
		}else{
			$setlog = $this->set_gacha_buy_log($user["id"], $gacha["id"], $gold, $silver);
		}
		if(!$setlog){
		    $this->user_db->trans_rollback();
			$this->error("set log error");
		}
		$this->log("set_gacha_log");
		//奖品
		$item_model = new Item_model();
		$equipment_model = new Equipment_model();
		for($i=0;!$error && $i<count($slot_list);$i++){
			$gacha_child = $slot_list[$i];
			$this->log("i=".$i.", ".$gacha_child["type"]);
			switch($gacha_child["type"]){
				case "item":
					$gacha_get = $item_model->set_item($user["id"], $gacha_child["child_id"]);
					break;
				case "horse":
				case "weapon":
				case "clothes":
					$gacha_get = $equipment_model->set_equipment($user["id"], $gacha_child["child_id"], $gacha_child["type"]);
					break;
			}
			if(!$gacha_get){
			    $this->user_db->trans_rollback();
				$this->error("get present error");
			}
		}
		$this->log("gacha get over");
		$this->user_db->trans_commit();
		$contents = array();
		foreach ($slot_list as $slot) {
			$contents[] = array("type"=>$slot["type"], "content_id"=>$slot["child_id"]);
		}
		return $contents;
	}
	function get_gacha_masters($id = null){
		$select = "id, gold, silver, from_time, to_time, free_time,free_count";
		$table = $this->master_db->gacha;
		$where = array();
		$now = date("Y-m-d H:i:s",time());
		$where[] = "(`from_time`='0000-00-00 00:00:00' OR `from_time`<'{$now}')";
		$where[] = "(`to_time`='0000-00-00 00:00:00' OR `to_time`>'{$now}')";
		if(!is_null($id)){
			$where[] = "id={$id}";
		}
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_gacha_childs($gacha_id){
		$select = "id, gacha_id, type, child_id, probability";
		$table = $this->master_db->gacha_child;
		$where = array();
		$where[] = "gacha_id={$gacha_id}";
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function set_gacha_buy_log($user_id, $gacha_id, $gold, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'gacha'";
		$values['child_id'] = $gacha_id;
		$values['gold'] = -$gold;
		$values['silver'] = -$silver;
		$values['register_time'] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
}