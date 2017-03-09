<?php 

class Equipment extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('equipment_model', 'user_model'));
	}
	public function equipment_list()
	{
		$user = $this->getSessionData("user");
		$equipment_model = new Equipment_model();
		$equipments = $equipment_model->get_list($user["id"]);
		if($equipments){
			$result = array("equipments"=>$equipments);
			$this->out($result);
		}else{
			$this->error("Equipment->equipment_list error");
		}
	}
}
