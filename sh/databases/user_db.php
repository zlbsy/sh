<?php 
class User_DB extends Base_Database {
	var $player = "fsyy_lufylegend.player";
	var $top_map = "fsyy_lufylegend.top_map";
	var $item = "fsyy_lufylegend.item";
	var $characters = "fsyy_lufylegend.characters";
	var $bankbook = "fsyy_lufylegend.bankbook";
	var $world = "fsyy_lufylegend.world";
	var $area = "fsyy_lufylegend.area";
	var $stage = "fsyy_lufylegend.stage";
	var $equipment = "fsyy_lufylegend.equipment";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
