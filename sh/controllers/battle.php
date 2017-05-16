<?php 

class Battle extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('battle_model', 'user_model'));
	}
	public function battle_list()
	{
		$user = $this->getSessionData("user");
		$battle_model = new Battle_model();
		$battle_list = $battle_model->get_list($user["id"]);
		if(!is_null($battle_list)){
			$result = array("battlelist"=>$battle_list);
			$this->out($result);
		}else{
			$this->error("Battle->battle_list error");
		}
	}
}
