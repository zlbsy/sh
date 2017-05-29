<?php 
class Battle_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	var $exp_master = null;
	function get_list($user_id, $battlefield_id = 0){
		$select = "id as Id,battlefield_id as BattlefieldId,level as Level,star as Star";
		$table = $this->user_db->battle_list;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($battlefield_id > 0){
			$where[] = "battlefield_id={$battlefield_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		if(is_null($result)){
			return array();
		}
		return $result;
	}
	function set_battlefield($user_id, $battlefield_id, $boss_id, $level, $star, $isFirstBattle){
		$values = array();
		$table = $this->user_db->battle_list;
		if($isFirstBattle){
			$values['user_id'] = $user_id;
			$values['battlefield_id'] = $battlefield_id;
			$values['boss_id'] = $boss_id;
			$values['level'] = $level;
			$values['star'] = $star;
			$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
			$res = $this->user_db->insert($values, $table);
		}else{
			$values[] = "`star`={$star}";
			$where = array("user_id={$user_id}", "battlefield_id={$battlefield_id}");
			$res = $this->user_db->update($values, $table, $where);
		}
		return $res;

		$select = "id as Id,battlefield_id as BattlefieldId,level as Level,star as Star";
		$table = $this->user_db->battle_list;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($battlefield_id > 0){
			$where[] = "battlefield_id={$battlefield_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		if(is_null($result)){
			return array();
		}
		return $result;
	}
	function battle_start($battlefield_id){
		$battlefield_master = $this->get_master($battlefield_id);
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		if($user["Ap"] < $battlefield_master["ap"]){
			return false;
		}
		$user_model = new User_model();
		$conditions = json_decode($battlefield_master["conditions"], true);
		if(count($conditions) > 0){
			$progress = $user_model->get_story_progress($user_id);
			foreach ($$conditions as $value) {
				$has = $user_model->has_progress($value, $progress);
				if(!$has){
					return false;
				}
			}
		}
		$this->user_db->trans_begin();
		$ap = $user["Ap"] - $battlefield_master["ap"];
		$res_start = $user_model->update(array("id"=>$user_id, "battling_id"=>$battlefield_id, "ap"=>$ap, "last_ap_date"=>"'".date("Y-m-d H:i:s")."'"));
		if(!$res_start){
			$this->user_db->trans_rollback();
			$this->error("user update error");
		}
		$user["BattlingId"] = 1;
		$user["Ap"] = $ap;
		$this->setSessionData("user", $user);
		$this->user_db->trans_commit();
		return true;
	}
	function battle_end($battlefield_id, $characterIds, $star){
		$battlefield_master = $this->get_master($battlefield_id);
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$user_model = new User_model();
		$conditions = json_decode($battlefield_master["conditions"], true);
		if(count($conditions) > 0){
			$progress = $user_model->get_story_progress($user_id);
			foreach ($$conditions as $value) {
				$has = $user_model->has_progress($value, $progress);
				if(!$has){
					return null;
				}
			}
		}
		$character_model = new Character_model();
		$characters = $character_model->get_character_list($user_id);
		foreach ($characterIds as $characterId) {
			$key = array_search($characterId, array_column($characters, 'CharacterId'));
			if(is_null($key)){
				return null;
			}
		}
		$battleList = $this->get_list($user_id, $battlefield_id);
		$isFirstBattle = (count($battleList) == 0);
		if(!$isFirstBattle){
			$old_star = $battleList[0]["Star"];
			$star = $star > $old_star ? $star : $old_star;
		}
		$battlefield_rewards = $this->get_rewards_master($battlefield_id);
		$contents = array();
		foreach ($battlefield_rewards as $battlefield_reward) {
			$content = $battlefield_reward["content"];
			if($battlefield_reward["is_first"] == 1){
				if($isFirstBattle){
					$contents[] = $content;
				}
				continue;
			}
			$probability = $battlefield_reward["probability"];
			if(rand(0,100) <= $probability){
				$contents[] = $content;
			}
		}
		$npcs = $this->get_master_battlefield_npcs($battlefield_id);
		$boss_id = $npcs[0]["npc_id"];
		$sum_level = 0;
		foreach ($npcs as $npc) {
			$sum_level += $npc["level"];
			if($npc["boss"]){
				$boss_id = $npc["npc_id"];
			}
		}
		$level = floor($sum_level / count($npcs));
		$user_characters = $this->get_user_characters_constant();
		$this->user_db->trans_begin();
		$contents_res = $user_model->set_contents($user_id,$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$res_set = $this->set_battlefield($user_id, $battlefield_id, $boss_id, $level, $star, $isFirstBattle);
		if(!$res_set){
			$this->user_db->trans_rollback();
			$this->error("set_battlefield error");
		}
		$user_level = 0;
		foreach ($characterIds as $characterId) {
			$key = array_search($characterId, array_column($characters, 'CharacterId'));
			$charater = $characters[$key];
			$exp = $this->get_character_exp($charater["Level"], $level, $battlefield_master["exp"]);
			$character_level = $this->get_character_level($charater["Exp"] + $exp);
			$character_args = array("exp"=>$exp, "level"=>$character_level);
			$res_update_character = $character_model->update_character($user_id, $characterId, $character_args);
			if(!$res_update_character){
				$this->user_db->trans_rollback();
				$this->error("character update error");
			}
			if($character["Id"] >= $user_characters[0] && $character["Id"] <= $user_characters[1]){
				$user_level = $character_level;
			}
		}
		$res_update = $user_model->update(array("id"=>$user_id, "battling_id"=>0, "level"=>$user_level));
		if(!$res_update){
			$this->user_db->trans_rollback();
			$this->error("user update error");
		}
		$user["BattlingId"] = 0;
		$user["Level"] = $user_level;
		$this->setSessionData("user", $user);
		$this->master_db->trans_commit();
		return $contents;
	}
	function get_character_exp($character_level, $battlefield_level, $battlefield_exp){
		$exp = $battlefield_exp;
		$num = 0;
		if($character_level > $battlefield_level){
			$num = $character_level - $battlefield_level;
		}else if($character_level < $battlefield_level - 4){
			$num = $battlefield_level - 4 - $character_level;
		}
		if($num > 4){
			$num = 4;
		}
		$exp = floor($battlefield_exp * (1 - $num * 0.2));
		return $exp;
	}
	function get_master($battlefield_id){
		$select = "`id`,`map_id`,`conditions`, `max_num`, `ap`, `exp`";
		$table = $this->master_db->battlefield;
		$where = array();
		$where[] = "id={$battlefield_id}";
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_character_level($exp){
		if($this->exp_master == null){
			$select = "`exp_type`,`level`,`value`";
			$table = $this->master_db->exp;
			$order_by = "level desc";
			$this->exp_master = $this->master_db->select($select, $table, null, $order_by);
		}
		foreach ($$this->exp_master as $child) {
			if($child["value"] >= $exp){
				return $child["level"];
			}
		}
		return 1;
	}
	public function get_user_characters_constant(){
		$result = $this->master_db->select("`val`", $this->master_db->constant, array("`name`='user_characters'"), null, null, Database_Result::TYPE_ROW);
		return json_decode($result["val"], true);
	}
	function get_master_battlefield_npcs($battlefield_id){
		$select = "`id`, `npc_id`,`boss`,`skills`, `x`,`y`, `level`,`star`,`weapon`,`horse`,`clothes`";
		$table = $this->master_db->battlefield_npc;
		$where = array("battlefield_id = {$battlefield_id}","belong='enemy'");
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_rewards_master($battlefield_id){
		$select = "`id`,`map_id`,`conditions`, `max_num`, `ap`";
		$table = $this->master_db->battlefield_reward;
		$where = array();
		$where[] = "id={$battlefield_id}";
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
}
