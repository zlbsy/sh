<?php 
//die(dirname(__FILE__));
class Master extends MY_Controller {
	function __construct() {
		//$this->needAuth = true;
		parent::__construct();
		//$this->load->model(array('master_model'));
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
			$master_tile = $this->master_model->get_master_tile();
			$result["tiles"]=$master_tile;
		}
		if($args["base_map"]){
			$master_base_map	 = $this->master_model->get_master_base_map();
			$result["base_maps"]=$master_base_map;
		}
		if($args["world"]){
			$master_world	 = $this->master_model->get_master_world();
			$result["worlds"]=$master_world;
		}
		if($args["area"]){
			$master_area	 = $this->master_model->get_master_area();
			$result["areas"]=$master_area;
		}
		if($args["character"]){
			$master_character = $master_model->get_master_character();
			$result["characters"]=$master_character;
		}
		if($args["horse"]){
			$master_horse = $this->master_model->get_master_horse();
			$result["horses"]=$master_horse;
		}
		if($args["weapon"]){
			$master_weapon = $this->master_model->get_master_weapon();
			$result["weapons"]=$master_weapon;
		}
		if($args["clothes"]){
			$master_clothes = $this->master_model->get_master_clothes();
			$result["clothes"]=$master_clothes;
		}
		if($args["skill"]){
			$master_skill = $this->master_model->get_master_skill();
			$result["skills"]=$master_skill;
		}
		if($args["item"]){
			$master_item = $this->master_model->get_master_items();
			$result["items"]=$master_item;
		}
		if($args["gacha"]){
			$master_gacha = $this->master_model->get_master_gachas();
			$result["gachas"]=$master_gacha;
		}

		if($args["character_star"]){
			$master_character_star = $this->master_model->get_master_character_star();
			$result["character_star"]=$master_character_star;
		}
		if($args["growing"]){
			$master_growing = $this->master_model->get_master_growing();
			$result["growing"]=$master_growing;
		}
		
		return $result;
	}
	public function alldata()
	{
		load_model(array("master_model"));
		$master_data = $this->master_data($this->args);
		if(!is_null($master_data)){
			$this->out($master_data);
		}else{
			$this->error("Master->all error");
		}
	}
	public function version()
	{
		$this->load->model(array('version_model'));
		$result = array();
		$versions = $this->version_model->get_master();
		$result["versions"]=$versions;
		$this->out($result);
	}
	public function all()
	{
		$master_data = $this->master_version($this->args);
		if(!is_null($master_data)){
			$this->out($master_data);
		}else{
			$this->error("Master->all error");
		}
	}
}
