<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Gacha extends MY_Controller {
	function __construct() {
	//	$this->needAuth = true;
		parent::__construct();
		$this->load->model(array('gacha_model','user_model','item_model'));
	}
	public function freelog()
	{
		$user_id = $this->args["user_id"];
		if(is_null($user_id)){
			$user = $this->getSessionData("user");
			$user_id = $user["id"];
		}

		$logs = $this->gacha_model->get_free_logs($user_id);
		if($logs){
			$this->out(array("gachas"=>$logs));
		}else{
			$this->error("Gacha->freelog error");
		}
	}

	public function liqueur_list()
	{
		$liqueurs = $this->tavern_model->liqueur_list($this->args);
		if($liqueurs){
			$this->out(array("liqueurs"=>$liqueurs));
		}else{
			$this->error("Tavern->liqueur_list error");
		}
	}
	public function liqueur_buy()
	{
		$items = $this->tavern_model->liqueur_buy($this->args);
		if($items){
			//player session 更新
			$user_old = $this->getSessionData("user");
			$user = $this->user_model->get($user_old["id"]);
			$this->setSessionData("user", $user);
			$this->out(array("items"=>$items, "user"=>$user));
		}else{
			$this->error("Tavern->liqueur_buy error");
		}
	}
}
