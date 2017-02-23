<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class User extends MY_Controller {
	function __construct() {
		parent::__construct();
		$this->load->model(array('user_model', 'character_model', 'item_model', 'version_model'));
	}
	public function register()
	{
		$id = $this->user_model->register($this->args);
		if($id){
			$this->out(array("id"=>$id));
		}else{
			$this->error("User is exists");
		}
	}
	public function login()
	{
		$user = $this->user_model->login($this->args);
		if($user){
			$versions = $this->version_model->get_master();
			$this->setSessionData("user", $user);
			$this->out(array("user"=>$user, "versions"=>$versions, "ssid"=>session_id()));
		}else{
			$this->error("Login failed");
		}
	}
	public function update()
	{
		$this->out(null);
	}
	public function get()
	{
		$user = $this->getSessionData("user");
		//将主页面地图信息加进user
		if(is_null($user["TopMap"])){
			$user["TopMap"] = $this->user_model->get_top_map($user["id"]);
		}
		$this->out($user);
	}
}
