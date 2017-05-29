<?php 
class User extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'item_model', 'version_model', 'loginbonus_model'));
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
		$user_model = new User_model();
		$this->args["pass"] = md5($this->args["pass"]);
		$user = $user_model->login($this->args);
		if($user){
			$version_model = new Version_model();
			$versions = $version_model->get_master();
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
	public function progress()
	{
		$user_model = new User_model();
		$result = $user_model->set_story_progress($this->args["user_id"], $this->args["k"], $this->args["v"]);
		$this->out(array());
	}
	public function get()
	{
		$user = $this->getSessionData("user");
		$user_model = null;
		if(is_null($user["TopMap"])){
			$user_model = is_null($user_model) ? new User_model() : $user_model;
			$user["TopMap"] = $user_model->get_top_map($user["id"]);
		}
		if(is_null($user["progress"])){
			$user_model = is_null($user_model) ? new User_model() : $user_model;
			$user["progress"] = $user_model->get_story_progress($user["id"]);
		}
		if(!isset($user["loginbonus_received"])){
			$loginbonus_model = new Loginbonus_model();
			$user["loginbonus_cnt"] = $loginbonus_model->get_log_count($user["id"]);
			$user["loginbonus_received"] = $loginbonus_model->received_loginbonus($user["id"]);
		}
		$this->setSessionData("user", $user);
		$this->out(array("user"=>$user));
	}
}
