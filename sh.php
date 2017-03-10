<?php
/*
sh.php?class=XXX&method=XXX
*/
$controller_p = $_GET["class"];
$method = $_GET["method"];
$class = ucfirst($controller_p);
require_once("sh/core/Base_Database.php");
require_once("sh/databases/master_db.php");
require_once("sh/databases/user_db.php");
require_once("sh/helpers/common_helper.php");
require_once("sh/core/MY_Controller.php");
require_once("sh/core/MY_Model.php");
require_once("sh/controllers/{$controller_p}.php");
$controller = new $class();
$controller->$method();

?>
