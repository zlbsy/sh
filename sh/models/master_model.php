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
		$result = $this->master_db->select($select, $table, null, $order_by);
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
		$result_select = $this->master_db->select($select, $table, $where, $order_by);
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
}
?>
