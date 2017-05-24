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
	var $gacha_characters = "fsyy_master_lufylegend.gacha_characters";
	var $gacha_child = "fsyy_master_lufylegend.gacha_child";
	var $gacha_price = "fsyy_master_lufylegend.gacha_price";
	var $skill = "fsyy_master_lufylegend.skill";
	var $item = "fsyy_master_lufylegend.item";
	var $version = "fsyy_master_lufylegend.version";
	var $word = "fsyy_master_lufylegend.word";
	var $battlefield = "fsyy_master_lufylegend.battlefield";
	var $battlefield_tile = "fsyy_master_lufylegend.battlefield_tile";
	var $battlefield_npc = "fsyy_master_lufylegend.battlefield_npc";
	var $battlefield_own = "fsyy_master_lufylegend.battlefield_own";
	var $npc = "fsyy_master_lufylegend.npc";
	var $npc_equipment = "fsyy_master_lufylegend.npc_equipment";
	var $character_skill = "fsyy_master_lufylegend.character_skill";
	var $tutorial = "fsyy_master_lufylegend.tutorial";
	var $avatar_setting = "fsyy_master_lufylegend.avatar_setting";
	var $login_bonus = "fsyy_master_lufylegend.login_bonus";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('fsyy_master_lufylegend', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
