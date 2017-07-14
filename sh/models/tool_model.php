<?php 
class Tool_model extends MY_Model
{
	var $base_map = "sh109_master.base_map";
	function __construct(){
		parent::__construct();
	}
	function set_basemap($id, $width, $height, $tile_ids){
		$basemap = $this->get_basemap($id);
		if($basemap == null){
			$values = array();
			$values["id"] = $id;
			$values["width"] = $width;
			$values["height"] = $height;
			$values["tile_ids"] = "'".$tile_ids."'";
			return $this->master_db->insert($values, $this->base_map);
		}else{
			$values = array();
			$values[] = "width={$width}";
			$values[] = "height={$height}";
			$values[] = "tile_ids='{$tile_ids}'";
			$where = array();
			$where[] = "id={$id}";
			return $this->master_db->update($values, $this->base_map, $where);
		}
	}
	function get_basemap($id){
		$select = "id,width,height,tile_ids";
		$where = array();
		$where[] = "id={$id}";
		$result = $this->master_db->select($select, $this->base_map, $where);
		return $result;
	}
}
