<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Tavern extends MY_Controller {
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		$this->load->model(array('tavern_model','user_model','item_model'));
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
