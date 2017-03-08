<?php class MY_Controller extends CI_Controller {
	var $args;
	var $needAuth = false;
	function MY_Controller() {
		date_default_timezone_set('PRC');
		parent::__construct();
		//$this->load->library('viewloader');
		//$this->set_viewdata("page",strtolower($this->uri->segment(1)));
		($this->args = $this->input->get()) || ($this->args = $this->input->post());
		if($this->args["ssid"]){
			session_id($this->args["ssid"]);
		}else{
			session_id($this->session->userdata('session_id'));
		}
		session_start();
		if(!$this->args["ssid"]){
			$_SESSION = array();
		}
		if($this->needAuth){
			$this->checkAuth();
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
		die(json_encode(array("result"=>1,"data"=>$arr)));
		//die(json_encode($arr));
		//$this->load->view('out', array(("out_list" => $arr)));
		//$this->set_viewname("out");
	}
	protected function checkParam($list){
		foreach ($list as $value) {
			if(!isset($this->args[$value])){
				$this->error("Param error : " . $value);
			}
		}
	}
	protected function set_layoutname($_layoutname){
		ViewLoader::set_layoutname($_layoutname);
	}
	
	protected function set_viewname($_viewname){
		ViewLoader::set_viewname($_viewname);
	}
	
	protected function set_viewdata($_viewdata,$_viewdata_value = null){
		$this->load->vars($_viewdata,$_viewdata_value);
	}
	protected function getSessionData($key){
		//return $this->session->userdata($key);
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		//$this->session->set_userdata($key,$value);
		$_SESSION[$key] = $value;
	}
	protected function checkAuth(){
		$user = $this->getSessionData("user");
		if(empty($user)){
			$this->error("sessionError");
		}
	}
}
