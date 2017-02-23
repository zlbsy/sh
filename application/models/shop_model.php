<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
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
		$error = false;
		//购买建筑记录(消费)
		if(!$error){
			$setlog = $this->set_building_log($user["id"],$master_building["tile_id"],$gold,$silver);
			$error = ($setlog ? false : true);
		}
		//获取建筑
		if(!$error){
			$setlog = $this->set_building($user["id"],$master_building["tile_id"],$x,$y);
			$error = ($setlog ? false : true);
		}
		//存款更新
		if(!$error){
			$up_result = $this->user_model->update_silver();
			$error = ($up_result ? false : true);
		}
		if ($error || $this->user_db->trans_status() === FALSE)
		{
		    $this->user_db->trans_rollback();
			return false;
		}
		else
		{
		    $this->user_db->trans_commit();
		}
		return true;
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
	function set_building($user_id, $child_id, $x, $y){
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('tile_id', $child_id);
		$this->user_db->set('level', 1);
		$this->user_db->set('x', $x);
		$this->user_db->set('y', $y);
		$this->user_db->set('register_time', date("Y-m-d H:i:s",time()));
		$res = $this->user_db->insert(USER_TOP_MAP);
		return $res;
	}
	function set_building_log($user_id, $child_id, $gold, $silver){
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('type', 'buy_building');
		$this->user_db->set('child_id', $child_id);
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
