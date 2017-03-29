<?php 
//die(dirname(__FILE__));
class Master extends MY_Controller {
	function __construct() {
		//$this->needAuth = true;
		parent::__construct();
		load_model(array("master_model"));
	}
	private function master_data($args)
	{
		$result = array();
		$master_model = new Master_model();
		if($args["constant"]){
			$master_constant = $master_model->get_master_constant();
			$result["constant"]=$master_constant;
		}
		if($args["building"]){
			$master_building = $master_model->get_master_building();
			$result["buildings"]=$master_building;
		}
		if($args["tile"]){
			$master_tile = $master_model->get_master_tile();
			$result["tiles"]=$master_tile;
		}
		if($args["base_map"]){
			$master_base_map	 = $master_model->get_master_base_map();
			$result["base_maps"]=$master_base_map;
		}
		if($args["world"]){
			$master_world	 = $master_model->get_master_world();
			$result["worlds"]=$master_world;
		}
		if($args["area"]){
			$master_area	 = $master_model->get_master_area();
			$result["areas"]=$master_area;
		}
		if($args["character"]){
			$master_character = $master_model->get_master_character();
			$result["characters"]=$master_character;
		}
		if($args["horse"]){
			$master_horse = $master_model->get_master_horse();
			$result["horses"]=$master_horse;
		}
		if($args["weapon"]){
			$master_weapon = $master_model->get_master_weapon();
			$result["weapons"]=$master_weapon;
		}
		if($args["clothes"]){
			$master_clothes = $master_model->get_master_clothes();
			$result["clothes"]=$master_clothes;
		}
		if($args["skill"]){
			$master_skill = $master_model->get_master_skill();
			$result["skills"]=$master_skill;
		}
		if($args["item"]){
			$master_item = $master_model->get_master_items();
			$result["items"]=$master_item;
		}
		if($args["gacha"]){
			$master_gacha = $master_model->get_master_gachas();
			$result["gachas"]=$master_gacha;
		}
		if($args["word"]){
			$master_word = $master_model->get_master_words();
			$result["words"]=$master_word;
		}
		if($args["npc"]){
			$master_npc = $master_model->get_master_npcs();
			$result["npcs"]=$master_npc;
		}
		if($args["npc_equipment"]){
			$master_npc_equipment = $master_model->get_master_npc_equipments();
			$result["npc_equipments"]=$master_npc_equipment;
		}
		if($args["battlefield"]){
			$master_battlefield = $master_model->get_master_battlefields();
			$result["battlefields"]=$master_battlefield;
		}
		
		return $result;
	}
	public function alldata()
	{
		$master_data = $this->master_data($this->args);
		if(!is_null($master_data)){
			$this->out($master_data);
		}else{
			$this->error("Master->all error");
		}
	}
	public function version()
	{
		load_model(array("version_model"));
		$result = array();
		$version_model = new Version_model();
		$versions = $version_model->get_master();
		$result["versions"]=$versions;
		$this->out($result);
	}
}
