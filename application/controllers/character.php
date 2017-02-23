
<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
class Character extends MY_Controller {
	function __construct() {
		parent::__construct();
		$this->load->model(array('user_model', 'character_model', 'item_model', 'version_model', 'equipment_model'));
	}
	public function get()
	{exit("get");
		/*$user = $this->getSessionData("user");
		$characters = $this->character_model->get_character_list($user["id"],$this->args["chara_id"]);
		if($characters !== null){
			$this->out(array("characters"=>$characters));
		}else{
			$this->error("character is not exists");
		}*/
	}
	public function character_list()
	{
		//exit("list");
		//$characters = array();
		//$this->out(array("characters"=>$characters));
		//return;
		$user_id = $this->args["user_id"];
		if(is_null($user_id)){
			$user = $this->getSessionData("user");
			$user_id = $user["id"];
		}
		$characters = $this->character_model->get_character_list($user_id);
		if($characters !== null){
			$this->out(array("characters"=>$characters));
		}else{
			$this->error("character is not exists");
		}
	}
}
