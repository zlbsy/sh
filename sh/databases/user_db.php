<?php 
class User_DB extends Base_Database {
	var $player = "player";
	var $top_map = "top_map";
	var $item = "item";
	var $characters = "characters";
	var $bankbook = "bankbook";
	var $world = "world";
	var $area = "area";
	var $stage = "stage";
	var $equipment = "equipment";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
