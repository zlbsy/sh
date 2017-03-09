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
	public function select($sql){
		$result = mysql_query($sql, $this->connect);
		return $result;
	}
}
