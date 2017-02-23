<?php
if (!defined('BASEPATH'))exit('No direct script access allowed');
if (!function_exists('multidimensional_search')) {
	function multidimensional_search($parents, $searched) {
		if (empty($searched) || empty($parents)) {
			return false;
		}

		foreach ($parents as $key => $value) {
			$exists = true;
			foreach ($searched as $skey => $svalue) {
				$exists = ($exists && IsSet($parents[$key][$skey]) && $parents[$key][$skey] == $svalue);
			}
			if ($exists) {
				return $key;
			}
		}

		return false;
	}

}
