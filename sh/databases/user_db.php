<?php 
class User_DB extends Base_Database {
	var $player = "fsyy_lufylegend.player";
	var $top_map = "fsyy_lufylegend.top_map";
	var $item = "fsyy_lufylegend.item";
	var $characters = "fsyy_lufylegend.characters";
	var $character_skill = "fsyy_lufylegend.character_skill";
	var $bankbook = "fsyy_lufylegend.bankbook";
	var $world = "fsyy_lufylegend.world";
	var $area = "fsyy_lufylegend.area";
	var $stage = "fsyy_lufylegend.stage";
	var $equipment = "fsyy_lufylegend.equipment";
	var $gacha_free_log = "fsyy_lufylegend.gacha_free_log";
	var $battle_list = "fsyy_lufylegend.battle_list";
	var $story_progress = "fsyy_lufylegend.story_progress";
	var $login_bonus = "fsyy_lufylegend.login_bonus";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
