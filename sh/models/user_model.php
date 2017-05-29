<?php 
class User_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	var $_select_clums = 'id, name, nickname as Nickname, face as Face, level as Level, battling_id as BattlingId, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	var $_select_other_clums = 'id, name, nickname as Nickname, face as Face, level as Level, gold as Gold, silver as Silver, ap as Ap, map_id as MapId, last_ap_date as LastApDate';
	function register($account, $password, $name, $character_id){
		$character_model = new Character_model();
		$characters = $character_model->get_character_list(0, $character_id);
		if(count($characters) == 0){
			return false;
		}
		$character = $characters[0];
		$this->user_db->trans_begin();
		$values = array();
		$values['account'] = "'{$account}'";
		$values['pass'] = "'{$password}'";
		$values['name'] = "'{$name}'";
		$values['face'] = $character_id;
		$now = date("Y-m-d H:i:s");
		$values['last_ap_date'] = "'{$now}'";
		$values['register_time'] = "'{$now}'";
		$res_player = $this->user_db->insert($values, $this->user_db->player);
		if(!$res_player){
			$this->user_db->trans_rollback();
			$this->error("register fail player");
		}
		$user = $this->login(array("account"=>$account, "pass"=>$password));
		if(is_null($user)){
			$this->user_db->trans_rollback();
			$this->error("register fail user");
		}
		$user_id = $user["id"];
		$character_values = array();
		$character_values['user_id'] = $user_id;
		$character_values['character_id'] = $character_id;
		//$character_values['star'] = $character["Star"];
		//$character_values['weapon_type'] = "'".$character["WeaponType"]."'";
		//$character_values['move_type'] = "'".$character["MoveType"]."'";
		$character_values['horse'] = $character["Horse"];
		$character_values['clothes'] = $character["Clothes"];
		$character_values['weapon'] = $character["Weapon"];
		$character_values['register_time'] = "'{$now}'";
		$res_character = $character_model->character_insert($character_values);
		if(!$res_character){
			$this->user_db->trans_rollback();
			$this->error("register fail character");
		}
		$character_skills = array();
		$character_skills['user_id'] = $user_id;
		$character_skills['character_id'] = $character_id;
		$Skills = $character["Skills"][0];
		$character_skills['skill_id'] = $Skills["SkillId"];
		$character_skills['level'] = $Skills["Level"];
		$character_skills['register_time'] = "'{$now}'";
		$res_character = $character_model->character_skill_insert($character_skills);
		if(!$res_character){
			$this->user_db->trans_rollback();
			$this->error("register fail skill");
		}
		/*$equipment_model = new Equipment_model();
		if($character["Horse"] > 0){
			$rs_horse = $equipment_model->set_equipment($user_id, $character["Horse"], "horse", $character_id);
			if(!$rs_horse){
				$this->user_db->trans_rollback();
				$this->error("register fail horse");
			}
		}
		$rs_weapon = $equipment_model->set_equipment($user_id, $character["Weapon"], "weapon", $character_id);
		if(!$rs_weapon){
			$this->user_db->trans_rollback();
			$this->error("register fail weapon");
		}
		$rs_clothes = $equipment_model->set_equipment($user_id, $character["Clothes"], "clothes", $character_id);
		if(!$rs_clothes){
			$this->user_db->trans_rollback();
			$this->error("register fail clothes");
		}*/
		$this->user_db->trans_commit();
		return $user_id;
	}
	function login($args){
		$table = $this->user_db->player;
		$where = array();
		$where[] = "account='{$args["account"]}'";
		$where[] = "pass='{$args["pass"]}'";
		$result = $this->user_db->select($this->_select_clums, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_user_from_account($account){
		$table = $this->user_db->player;
		$where = array();
		$where[] = "account='{$account}'";
		$result = $this->user_db->select($this->_select_clums, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function update($args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$user = $this->getSessionData("user");
		$where = array("id={$user["id"]}");
		$table = $this->user_db->player;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
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
		$table = $this->user_db->bankbook;
		$where = array("user_id={$user["id"]}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return false;
		}
		return $this->update($result);
	}
	function get($id, $is_all = false, $is_self = false){
		$user = $this->getSessionData("user");
		if(!$is_self){
			$is_self = (isset($id) && $id == $user["id"]);
		}
		$select = $is_self ? $this->_select_clums : $this->_select_other_clums;
		$table = $this->user_db->player;
		$where = array("id={$id}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return null;
		}
		if($is_self && $is_all){
			$result["TopMap"] = $this->get_top_map($id);
			$result["progress"] = $this->get_story_progress($id);
		}
		return $result;
	}
	function get_top_map($user_id){
		$select = "id, user_id, num, tile_id, x, y, level";
		$table = $this->user_db->top_map;
		$where = array("user_id={$user_id}");
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function has_progress($key,$progress) {
	    foreach($progress as $child) {
	        if($child["k"] == $key){
	        	return $child["v"] == 1;
	        }
	    }
	    return false;
	}
	function get_story_progress($user_id, $k=null){
		$select = "k, v";
		$table = $this->user_db->story_progress;
		$where = array("user_id={$user_id}");
		if(!is_null($k)){
			$where[] = "k='".$k."'";
		}
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function set_story_progress($user_id,$k,$v){
		$values = array();
		$all_progress = $this->get_story_progress($user_id, $k);
		if(count($all_progress) == 0){
			$values["k"] = "'{$k}'";
			$values["v"] = $v;
			$values["user_id"] = $user_id;
			return  $this->user_db->insert($values, $this->user_db->story_progress);
		}
		//$values[] = "k=".$k;
		$values[] = "v=".$v;
		$table = $this->user_db->story_progress;
		$where = array("user_id={$user_id}","k='{$k}'");
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function set_contents($user_id, $contents){
		$item_update = false;
		$equipment_update = false;
		$character_update = false;
		$user_update = false;
		foreach($contents as $content){  
			$content_res = $this->set_content($user_id,$content,$item_update,$equipment_update,$character_update,$user_update);
			if(!$content_res){
				return false;
			}
		}
		$user = $this->getSessionData("user");
		if($item_update){
			$item_model = new Item_model();
			$items = $item_model->get_item_list($user["id"]);
			$user["items"] = $items;
		}
		if($equipment_update){
			$equipment_model = new Equipment_model();
			$equipments = $equipment_model->get_list($user["id"]);
			$user["equipments"] = $equipments;
		}
		if($character_update){
			$character_model = new Character_model();
			$characters = $character_model->get_character_list($user_id);
			$user["characters"] = $characters;
		}
		if($user_update){
			$new_user = $this->get($user_id,false,true);
			$user["Gold"] = $new_user["Gold"];
			$user["Silver"] = $new_user["Silver"];
			$user["Ap"] = $new_user["Ap"];
		}
		$this->setSessionData("user",$user);
		return true;
	}
	function set_content($user_id, $content, &$item_update, &$equipment_update, &$character_update,&$user_update){
		$res_get = false;
		switch($content["type"]){
			case "item":
				$item_model = new Item_model();
				$res_get = $item_model->set_item($user_id, $content["content_id"]);
				$item_update = true;
				break;
			case "horse":
			case "weapon":
			case "clothes":
				$equipment_model = new Equipment_model();
				$res_get = $equipment_model->set_equipment($user_id, $content["content_id"], $content["type"]);
				$equipment_update = true;
				break;
			case "character":
				$now = date("Y-m-d H:i:s");
				$character_values = array();
				$character_values['user_id'] = $user_id;
				$character_values['character_id'] = $content["content_id"];
				$character_values['register_time'] = "'{$now}'";
				$character_model = new Character_model();
				$res_get = $character_model->character_insert($character_values);
				$character_update = true;
				break;
			case "gold":
				$get_type = isset($content["get_type"]) ? $content["get_type"] : "";
				$content_id = isset($content["content_id"]) ? $content["content_id"] : 0;
				$res_get = $this->set_money_log($user_id,$get_type,$content_id,$content["value"],0);
				$this->update_money();
				$user_update = true;
				break;
			case "silver":
				$get_type = isset($content["get_type"]) ? $content["get_type"] : "";
				$content_id = isset($content["content_id"]) ? $content["content_id"] : 0;
				$res_get = $this->set_money_log($user_id,$get_type,$content_id,0,$content["value"]);
				$this->update_money();
				$user_update = true;
				break;
			case "ap":
				break;
		}
		return $res_get;
	}
	function set_money_log($user_id, $type,$child_id, $gold, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'{$type}'";
		$values['child_id'] = $child_id;
		$values['gold'] = $gold;
		$values['silver'] = $silver;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
}
