<?php 
class Battle_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	
	function get_list($user_id, $battlefield_id = 0){
		$select = "id as Id,battlefield_id as BattlefieldId,level as Level,star as Star";
		$table = $this->user_db->battle_list;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($battlefield_id > 0){
			$where[] = "battlefield_id={$battlefield_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		if(is_null($result)){
			return array();
		}
		return $result;
	}
	function start($user_id, $battlefield_id){
		//TODO::check battlefield is open
		$user = $this->getSessionData("user");
		$user_model = new User_model();
		$res_start = $user_model->update(array("id"=>$user_id, "battling_id"=>$battlefield_id));
		return true;
	}
}
