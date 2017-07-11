<?php 
class Master_DB extends Base_Database {
	var $base_character = "sh109_master.base_character";
	var $constant = "sh109_master.constant";
	var $building = "sh109_master.building";
	var $tile = "sh109_master.tile";
	var $base_map = "sh109_master.base_map";
	var $world = "sh109_master.world";
	var $area = "sh109_master.area";
	var $horse = "sh109_master.horse";
	var $weapon = "sh109_master.weapon";
	var $clothes = "sh109_master.clothes";
	var $gacha = "sh109_master.gacha";
	var $gacha_characters = "sh109_master.gacha_characters";
	var $gacha_child = "sh109_master.gacha_child";
	var $gacha_price = "sh109_master.gacha_price";
	var $skill = "sh109_master.skill";
	var $item = "sh109_master.item";
	var $version = "sh109_master.version";
	var $word = "sh109_master.word";
	var $battlefield = "sh109_master.battlefield";
	var $battlefield_tile = "sh109_master.battlefield_tile";
	var $battlefield_npc = "sh109_master.battlefield_npc";
	var $battlefield_own = "sh109_master.battlefield_own";
	var $npc = "sh109_master.npc";
	var $npc_equipment = "sh109_master.npc_equipment";
	var $character_skill = "sh109_master.character_skill";
	var $character_star = "sh109_master.character_star";
	var $tutorial = "sh109_master.tutorial";
	var $avatar_setting = "sh109_master.avatar_setting";
	var $login_bonus = "sh109_master.login_bonus";
	var $shop = "sh109_master.shop";
	var $battlefield_reward = "sh109_master.battlefield_reward";
	var $exp = "sh109_master.exp";
	var $strategy = "sh109_master.strategy";
	var $mission = "sh109_master.mission";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('sh109_master', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
