<?php 
class MY_Model {
	static $_user_db = null;
	static $_master_db = null;
	var $user_db = null;
	var $master_db = null;
	function __construct(){
		if(MY_Model::$_user_db == null){
			MY_Model::$_user_db = new User_DB();
		}
		if(MY_Model::$_master_db == null){
			MY_Model::$_master_db = new Master_DB();
		}
		$this->user_db = MY_Model::$_user_db;
		$this->master_db = MY_Model::$_master_db;
	}
	protected function getSessionData($key){
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		$_SESSION[$key] = $value;
	}
	protected function error($message){
		die(json_encode(array("result"=>0,"message"=>$message)));
	}
	public static function CloseDB(){
		MY_Model::$_user_db->close();
		MY_Model::$_master_db->close();
	}
	protected function log($message){
		$fp = fopen('/var/www/d/test/test.txt', 'w');
		fwrite($fp, $message);
		fclose($fp);
	}
}
