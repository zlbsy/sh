<?php 
class Tool_model extends MY_Model
{
	var $base_map = "sh109_master.base_map";
	var $world = "sh109_master.world";
	var $area = "sh109_master.area";
	function __construct(){
		parent::__construct();
	}
	function set_stage($world_id, $stages){
		$this->master_db->trans_begin();
		$where = array("world_id='{$world_id}'");
		$res = $this->master_db->delete($this->area, $where);
		if(!$res){
			$this->master_db->trans_rollback();
			return false;
		}
		foreach ($stages as $world) {
			$values = array();
			$values["x"] = $world["x"];
			$values["y"] = $world["y"];
			$values["tile_id"] = $world["tile_id"];
			$values["world_id"] = $world_id;
			$res = $this->master_db->insert($values, $this->area);
			if(!$res){
				$this->master_db->trans_rollback();
				return false;
			}
		}
		$this->master_db->trans_commit();
		return true;
	}
	function set_world($worlds){
		$this->master_db->trans_begin();
		$sql = "TRUNCATE ".$this->world;
		$res = $this->master_db->selectSQL($sql);
		if(!$res){
			$this->master_db->trans_rollback();
			return false;
		}
		foreach ($worlds as $world) {
			$values = array();
			$values["id"] = $world["id"];
			$values["x"] = $world["x"];
			$values["y"] = $world["y"];
			$values["tile_id"] = $world["tile_id"];
			$values["map_id"] = $world["map_id"];
			$values["build_name_cn"] = "'".$world["build_name"]."'";
			$res = $this->master_db->insert($values, $this->world);
			if(!$res){
				$this->master_db->trans_rollback();
				return false;
			}
		}
		$this->master_db->trans_commit();
		return true;
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
