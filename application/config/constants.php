<?php  if ( ! defined('BASEPATH')) exit('No direct script access allowed');

/*
|--------------------------------------------------------------------------
| File and Directory Modes
|--------------------------------------------------------------------------
|
| These prefs are used when checking and setting modes when working
| with the file system.  The defaults are fine on servers with proper
| security, but you may wish (or even need) to change the values in
| certain environments (Apache running a separate process for each
| user, PHP under CGI with Apache suEXEC, etc.).  Octal values should
| always be used to set the mode correctly.
|
*/
define('FILE_READ_MODE', 0644);
define('FILE_WRITE_MODE', 0666);
define('DIR_READ_MODE', 0755);
define('DIR_WRITE_MODE', 0777);

/*
|--------------------------------------------------------------------------
| File Stream Modes
|--------------------------------------------------------------------------
|
| These modes are used when working with fopen()/popen()
|
*/

define('FOPEN_READ',							'rb');
define('FOPEN_READ_WRITE',						'r+b');
define('FOPEN_WRITE_CREATE_DESTRUCTIVE',		'wb'); // truncates existing file data, use with care
define('FOPEN_READ_WRITE_CREATE_DESTRUCTIVE',	'w+b'); // truncates existing file data, use with care
define('FOPEN_WRITE_CREATE',					'ab');
define('FOPEN_READ_WRITE_CREATE',				'a+b');
define('FOPEN_WRITE_CREATE_STRICT',				'xb');
define('FOPEN_READ_WRITE_CREATE_STRICT',		'x+b');


define('DAY_START', date("Y-m-d 05:00:00", date("H",time()) > "05" ? time() : strtotime("-1 day")));

define('MASTER_DATABASE', 'fsyy_master_lufylegend');
define('USER_DATABASE', 'fsyy_lufylegend');

define('USER_PLAYER', USER_DATABASE.'.player');
define('USER_ITEM', USER_DATABASE.'.item');
define('USER_CHARACTERS', USER_DATABASE.'.characters');
define('USER_BANKBOOK', USER_DATABASE.'.bankbook');
define('USER_AREA', USER_DATABASE.'.area');
define('USER_STAGE', USER_DATABASE.'.stage');
define('USER_EQUIPMENT', USER_DATABASE.'.equipment');
define('USER_TOP_MAP', USER_DATABASE.'.top_map');

define('MASTER_BASE_CHARACTER', MASTER_DATABASE.'.base_character');
define('MASTER_TILE', MASTER_DATABASE.'.tile');
define('MASTER_BASE_MAP', MASTER_DATABASE.'.base_map');
define('MASTER_BUILDING', MASTER_DATABASE.'.building');
define('MASTER_CONSTANT', MASTER_DATABASE.'.constant');
define('MASTER_VERSION', MASTER_DATABASE.'.version');
define('MASTER_WORLD', MASTER_DATABASE.'.world');
define('MASTER_AREA', MASTER_DATABASE.'.area');
define('MASTER_HORSE', MASTER_DATABASE.'.horse');
define('MASTER_CLOTHES', MASTER_DATABASE.'.clothes');
define('MASTER_WEAPON', MASTER_DATABASE.'.weapon');
define('MASTER_GACHA', MASTER_DATABASE.'.gacha');
define('MASTER_GACHA_CHILD', MASTER_DATABASE.'.gacha_child');

define('MASTER_CHARACTER_STAR', MASTER_DATABASE.'.character_star');
define('MASTER_GROWING', MASTER_DATABASE.'.growing');
define('MASTER_ITEM', MASTER_DATABASE.'.item');
define('MASTER_CHAPTER', MASTER_DATABASE.'.chapter');
define('MASTER_AREA', MASTER_DATABASE.'.area');
define('MASTER_AREA_GET', MASTER_DATABASE.'.area_get');
define('MASTER_STAGE', MASTER_DATABASE.'.stage');
define('MASTER_STAGE_CHARACTER', MASTER_DATABASE.'.stage_character');
define('MASTER_STAGE_GET', MASTER_DATABASE.'.stage_get');
define('MASTER_EXP', MASTER_DATABASE.'.exp');
define('MASTER_GROWING', MASTER_DATABASE.'.growing');
define('MASTER_EQUIPMENT', MASTER_DATABASE.'.equipment');
define('MASTER_SKILL', MASTER_DATABASE.'.skill');




/* End of file constants.php */
/* Location: ./application/config/constants.php */
