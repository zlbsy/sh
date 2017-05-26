<?php 
class User_DB extends Base_Database {
	var $player = "sh109_user.player";
	var $top_map = "sh109_user.top_map";
	var $item = "sh109_user.item";
	var $characters = "sh109_user.characters";
	var $character_skill = "sh109_user.character_skill";
	var $bankbook = "sh109_user.bankbook";
	var $world = "sh109_user.world";
	var $area = "sh109_user.area";
	var $stage = "sh109_user.stage";
	var $equipment = "sh109_user.equipment";
	var $gacha_free_log = "sh109_user.gacha_free_log";
	var $battle_list = "sh109_user.battle_list";
	var $story_progress = "sh109_user.story_progress";
	var $login_bonus = "sh109_user.login_bonus";
	var $present= "sh109_user.present";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('sh109_user', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
