<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Equipment extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		$this->load->model(array('equipment_model', 'user_model'));
	}
	public function equipment_list()
	{
		$user = $this->getSessionData("user");
		$equipments = $this->equipment_model->get_list($user["id"]);
		if($equipments){
			$result = array("equipments"=>$equipments);
			$this->out($result);
		}else{
			$this->error("Equipment->equipment_list error");
		}
	}
	public function sale()
	{
		$user = $this->getSessionData("user");
		$result = $this->item_model->sale($user["id"], $this->args["e_id"]);
		if(!$result){
			$this->error("Equipment->sale error");
			return;
		}
		$user = $this->user_model->get($user["id"]);
		$this->setSessionData("user", $user);
		$equipments = $this->equipment_model->get_list($user["id"]);
		if($equipments){
			$result = array("equipments"=>$equipments, "user"=>$user);
			$this->out($result);
		}else{
			$this->error("Equipment->sale error");
		}
	}
	public function equip()
	{
		$this->checkParam(array("character_id", "e_id"));
		$user = $this->getSessionData("user");
		$equipment = $this->equipment_model->get_equipment($user["id"], $this->args["e_id"]);
		if(empty($equipment)){
			$this->error("Equipment->equip error:0");
			return;
		}
		$this->load->model(array('character_model'));
		$character_master = $this->character_model->get_master_character($this->args["character_id"]);
		if(empty($character_master)){
			$this->error("Equipment->equip error:0");
			return;
		}
		$equipment_master = $this->equipment_model->get_master($equipment["equipment_id"]);
		if(empty($equipment_master)){
			$this->error("Equipment->equip error:0");
			return;
		}
		
		$update_character_ids = array();
		$result = $this->equipment_model->equip($user["id"], $equipment, $equipment_master, $this->args["character_id"], $update_character_ids);
		if(!$result){
			$this->error("Equipment->equip error:1");
			return;
		}
		$characters = $this->character_model->get_character_list($user["id"],$update_character_ids);
		if($characters){
			$result = array("characters"=>$characters);
			$this->out($result);
		}else{
			$this->error("Equipment->equip error:2");
		}
	}
}
