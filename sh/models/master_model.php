<?php 
class Master_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	public function get_master_character(){
		$select = "`id`,`name`,`nickname`,`head`,`hat`,`qualification`,`force`,`intelligence`,
		`command`,`agility`,`luck`,`power`,`moving_power`,`riding`,`walker`,`pike`,`sword`,
		`long_knife`,`knife`,`long_ax`,`ax`,`sticks`,`fist`,`archery`,`hidden_weapons`,`dual_wield`, `start_star`, `face_rect`";
		$table = $this->master_db->base_character;
		$order_by = "id";
		$result = $this->master_db->select($select, $table, null, $order_by, Database_Result::DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row['face_rect'] = explode(',', $row['face_rect']);
			$result_array[] = $row;
		}
		return $result_array;
	}
	public function get_master_constant(){
		$result = $this->master_db->select("`name`,`val`", $this->master_db->constant);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$result_array[$row["name"]] = $row["val"];
		}
		return $result_array;
	}
	public function get_master_building($level = null, $building_id = null){
		$select = "`id`,`from_level`,`to_level`,`tile_id`,`sum`,`price`,`price_type`";
		$table = $this->master_db->building;
		$where = null;
		if(!is_null($level)){
			$where = array();
			$where[] = "from_level <= {$level}";
			$where[] = "to_level >= {$level}";
			$where[] = "tile_id = {$building_id}";
		}
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, $where, $order_by, Database_Result::DEFAULT);
		if(!is_null($level)){
			$result = mysql_fetch_assoc($result_select);
		}else{
			$result = array();
			while ($row = mysql_fetch_assoc($result_select)) {
				$result[] = $row;
			}
		}
		return $result;
	}
	function get_master_tile(){
		$select = "`id`,`name`,`strategy`,`heal`,`boat`";
		$table = $this->master_db->tile;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_base_map(){
		$select = "`id`,`width`,`height`,`tile_ids`";
		$table = $this->master_db->base_map;
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, null, $order_by, Database_Result::DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["tile_ids"] = explode(",", $row["tile_ids"]);
			$result[] = $row;
		}
		return $result;
	}
	function get_master_world(){
		$select = "`id`,`tile_id`,`x`,`y`,`level`,`map_id`";
		$table = $this->master_db->world;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_area(){
		$select = "`id`,`world_id`,`tile_id`,`x`,`y`,`level`,`map_id`";
		$table = $this->master_db->area;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_horse(){
		$select = "`id`,`name`,`move_power`,`hp`";
		$table = $this->master_db->horse;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_weapon(){
		$select = "`id`,`name`,`physical_attack`,`power`";
		$table = $this->master_db->weapon;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_clothes(){
		$select = "`id`,`name`,`defense`,`hp`";
		$table = $this->master_db->clothes;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_skill(){
		$select = "`id`,`name`,`type`,`price`";
		$table = $this->master_db->skill;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_items(){
		$select = "`id`,`name`,`type`,`child_id`,`price`,`explanation`";
		$table = $this->master_db->item;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_gachas(){
		$select = "'id, name, gold, silver, from_time, to_time, free_time, free_count'";
		$table = $this->master_db->gacha;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		foreach ($result as $key => $child) {
			$child["childs"] = $this->get_master_gacha_childs($child["id"]);
			$result[$key] = $child;
		}
		return $result;
	}
	function get_master_gacha_childs($gacha_id){
		$select = "'id, gacha_id, type, child_id, probability'";
		$table = $this->master_db->gacha_child;
		$where = array("gacha_id = {$gacha_id}");
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
}
?>
