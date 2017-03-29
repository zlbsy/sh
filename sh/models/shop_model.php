<?php 
class Shop_model extends MY_Model
{
	function __construct(){
		parent::__construct();
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
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'buy_building'";
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
