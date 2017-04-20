
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
		$this->checkParam("account", "password","name","select_id");
		$account = $this->args["account"];
		$user_model = new User_model();
		$has_account = $user_model->getUserFromAccount($account);
		if($has_account){
			$this->error("account is exist");
		}	
		$result = $user_model->register($account, $password, $name, $select_id);
		if($result){
				
		}else{
			$this->error("register fail");
		}
	}
}
