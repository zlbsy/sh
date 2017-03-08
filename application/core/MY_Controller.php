<?php class MY_Controller extends CI_Controller {
	var $args;
	var $needAuth = false;
	function MY_Controller() {
		date_default_timezone_set('PRC');
		parent::__construct();
		//$this->load->library('viewloader');
		//$this->set_viewdata("page",strtolower($this->uri->segment(1)));
		($this->args = $this->input->get()) || ($this->args = $this->input->post());
		if($this->args["ssid"]){
			session_id($this->args["ssid"]);
		}else{
			session_id($this->session->userdata('session_id'));
		}
		session_start();
		if(!$this->args["ssid"]){
			$_SESSION = array();
		}
		if($this->needAuth){
			$this->checkAuth();
		}
		/*
		$now_page = strtolower($this->uri->segment(1));
		switch($now_page){
			case "lufylegend":
				$this->set_viewdata("html_title","html5开源引擎");
				break;
			case "forum":
				$this->set_viewdata("html_title","讨论交流区");
				break;
			case "move":
				$this->set_viewdata("html_title","开发视频");
				break;
		}*/
		/*
		if($_GET["PHPSESSID"])session_id($_GET["PHPSESSID"]);
		session_start();
		date_default_timezone_set('Asia/Tokyo');
		$this->load->library('authentication');
		
		//現在の画面確認
		$mcheck_page = strtolower($this->uri->segment(1));
		$api_status_check = $this->api_connection_model->checkIfLiveAPI();
		//$api_status_check = "db";


		//サービスステータス、DB状況によってメンテ画面切り替え
		if(SERVICE_STATUS == "maintenance" || $api_status_check == "db") {
			if($mcheck_page != "maintenance") {
				redirect(base_url().'maintenance');
			}
		}

		//通常時のメンテ画面へのアクセスを拒否
		elseif(SERVICE_STATUS != "maintenance" && $api_status_check != "db" && $mcheck_page == "maintenance") {
			redirect(base_url().'top');	
		}

		$api_searchword = $this->api_connection_model->getSearchWord();
		$agent_switcher = $this->agent->mobile();
		//print_r($agent_switcher);
		$smart_phones_array = array('android2', 'android3', 'android4','android5', 'iOS', 'iPod', 'iPad');

		//ユーザーエージェントを使った切り替えと転送
		//スマフォ配列に入ってる奴の場合はそのまま
		if (in_array($agent_switcher, $smart_phones_array)) {
			define('AGENT_SWITCHER',$agent_switcher);
			$is_logeend_in = $this->auth_model->loggedIn();
			if(!defined('LOGGED_IN')) {
				define('LOGGED_IN', $is_logeend_in);
			}
		}
		//携帯なんだけど、スマフォ配列には入っていないー>つまり非対応の携帯またはスマフォ
		elseif(!in_array($agent_switcher, $smart_phones_array) && $agent_switcher != "") {
			//mobileと定義
			define('AGENT_SWITCHER', 'mobile');
			//転送301
//			redirect(DORUBAKO_PATH.'m/event/smartphone.php','location', 301);
		}
		//それ以外はPC扱い
		else {
			//PCと定義
			define('AGENT_SWITCHER', 'PC');
			if($mcheck_page == "register" && $this->uri->total_segments() == 4){
				//友達紹介
				$introducer = $this->uri->segment(4);
				if($introducer != null && is_numeric($introducer) && $introducer > 0)
					redirect(DORUBAKO_PATH."regist2.php?i=".$introducer,'location', 301);
			}
			// 転送301
			redirect(DORUBAKO_PATH,'location', 301);
		}
		if(strstr(strtoupper($this->agent->agent_string()), "SC-01C")) {
			//GALAXY Tabを　PCと定義
			define('AGENT_SWITCHER', 'PC');
			if($mcheck_page == "register" && $this->uri->total_segments() == 4) {
				//友達紹介
				$introducer = $this->uri->segment(4);
				if($introducer != null && is_numeric($introducer) && $introducer > 0)
					redirect(DORUBAKO_PATH."regist2.php?i=".$introducer,'location', 301);
			}
			// 転送301
			redirect(DORUBAKO_PATH,'location', 301);
		}
		if($this->session->userdata('memberid') != "") {
			//最後のログインから２４時間過ぎたら自動ログインさせる
			$last_login = $this->session->userdata('lastlogin');
			$now = now();
			$difference = $now-$last_login;			
			if($difference >= '86400') //24h = 86400 seconds
			{
				$pid = $this->session->userdata('pid');
				$memberid = $this->session->userdata('memberid');
				$login_result = $this->auth_model->logMeInAuto($pid, $memberid);
				$alert = $login_result['result']['ret'];
				$result_arr = $login_result['result']['data'];
				if($alert == "error") {
					$this->auth_model->doLogout();
					redirect(base_url().'auth');
				}
			} elseif($alert == "success") {
				$user_data = $this->api_connection_model->getMyData();
				$this->auth_model->setLoginSession($result_arr, $user_data['email']);
			}
		} else {
		}
		$this->ids = $this->GetId();
		$this->set_viewdata("user_data",$this->getSessionData("user_data"));
		*/
	}
	protected function error($message){
		die(json_encode(array("result"=>0,"message"=>$message)));
	}
	protected function out($arr){
		if(empty($arr)){
			$arr = array();
		}
		$arr["now"] = date("Y-m-d H:i:s");
		$arr["result"] = 1;
		die(json_encode(array("result"=>1,"data"=>$arr)));
		//die(json_encode($arr));
		//$this->load->view('out', array(("out_list" => $arr)));
		//$this->set_viewname("out");
	}
	protected function checkParam($list){
		foreach ($list as $value) {
			if(!isset($this->args[$value])){
				$this->error("Param error : " . $value);
			}
		}
	}
	protected function set_layoutname($_layoutname){
		ViewLoader::set_layoutname($_layoutname);
	}
	
	protected function set_viewname($_viewname){
		ViewLoader::set_viewname($_viewname);
	}
	
	protected function set_viewdata($_viewdata,$_viewdata_value = null){
		$this->load->vars($_viewdata,$_viewdata_value);
	}
	protected function getSessionData($key){
		//return $this->session->userdata($key);
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		//$this->session->set_userdata($key,$value);
		$_SESSION[$key] = $value;
	}
	protected function checkAuth(){
		$user = $this->getSessionData("user");
		if(empty($user)){
			$this->error("sessionError");
		}
	}
	/*
	function error($params) {
		$this->set_viewdata("err_title",$params["err_title"]);
		$this->set_viewdata("err_message",$params["err_message"]);
		$this->set_viewdata("prev_page_link",$params["prev_page_link"]);
		$this->set_viewdata("prev_page",$params["prev_page"]);
		$this->set_viewname("error/error");
	}
	function isEmpty($var) {
	    $rtn_val = false;
	
	    if (is_null($var)) {
	        $rtn_val = true;
	    } else if ($var === "") {
	        $rtn_val = true;
	    }
	
	    return $rtn_val;
	}
	function maintenance(){
		redirect(base_url().'maintenance');
		
	}

	
	protected function set_pagination($arr_src, $per_page = 10, $base_url = null){
		$pagenation_config = array(
					'total_rows' => is_array($arr_src)?count($arr_src):$arr_src,
					'per_page' => $per_page
		);
		if(isset($base_url)) {
			$pagenation_config['base_url'] = $base_url;
		}
		$this->paginationhapitas->initialize($pagenation_config);
		$this->set_viewdata("pagination", $this->paginationhapitas->create_links());
	}
	function GetId(){
		require_once('Net/UserAgent/Mobile.php');
	
		$agent = Net_UserAgent_Mobile::singleton();
		$Carrier = OTH;
		$SerialNumber = '';
		$CardID = '';
		$UID = '';
		if ($agent->isDoCoMo())
		{
			$Carrier = DCM;
			$SerialNumber = $agent->getSerialNumber();
			$CardID = $agent->getCardID();
			$UID = $agent->getUID();
		}
		else if ($agent->isEZweb())
		{
			$Carrier = AU;
			$UID = $agent->getUID();
			$SerialNumber = $UID;
		}
		else if ($agent->isSoftBank())
		{
			$Carrier = SB;
			$SerialNumber = $agent->getSerialNumber();
			$UID = $agent->getUID();
		}
		else if ($agent->isWillcom())
		{
		}
	
		$id = array();
		$id['CARRIER'] = $Carrier;
		$id['SERIAL_NUMBER'] = $SerialNumber;
		$id['CARD_ID'] = $CardID;
		$id['UID'] = $UID;
		return $id;
	}
	*/
}
