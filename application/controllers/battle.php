<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Battle extends MY_Controller {
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		$this->load->model(array('battle_model'));
	}
	public function win()
	{
		$chapters = $this->battle_model->win();
		if($chapters){
			$this->out(array("chapters"=>$chapters));
		}else{
			$this->error("Quest->chapter_list error");
		}
	}
	public function area_list()
	{
		$this->load->helper('search');
		$areas = $this->quest_model->area_list($this->args["chapter_id"]);
		if($areas){
			$this->out(array("areas"=>$areas));
		}else{
			$this->error("Quest->area_list error");
		}
	}
	public function stage_list()
	{
		$this->load->helper('search');
		$stages = $this->quest_model->stage_list($this->args["area_id"]);
		if($stages){
			$this->out(array("stages"=>$stages));
		}else{
			$this->error("Quest->stage_list error");
		}
	}
}