<?php 

class Tool extends MY_Controller {
	function __construct() {
		//$this->needAuth = true;
		parent::__construct();
		load_model(array('tool_model'));
	}
	public function set_basemap()
	{
		$id = $this->args["id"];
		$width = $this->args["width"];
		$height = $this->args["height"];
		$tile_ids = $this->args["tile_ids"];
		$tool_model = new Tool_model();
		$res = $tool_model->set_basemap($id, $width, $height, $tile_ids);
		if($res){
			$this->out(array());
		}else{
			$this->error("Tools->set_tiles error");
		}
	}
	public function set_world()
	{
		/*$id = $this->args["id"];
		$width = $this->args["width"];
		$height = $this->args["height"];*/
		$worlds = json_decode($this->args["worlds"]);
		$tool_model = new Tool_model();
		$res = $tool_model->set_world($worlds);
		if($res){
			$this->out(array());
		}else{
			$this->error("Tools->set_tiles error");
		}
	}
}
