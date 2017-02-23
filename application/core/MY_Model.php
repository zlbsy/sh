<?php 
if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class MY_Model extends CI_Model {
	static $_user_db = null;
	static $_master_db = null;
	var $user_db = null;
	var $master_db = null;
	var $_where = array();
	var $_where_in = array();
	var $_select = array();
	var $_join = array();
	var $_set = array();
	function __construct(){
		parent::__construct();
		if(MY_Model::$_user_db == null){
			MY_Model::$_user_db = $this->load->database('fsyy_lufylegend', TRUE);
		}
		if(MY_Model::$_master_db == null){
			MY_Model::$_master_db = $this->load->database('fsyy_master_lufylegend', TRUE);
		}
		$this->user_db = MY_Model::$_user_db;
		$this->master_db = MY_Model::$_master_db;
	}
	protected function getSessionData($key){
		//return $this->session->userdata($key);
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		//$this->session->set_userdata($key,$value);
		$_SESSION[$key] = $value;
	}
	protected function transBool($result){
		if(!$result){
			$this->user_db->trans_rollback();
			return false;
		}
		return true;
	}
	protected function transEmpty($result){
		if(empty($result)){
			$this->user_db->trans_rollback();
			return false;
		}
		return true;
	}
	protected function transStatus(){
		if ($this->user_db->trans_status() === FALSE)
		{
		    $this->user_db->trans_rollback();
			return false;
		}
		else
		{
		    $this->user_db->trans_commit();
			return true;
		}
	}
	/*
	function set($fields){
		if($fields && is_array($fields)){
			foreach ($fields as $key=>$value)$this->_set[$key] = $value;
		}
		return $this;
	}
	function select($fields){
		if($fields && is_array($fields)){
			foreach ($fields as $value)$this->_select[] = $value;
		}
		return $this;
	}
	function join($type,$table,$on){
		$this->_join[] = array("type"=>$type,"table"=>$table,"on"=>$on);
		return $this;
	}
	function where($fields){
		if($fields && is_array($fields)){
			foreach ($fields as $key=>$value)$this->_where[$key] = $value;
		}
		return $this;
	}
	function where_in($fields){
		if($fields && is_array($fields)){
			foreach ($fields as $key=>$value)$this->_where_in[$key] = $value;
		}
		return $this;
	}
	function get_count($table_name,$fields=null){
		$this->load->database();
		if($fields && is_array($fields)){
			foreach ($fields as $key=>$value)$this->db->where($key,$value);
		}
		return $this->db->count_all($table_name);
	}
	function get($table_name,$fields=null,$limit=0,$offset=0){
		$this->load->database();
		if(count($this->_select)){
			$this->db->select(implode(",",$this->_select),FALSE);
		}
		if(count($this->_where)){
			foreach ($this->_where as $key=>$value)$this->db->where($key,$value);
		}
		if(count($this->_where_in)){
			foreach ($this->_where_in as $key=>$value)$this->db->where_in($key,$value);
		}
		if($fields && is_array($fields)){
			foreach ($fields as $key=>$value)$this->db->where($key,$value);
		}
		
		if(count($this->_join)){
			foreach ($this->_join as $value){
				$this->db->join($value["table"], $value["on"], $value["type"]);
			}
		}
		if($limit)$this->db->limit($limit, $offset);
		$result =  $this->db->get($table_name);
		$this->_where = array();
		$this->_where_in = array();
		$this->_select = array();
		$this->_join = array();
		return $result->result_array();
	}
	function create_or_update($args=array(),$string_fields=array(),$table_name,$id=null)
	{
		$this->load->database();
		$fields = $this->db->list_fields($table_name);
		$this->_field($args, $string_fields, $fields, $id);
		if($id){
			$this->db->where($id,$args[$id]);
			$result = $this->db->update($table_name);
		}else{
			$result = $this->db->insert($table_name);
		}
		$this->_where = array();
		$this->_where_in = array();
		$this->_select = array();
		$this->_join = array();
		return $result;
	}
	function update($table_name){
		$this->load->database();
		if(count($this->_where)){
			foreach ($this->_where as $key=>$value)$this->db->where($key,$value);
		}
		if(count($this->_where_in)){
			foreach ($this->_where_in as $key=>$value)$this->db->where_in($key,$value);
		}
		if(count($this->_set)){
			foreach ($this->_set as $key=>$value)$this->db->set($key,$value);
		}
		$result = $this->db->update($table_name);
		$this->_where = array();
		$this->_set = array();
		return $result;
	}
	
	function _field($args,$string_fields,$fields,$id){
		
		foreach ($fields as $field)
		{
			if($field != $id && array_key_exists($field, $args)){
				if(in_array($field,$string_fields)){
					$this->db->set($field,$args[$field],FALSE);
				}else{
					$this->db->set($field,$args[$field]);
				}
				
			}
		}
	}
	*/
	
	
	//[brush][html][/brush]
	/**
	 * vv
	 * >aa</pre>ddd
	 * >aass</pre>
	 */
	 /*
	function getSql($value){
		$value = str_replace("'", "''", $value);
		
		$value_brush = explode("[brush]", $value);
		$value = $this->changeToHtml($value_brush[0]);
		for($i=1;$i<count($value_brush);$i++){
			$value_brush_end = explode("[/brush]", $value_brush[$i]);
			$value .= "[brush]".$this->changeToPre($value_brush_end[0])."[/brush]".$this->changeToHtml($value_brush_end[1]);
		}
		$value = str_replace('[brush][html]', '<pre  class="brush:html">', $value);
		$value = str_replace('[brush][js]', '<pre  class="brush:js">', $value);
		$value = str_replace('[/brush]', '</pre>', $value);
	
		return $value;
	}
	function changeToPre($value){
		$value = str_replace("<", "&#60;", $value);
		return $value;
	}
	function changeToHtml($value){
		$value = str_replace(" ", "&nbsp;", $value);
		$value = str_replace("<", "&#60;", $value);
		$value = str_replace(">", "&#62;", $value);
		$value = str_replace("\n", "<br />", $value);
		return $value;
	}
	function checkName($value){
		$value = str_replace("lufy", "***", strtolower($value));
		return $value;
	}
	function getHtml($value){
		$value = str_replace(" ", "&nbsp;", $value);
		$value = str_replace("<", "&#60;", $value);
		//$value = str_replace(">", "&#62;", $value);
		$value = str_replace("&#60;pre ", "<pre ", $value);
		$value = str_replace("&#60;/pre>", "</pre>", $value);
		
		$value = str_replace("[Enter]", "<br />", $value);
		$value_a = explode("[a]", $value);
		$value = $value_a[0];
		for($i=1;$i<count($value_a);$i++){
			$value_aend = explode("[/a]", $value_a[$i]);
			$value .= '<a href="'.$value_aend[0].'" target="_blank">'.$value_aend[0].'</a>'.$value_aend[1];
		}
		echo $value;
	}
	function getIp(){
		if (@$_SERVER['HTTP_CLIENT_IP'] && $_SERVER['HTTP_CLIENT_IP']!='unknown') {   
			$ip = $_SERVER['HTTP_CLIENT_IP'];   
		} elseif (@$_SERVER['HTTP_X_FORWARDED_FOR'] && $_SERVER['HTTP_X_FORWARDED_FOR']!='unknown') {   
			$ip = $_SERVER['HTTP_X_FORWARDED_FOR'];   
		} else {   
			$ip = $_SERVER['REMOTE_ADDR'];   
		}   
		return $ip;
	}
	*/
}