<?php 
class Base_Database {
	var $connect;
	//function Base_Database() {
	function __construct() {
		$this->connect = mysql_connect('localhost', 'lufy', '3gj_lufy');
		if (!$this->connect) {
			throw new Exception('' + mysql_error());
			die('');
		}
		mysql_set_charset('utf8', $this->connect);
	}
	public function close(){
		mysql_close($this->connect);
	}
	public function selectSQL($sql){
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
	public function select($select, $table, $where = null, $order = null, $limit = null, $result_type = Database_Result::TYPE_ARRAY){
		$sql = "";
		if(is_array($select)){
			$sql .= "SELECT " . implode(",", $select);
		}else{
			$sql .= "SELECT " . $select;
		}
		if(is_array($table)){
			$sql .= " FROM " . implode(",", $table);
		}else{
			$sql .= " FROM " . $table;
		}
		if(!is_null($where)){
			$sql .= " WHERE " . implode(" AND ", $where);
		}
		if(!is_null($order)){
			$sql .= " ORDER BY " . (is_array($order) ? implode(" , ", $order) : $order);
		}
		if(!is_null($limit)){
			$sql .= " LIMIT " . $limit;
		}
		$result_select = mysql_query($sql, $this->connect);
		if($result_type == Database_Result::TYPE_ARRAY){
			$result = array();
			while ($row = mysql_fetch_assoc($result_select)) {
				$result[] = $row;
			}
			return $result;
		}else if($result_type == Database_Result::TYPE_ROW){
			return mysql_fetch_assoc($result_select);
		}
		return $result_select;
	}
}
