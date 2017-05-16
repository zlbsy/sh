
<?php 
class Register extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('character_model','user_model'));
	}
	public function character_list()
	{
		$character_model = new Character_model();
		$characters = $character_model->get_character_list(0);
		if($characters !== null){
			$this->out(array("characters"=>$characters));
		}else{
			$this->error("character is not exists");
		}
	}
	public function insert()
	{
		load_model(array('equipment_model'));
		$this->checkParam(array("account", "password","name","character_id"));
		$account = $this->args["account"];
		$user_model = new User_model();
		$has_account = $user_model->get_user_from_account($account);
		if($has_account){
			$this->error("account is exist");
		}	
		$password = $this->args["password"];
		$name = $this->args["name"];
		$character_id = $this->args["character_id"];
		$id = $user_model->register($account, $password, $name, $character_id);
		if($id){
				$user = $user_model->get($id, true, true);
				$this->setSessionData("user", $user);
				$this->out(array("user"=>$user));
		}else{
			$this->error("register fail");
		}
	}
}
