<?php 
class Shop_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function buy_item($shop_id){
		$user = $this->getSessionData("user");
		$user_id = $user["id"]; 
		$master = $this->get_master($shop_id);
		if(is_null($master) || $master["money"] > 0){
			return false;
		}
		if($user["Gold"] < $master["gold"] || $user["Silver"] < $master["silver"]){
			return false;
		}
		$this->user_db->trans_begin();
		//购买记录(消费)
		$setlog = $this->set_buy_item_log($user["id"],$shop_id,$master["gold"],$master["silver"]);
		if(!$setlog){
			$this->user_db->trans_rollback();
			$this->error("set buy log error");
		}
		$up_result = false;
		if($master["gold"] > 0){
			$up_result = $user_model->update_gold();
		}else if($master["silver"] > 0){
			$up_result = $user_model->update_silver();
		}
		if(!$up_result){
			$this->user_db->trans_rollback();
			$this->error("money update error");
		}
		$user_model = new User_model();
		$content = json_decode($master["shop_content"], true);
		$contents_res = $user_model->set_contents($user_id,array($content));
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function get_master($shop_id){
		$now = date("Y-m-d H:i:s",time());
		$select = "`id`,`type`,`shop_content`,`gold`,`silver`,`money`";
		$table = $this->master_db->shop;
		$where = array("id={$shop_id}","limit_time>='{$now}'");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function buy_building($master_building, $x, $y){
		$user = $this->getSessionData("user");
		$price_type = $master_building["price_type"];
		$price = $master_building["price"];
		$gold = 0;
		$silver = 0;
		if($price_type == "gold"){
			$gold = $price;
			if($user["Gold"] < $gold){
				return null;
			}
		}else if($price_type == "silver"){
			$silver = $price;
			if($user["Silver"] < $silver){
				return null;
			}
		}
		$this->user_db->trans_begin();
		//购买建筑记录(消费)
		$setlog = $this->set_building_log($user["id"],$master_building["tile_id"],$gold,$silver);
		if(!$setlog){
			$this->user_db->trans_rollback();
			$this->error("set buy log error");
		}
		//获取建筑
		$setlog = $this->set_building($user["id"],$master_building["tile_id"],$x,$y);
		if(!$setlog){
			$this->user_db->trans_rollback();
			$this->error("set building error");
		}
		//存款更新
		$user_model = new User_model();
		if($price_type == "gold"){
			$up_result = $user_model->update_gold();
		}else if($price_type == "silver"){
			$up_result = $user_model->update_silver();
		}
		if(!$up_result){
			$this->user_db->trans_rollback();
			$this->error("no money");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function set_building_log($user_id, $child_id, $gold, $silver){
		return $this->set_buy_log($user_id, "'buy_building'",$child_id, $gold, $silver);
	}
	function set_buy_item_log($user_id, $child_id, $gold, $silver){
		return $this->set_buy_log($user_id, "'buy_item'",$child_id, $gold, $silver);
	}
	function set_buy_log($user_id, $type, $child_id, $gold, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = $type;
		$values['child_id'] = $child_id;
		$values['gold'] = -$gold;
		$values['silver'] = -$silver;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
	function set_building($user_id, $child_id, $x, $y){
		$values = array();
		$values['user_id'] = $user_id;
		$values['tile_id'] = $child_id;
		$values['level'] = 1;
		$values['x'] = $x;
		$values['y'] = $y;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->top_map);
		return $res;
	}

}
