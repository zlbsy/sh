<?php 
if(!defined('BASEPATH')) exit('No direct script access allowed');
class Version_model extends MY_Model
{
	
	function __construct(){
		parent::__construct();
	}
	function get_master(){
		$this->master_db->select('name, ver');
		$query = $this->master_db->get(MASTER_VERSION);
		if ($query->num_rows() == 0){
			return null;
		}
		$vers = $query->result_array();
		$result = array();
		foreach($vers as $value){
			$result[$value["name"]] = $value["ver"];
		}
		return $result;
	}
}
