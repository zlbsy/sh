<?php 
if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class MY_Model extends CI_Model {
	static $_user_db = null;
	static $_master_db = null;
	var $user_db = null;
	var $master_db = null;
	var $_where = array();
	var $_where_in = array();
	var $_select = array();
	var $_join = array();
	var $_set = array();
	function __construct(){
		parent::__construct();
		if(MY_Model::$_user_db == null){
			MY_Model::$_user_db = $this->load->database('fsyy_lufylegend', TRUE);
		}
		if(MY_Model::$_master_db == null){
			MY_Model::$_master_db = $this->load->database('fsyy_master_lufylegend', TRUE);
		}
		$this->user_db = MY_Model::$_user_db;
		$this->master_db = MY_Model::$_master_db;
	}
	protected function getSessionData($key){
		//return $this->session->userdata($key);
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		//$this->session->set_userdata($key,$value);
		$_SESSION[$key] = $value;
	}
	protected function error($message){
		die(json_encode(array("result"=>0,"message"=>$message)));
	}
	public static function CloseDB(){
	/*	$query= MY_Model::$_master_db->query("SHOW PROCESSLIST");
		$processlist = $query->result_array();
		foreach ($processlist as $item) {
                	if ($item['Command'] == 'Sleep' && $item['Time'] > 120) {
                		MY_Model::$_master_db->query('KILL ' . $item['Id']);
            		}
		}
	*/	MY_Model::$_user_db->close();
		MY_Model::$_master_db->close();
	}
}
