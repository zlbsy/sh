<?php 
class Master_DB extends Base_Database {
	var $base_character = "base_character";
	var $constant = "constant";
	var $building = "building";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_master_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
	protected function error($message){
		die(json_encode(array("result"=>0,"message"=>$message)));
	}
	protected function out($arr){
		if(empty($arr)){
			$arr = array();
		}
		$arr["now"] = date("Y-m-d H:i:s");
		$arr["result"] = 1;
		//MY_Model::CloseDB();
		die(json_encode(array("result"=>1,"data"=>$arr)));
		//$data = array("out_list" => array("result"=>1,"data"=>$arr));
		//$this->load->view('out_view', $data);
	}
	protected function checkParam($list){
		foreach ($list as $value) {
			if(!isset($this->args[$value])){
				$this->error("Param error : " . $value);
			}
		}
	}
	protected function getSessionData($key){
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		$_SESSION[$key] = $value;
	}
	protected function checkAuth(){
		$user = $this->getSessionData("user");
		if(empty($user)){
			$this->error("sessionError");
		}
	}
}
