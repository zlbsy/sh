<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class ItemType{
	const CHARACTER_STONE = "character_stone";
	const CHARACTER_FRAGMENT = "character_fragment";
	const ARTIFACT_FRAGMENT = "artifact_fragment";
	const ARTIFACT = "artifact";
	const EXP = "exp";
}
class Item_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function use_item($user_id, $item_id, $number){
		$item_master = $this->get_master($item_id);
		if($item_master == null){
			return false;
		}
		$user = $this->getSessionData("user");
		if($item_master['type'] == ItemType::CHARACTER_STONE){
			$number = 1;
		}else if($item_master['type'] == ItemType::CHARACTER_FRAGMENT){
			$charas = $this->character_model->get_character_list($user['id'],$item_master['child_id']);
			$character_master = $this->character_model->get_master_character($item_master['child_id']);
			if(empty($charas)){
				$character_star = $this->character_model->get_cost_from_star($character_master["start_star"]);
			}else{
				$character = $charas[0];
				$character_star = $this->character_model->get_cost_from_star($character["star"] + 1);
				if($character_star == null){
					$character_star = array("cost"=>PHP_INT_MAX);
				}
			}
			$number = $character_star["cost"];
		}else if($item_master['type'] == ItemType::ARTIFACT){
		
		}else if($item_master['type'] == ItemType::ARTIFACT_FRAGMENT){
		
		}
		$item = $this->get_item($user_id, $item_id);
		if ($item == null || $item['cnt'] < $number){
			return null;
		}
		$this->user_db->trans_begin();
		$error = false;
		$result = null;
		//处理
		if(!$error){
			$result_remove = $this->remove_item($item, $number);
			$error = ($result_remove ? false : true);
		}
		if(!$error){
			if($item_master['type'] == ItemType::CHARACTER_STONE){
				$character_master = $this->character_model->get_master_character($item_master['child_id']);
				$result = $this->character_model->set_character($character_master);
				$error = ($result ? false : true);
				if($result){
					$charas = $this->character_model->get_character_list($user['id'],$character_master['id']);
					$error = (!empty($charas) ? false : true);
					$result = array("type"=>ItemType::CHARACTER_STONE, "character"=>(empty($charas) ? null : $charas[0]));
				}
			}else if($item_master['type'] == ItemType::CHARACTER_FRAGMENT){
				if(empty($charas)){
					$result_set = $this->character_model->set_character($character_master);
					$error = ($result_set ? false : true);
					$charas = $this->character_model->get_character_list($user['id'],$character_master['id']);
					$result = array("type"=>ItemType::CHARACTER_STONE, "character"=>(empty($charas) ? null : $charas[0]));
				}else{
					$character = $charas[0];
					$result_set = $this->character_model->update($character["id"], array("star"=>$character["star"] + 1));
					$error = ($result_set ? false : true);
					$character["star"] = $character["star"] + 1;
					$result = array("type"=>ItemType::CHARACTER_FRAGMENT, "character"=>$character);
				}

			}else if($item_master['type'] == ItemType::ARTIFACT){
			
			}else if($item_master['type'] == ItemType::ARTIFACT_FRAGMENT){
			
			}
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
		return $result;
	}
	function sale($user_id, $item_id, $number){
		$item = $this->get_item($user_id, $item_id);
		if ($item == null || $item['cnt'] < $number){
			return null;
		}
		$this->user_db->trans_begin();
		$error = false;
		//出售
		if(!$error){
			$result = $this->remove_item($item, $number);
			$error = ($result ? false : true);
		}
		if(!$error){
			$item_master = $this->get_master($item_id);
			$user = $this->getSessionData("user");
		}
		//存款变化
		if(!$error){
			$setlog = $this->set_sale_log($user["id"],$item_id,$item_master["price"] * $number);
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
	function remove_item($item, $number = 1){
		$new_cnt = $item['cnt'] - $number;
		$this->user_db->where('id', $item['id']);
		if($new_cnt == 0){
			$result = $this->user_db->delete(USER_ITEM); 
		}else{
			$this->user_db->set('cnt', $new_cnt);
			$result = $this->user_db->update(USER_ITEM);
		}
		return $result;
	}
	function set_sale_log($user_id, $item_id, $price){
		$this->user_db->set("silver",$price);
		$this->user_db->set("user_id",$user_id);
		$this->user_db->set("child_id",$item_id);
		return $this->user_db->insert(USER_BANKBOOK);
	}
	function get_item_list($user_id){
		$this->user_db->select('item_id, cnt');
		$this->user_db->where('user_id', $user_id);
	//	$this->user_db->group_by('item_id');
		$query = $this->user_db->get(USER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
	function get_item($user_id, $item_id){
		$this->user_db->select('id, item_id, cnt');
		$this->user_db->where('user_id', $user_id);
		$this->user_db->where('item_id', $item_id);
		$query = $this->user_db->get(USER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function set_item($user_id, $item_id, $cnt = 1){
		$item = $this->get_item($user_id, $item_id);
		$this->user_db->set('user_id', $user_id);
		$this->user_db->set('item_id', $item_id);
		if($item == null){
			$this->user_db->set('cnt', $cnt);
			$this->user_db->set('register_time', date("Y-m-d H:i:s",time()));
			$res = $this->user_db->insert(USER_ITEM);
		}else{
			$this->user_db->set('cnt',$item['cnt'] +  $cnt);
			$this->user_db->where('id', $item['id']);
			$res = $this->user_db->update(USER_ITEM);
		}
		return $res;
	}
	function get_master($item_id){
		$this->master_db->select('id, name, type, child_id, price, explanation');
		$this->master_db->where('id', $item_id);
		$query = $this->master_db->get(MASTER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->first_row('array');
		return $result;
	}
	function get_master_items(){
		$this->master_db->select('id, name, type, child_id, price, explanation');
		$query = $this->master_db->get(MASTER_ITEM);
		if ($query->num_rows() == 0){
			return null;
		}
		$result = $query->result_array();
		return $result;
	}
}
