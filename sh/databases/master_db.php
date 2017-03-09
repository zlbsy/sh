<?php 
class Master_DB extends Base_Database {
	var $base_character = "base_character";
	var $constant = "constant";
	var $building = "building";
	var $tile = "tile";
	var $base_map = "base_map";
	var $world = "world";
	var $area = "area";
	var $horse = "horse";
	var $weapon = "weapon";
	var $clothes = "clothes";
	var $gacha = "gacha";
	var $gacha_child = "gacha_child";
	var $skill = "skill";
	var $item = "item";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_master_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
