<?php 
class Gacha extends MY_Controller {
	function __construct() {
	//	$this->needAuth = true;
		parent::__construct();
		load_model(array('gacha_model','user_model','item_model','equipment_model'));
	}
	public function freelog()
	{
		$user_id = $this->args["user_id"];
		if(is_null($user_id)){
			$user = $this->getSessionData("user");
			$user_id = $user["id"];
		}
		$gacha_model = new Gacha_model();
		$logs = $gacha_model->get_free_logs($user_id);
		if($logs){
			$this->out(array("gachas"=>$logs));
		}else{
			$this->error("Gacha->freelog error");
		}
	}
	public function slot()
	{
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$gacha_id = $this->args["gacha_id"];
		$cnt = $this->args["cnt"];
		$contents = $this->gacha_model->slot($this->args);
		if($contents){
			$this->out(array("contents"=>$contents));
		}else{
			$this->error("Gacha->slot error");
		}
	}
}
