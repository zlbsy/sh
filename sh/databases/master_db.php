<?php 
class Master_DB extends Base_Database {
	var $base_character = "fsyy_master_lufylegend.base_character";
	var $constant = "fsyy_master_lufylegend.constant";
	var $building = "fsyy_master_lufylegend.building";
	var $tile = "fsyy_master_lufylegend.tile";
	var $base_map = "fsyy_master_lufylegend.base_map";
	var $world = "fsyy_master_lufylegend.world";
	var $area = "fsyy_master_lufylegend.area";
	var $horse = "fsyy_master_lufylegend.horse";
	var $weapon = "fsyy_master_lufylegend.weapon";
	var $clothes = "fsyy_master_lufylegend.clothes";
	var $gacha = "fsyy_master_lufylegend.gacha";
	var $gacha_child = "fsyy_master_lufylegend.gacha_child";
	var $skill = "fsyy_master_lufylegend.skill";
	var $item = "fsyy_master_lufylegend.item";
	var $version = "fsyy_master_lufylegend.version";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_master_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
