<?php 
class Master_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	public function get_master_character(){
		$sql = "SELECT 
		`id`,`name`,`nickname`,`head`,`hat`,`qualification`,`force`,`intelligence`,
		`command`,`agility`,`luck`,`power`,`moving_power`,`riding`,`walker`,`pike`,`sword`,
		`long_knife`,`knife`,`long_ax`,`ax`,`sticks`,`fist`,`archery`,`hidden_weapons`,`dual_wield`, `start_star`, `face_rect`
		FROM base_character 
		ORDER BY id";
		$result = $this->master_db->select($sql);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row['face_rect'] = explode(',', $row['face_rect']);
			$result_array[] = $row;
		}
		return $result_array;
	}
}
?>