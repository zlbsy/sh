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
		if(isset($_GET["debug"])){
			echo $sql;
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
	public function updateSQL($sql){
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
	public function update($values, $table, $where){
		$sql = "UPDATE " . $table;
		$sql .= " (".implode(", ", $values).") "
		$sql .= " WHERE " . implode(" AND ", $where);
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
	public function insertSQL($sql){
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
	public function insert($values, $table){
		$sql = "INSERT INTO " . $table;
		$child_names = array();
		$child_values = array();
		foreach ($values as $key => $value) {
			$child_names[] = $key;
			$child_values[] = $value;
		}
		$sql .= " (".implode(", ", $child_names).") "
		$sql .= " VALUES ";
		$sql .= " (".implode(", ", $child_values).") "
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
}
