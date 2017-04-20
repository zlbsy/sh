<?php 
class User extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'item_model', 'version_model'));
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
		$this->out(array("user"=>$user));
	}
}
